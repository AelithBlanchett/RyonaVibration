using Buttplug.Client;
using Buttplug.Core;
using Buttplug.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RyonaVibration
{
    public class VibratorController
    {
        private SynchronizationContext _context;

        public VibratorController()
        {
            var existingContext = SynchronizationContext.Current;
            _context = existingContext?.CreateCopy() ?? new SynchronizationContext();
        }
        public virtual void PublishLogs(string entry)
        {
            _context.Send(ContextCallback, entry);
        }

        private void ContextCallback(object entry)
        {
            NewLogsPublished?.Invoke(this, entry as string);
        }

        public ButtplugClient Client { get; set; }

        public StringBuilder Logs { get; set; } = new StringBuilder();

        public event EventHandler<string> NewLogsPublished;

        // Now we scan for devices. Since we didn't add any Subtype Managers yet, this will go
        // out and find them for us. They'll be reported in the logs as they are found.
        //
        // We'll scan for devices, and print any time we find one.
        void HandleDeviceAdded(object aObj, DeviceAddedEventArgs aArgs)
        {
            PublishLogs($"Device connected: {aArgs.Device.Name}");
        }

        void HandleDeviceRemoved(object aObj, DeviceRemovedEventArgs aArgs)
        {
            PublishLogs($"Device disconnected: {aArgs.Device.Name}");
        }

        // Here's the scanning part. Pretty simple, just scan until the user hits a button. Any
        // time a new device is found, print it so the user knows we found it.
        public async Task ScanForDevices()
        {
            PublishLogs("Scanning for devices. Found devices will be printed to console.");
            await Client.StartScanningAsync();

            while (!Client.Devices.Any())
            {
                PublishLogs("Scanning for devices....");
                await Task.Delay(500);
            }
            

            // Stop scanning now, 'cause we don't want new devices popping up anymore.
            await Client.StopScanningAsync();
        }

        public async Task EmergencyStop()
        {
            PublishLogs("Stopping everything!.");
            if(Client != null && Client.Devices.Any())
            {
                foreach (var device in Client.Devices)
                {
                    await device.StopDeviceCmd();
                }
            }
            
        }

        public async Task Initialize()
        {
            // As usual, we start off with our connector setup. We really don't need access to the
            // connector this time, so we can just pass the created connector directly to the client.
            Client = new ButtplugClient("ButtplugPort", new ButtplugEmbeddedConnector("ButtplugPort"));

            await Client.ConnectAsync();

            // At this point, if you want to see everything that's happening, uncomment this block to
            // turn on logging. Warning, it might be pretty spammy.

            //void HandleLogMessage(object aObj, LogEventArgs aArgs) { PublishLogs($"LOG: {aArgs.Message.LogMessage}"); }
            //client.Log += HandleLogMessage; await client.RequestLogAsync(ButtplugLogLevel.Debug);

            Client.DeviceAdded += HandleDeviceAdded;
            Client.DeviceRemoved += HandleDeviceRemoved;

            // Scan for devices before we get to the main menu.
            await ScanForDevices();
        }

        public static Guid CurrentTaskGuid { get; set; } = Guid.NewGuid();
        public static SpeedTime CurrentTaskSpeed { get; set; } = null;
        public bool IgnoreCommands { get; set; } = true;

        public static DateTime NextAvailableCommandDate { get; set; } = DateTime.MinValue;

        public async void SendVibration(SpeedTime e)
        {
            if(CurrentTaskSpeed == null)
            {
                CurrentTaskSpeed = e;
            }
            else if(CurrentTaskSpeed.SpeedInPercent > e.SpeedInPercent && NextAvailableCommandDate > DateTime.Now && e.Force == false)
            {
                PublishLogs($"SKIPPED lower vibration of {e.SpeedInPercent * 100d}% for {e.TimeInMs}ms");
                return;
            }

            if (IgnoreCommands)
            {
                PublishLogs($"SKIPPED FIRST COMMANDS vibration of {e.SpeedInPercent * 100d}% for {e.TimeInMs}ms");
                return;
            }

            var generatedGuid = Guid.NewGuid();
            CurrentTaskGuid = generatedGuid;
            NextAvailableCommandDate = DateTime.Now.AddMilliseconds(e.TimeInMs);

            if(Client == null)
            {
                PublishLogs($"SENT vibration of {e.SpeedInPercent * 100d}% for {e.TimeInMs}ms");
                return;
            }

            foreach (var device in Client.Devices)
            {
                var commandTypes = device.AllowedMessages.Keys.Intersect(new[] { typeof(VibrateCmd), typeof(RotateCmd), typeof(LinearCmd) }).ToArray();

                foreach (var cmdType in commandTypes)
                {
                    if (cmdType == typeof(VibrateCmd))
                    {
                        PublishLogs($"Vibrating all motors of {device.Name} at {e.SpeedInPercent*100d}% for 1s.");
                        try
                        {
                            await device.SendVibrateCmd(e.SpeedInPercent);
                            await Task.Delay(e.TimeInMs);
                            if(CurrentTaskGuid == generatedGuid)
                            {
                                await device.SendVibrateCmd(0);
                            }
                        }
                        catch (ButtplugDeviceException)
                        {
                            PublishLogs("Device disconnected. Please try another device.");
                        }
                    }
                    else if (cmdType == typeof(RotateCmd))
                    {
                        PublishLogs($"Rotating {device.Name} at {e.SpeedInPercent * 100d}% for 1s.");
                        try
                        {
                            await device.SendRotateCmd(e.SpeedInPercent, true);
                            await Task.Delay(e.TimeInMs);
                            if (CurrentTaskGuid == generatedGuid)
                            {
                                await device.SendRotateCmd(0, true);
                            }
                        }
                        catch (ButtplugDeviceException)
                        {
                            PublishLogs("Device disconnected. Please try another device.");
                        }
                    }
                    else if (cmdType == typeof(LinearCmd))
                    {
                        PublishLogs($"Oscillating linear motors of {device.Name} to {e.SpeedInPercent * 100d}%");
                        try
                        {
                            await device.SendLinearCmd((uint)e.TimeInMs, e.SpeedInPercent);
                            await Task.Delay(e.TimeInMs);
                            if (CurrentTaskGuid == generatedGuid)
                            {
                                await device.SendLinearCmd((uint)e.TimeInMs, 0);
                                await Task.Delay(e.TimeInMs);
                            }
                        }
                        catch (ButtplugDeviceException)
                        {
                            PublishLogs("Device disconnected. Please try another device.");
                        }
                    }
                }
            }
        }
    }
}
