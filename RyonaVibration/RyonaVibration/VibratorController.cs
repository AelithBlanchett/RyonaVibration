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
        protected virtual void PublishLogs(string entry)
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
            PublishLogs($"Device connected: {aArgs.Device.Name}");
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
                await Task.Delay(1);
            }
            

            // Stop scanning now, 'cause we don't want new devices popping up anymore.
            await Client.StopScanningAsync();
        }

        // Now we define the device control menus. After we've scanned for devices, the user can
        // use this menu to select a device, then select an action for that device to take.
        public async Task TestDevice(int commandNumber)
        {
            // Controlling a device has 2 steps: selecting the device to control, and choosing
            // which command to send. We'll just list the devices the client has available, then
            // search the device message capabilities once that's done to figure out what we can
            // send. Note that this is using the Device Index, which is assigned by the device
            // manager and may not be sequential (which is why we can't just use an array index).

            // Of course, if we don't have any devices yet, that's not gonna work.
            if (!Client.Devices.Any())
            {
                PublishLogs("No devices available. Please scan for a device.");
                return;
            }

            var options = new List<uint>();

            foreach (var dev in Client.Devices)
            {
                PublishLogs($"{dev.Index}. {dev.Name}");
                options.Add(dev.Index);
            }

            var device = Client.Devices.First();

            // Now that we've gotten a device, we need to choose an action for that device to
            // take. For sake of simplicity, right now we'll just use the 3 generic commands available:
            //
            // - Vibrate
            // - Rotate
            // - Linear (stroke/oscillate)
            //
            // Each device supported by the Buttplug C# library supports at least one of these 3
            // commands, so we know that the user will always have some option.
            var commandTypes = device.AllowedMessages.Keys.Intersect(new[] { typeof(VibrateCmd), typeof(RotateCmd), typeof(LinearCmd) }).ToArray();

            // We've got a device, and a command to take on that device. Let's do this thing. For
            // each command we'll either run at a speed, then stop, or move to a position, then
            // back again. To ensure that we don't have to deal with concurrent commands (again,
            // for sake of example simplicity, real world situations are gonna be far more
            // dynamic than this), we'll just block while this action is happening.
            //
            // We'll wrap each of our commands in a ButtplugDeviceException try block, as a
            // device might be disconnected between the time we enter the command menu and send
            // the command, and we don't want to crash when that happens.
            var cmdType = commandTypes.First();

            // Pattern matching for switch blocks doesn't seem to work here. :(
            if (cmdType == typeof(VibrateCmd))
            {
                PublishLogs($"Vibrating all motors of {device.Name} at 50% for 1s.");
                try
                {
                    await device.SendVibrateCmd(0.5);
                    await Task.Delay(1000);
                    await device.SendVibrateCmd(0);
                }
                catch (ButtplugDeviceException)
                {
                    PublishLogs("Device disconnected. Please try another device.");
                }
            }
            else if (cmdType == typeof(RotateCmd))
            {
                PublishLogs($"Rotating {device.Name} at 50% for 1s.");
                try
                {
                    await device.SendRotateCmd(0.5, true);
                    await Task.Delay(1000);
                    await device.SendRotateCmd(0, true);
                }
                catch (ButtplugDeviceException)
                {
                    PublishLogs("Device disconnected. Please try another device.");
                }
            }
            else if (cmdType == typeof(LinearCmd))
            {
                PublishLogs($"Oscillating linear motors of {device.Name} from 20% to 80% over 3s");
                try
                {
                    await device.SendLinearCmd(1000, 0.2);
                    await Task.Delay(1100);
                    await device.SendLinearCmd(1000, 0.8);
                    await Task.Delay(1100);
                    await device.SendLinearCmd(1000, 0.2);
                    await Task.Delay(1100);
                }
                catch (ButtplugDeviceException)
                {
                    PublishLogs("Device disconnected. Please try another device.");
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
    }
}
