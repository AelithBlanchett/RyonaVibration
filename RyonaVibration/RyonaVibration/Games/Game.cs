using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RyonaVibration.Games
{
    public abstract class Game<T> : IDisposable where T : PlayerStats, new()
    {
        public Memory.Mem Mem { get; set; } = new Memory.Mem();

        public bool Attached { get; set; } = false;

        public string ProcessId { get; set; } = string.Empty;

        public string ProcessName { get; set; } = "";

        public string GameName { get; set; } = "";

        public int PollingRate { get; set; } = 100;

        public T Player1 { get; set; } = new T();

        public T Player2 { get; set; } = new T();

        public T Player3 { get; set; } = new T();

        public T Player4 { get; set; } = new T();

        public Game(string processName, string gameName)
        {
            ProcessName = processName;
            GameName = gameName;
        }

        public async Task StartListening(int playerNumber)
        {
            while (Attached && Mem.theProc != null && !Mem.theProc.HasExited)
            {
                var stats = ReadEventForPlayerNumber(playerNumber);
                //rtbLogs.AppendText("\n--------------------\n");
                //foreach (var prop in stats.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                //{
                //    if (prop.GetValue(stats) != null)
                //    {
                //        //rtbLogs.AppendText("\n" + prop.Name + ": " + prop.GetValue(stats).ToString());
                //    }
                //}
                await Task.Delay(PollingRate);
            }
        }

        public void AttachToGame()
        {
            int gameProcId = Mem.GetProcIdFromName(ProcessName.Replace(".exe", ""));

            if (gameProcId != 0)
            {
                Attached = true;
                ProcessId = gameProcId.ToString();
                if (!Mem.OpenProcess(gameProcId))
                {
                    MessageBox.Show("Couldn't attach to game.");
                }
            }
            else
            {
                Attached = false;
                MessageBox.Show("The game isn't running yet.");
            }
        }

        public abstract void AttachListenersForPlayerNumber(VibratorController vibratorController, int playerNumber);

        public virtual T GetPlayerByNumber(int playerNumber)
        {
            T player = default;

            switch (playerNumber)
            {
                case 1:
                    player = Player1;
                    break;
                case 2:
                    player = Player2;
                    break;
                case 3:
                    player = Player3;
                    break;
                case 4:
                    player = Player4;
                    break;
                default:
                    break;
            }

            return player;
        }

        public virtual T ReadEventForPlayerNumber(int playerNumber)
        {
            var startKey = $"{GameName}.P{playerNumber}.";
            var keysToAssign = ConfigurationManager.AppSettings.AllKeys
                             .Where(key => key.StartsWith(startKey))
                             .Select(key => key)
                             .ToList();

            T player = GetPlayerByNumber(playerNumber);

            foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propKeyName = $"{GameName}.P1.{prop.Name}";
                if (keysToAssign.Contains(propKeyName))
                {
                    var address = ConfigurationManager.AppSettings[propKeyName];
                    if (prop.PropertyType == typeof(string))
                    {
                        var value = Mem.ReadString(address, length: 4).ToString();
                        prop.SetValue(player, value);
                    }
                    else if (prop.PropertyType == typeof(int))
                    {
                        var value = Mem.ReadInt(address);
                        prop.SetValue(player, value);
                    }
                    else if (prop.PropertyType == typeof(double))
                    {
                        var value = Mem.ReadDouble(address, round: false);
                        prop.SetValue(player, value);
                    }
                    else if (prop.PropertyType == typeof(decimal))
                    {
                        var value = Mem.ReadDouble(address, round: false);
                        prop.SetValue(player, value);
                    }
                    else if (prop.PropertyType == typeof(float))
                    {
                        var value = Mem.ReadFloat(address);
                        prop.SetValue(player, value);
                    }
                    else if (prop.PropertyType == typeof(bool))
                    {
                        var value = Mem.ReadByte(address);
                        prop.SetValue(player, value > 0);
                    }
                    else if (prop.PropertyType == typeof(long))
                    {
                        var value = Mem.ReadLong(address);
                        prop.SetValue(player, value);
                    }
                    else if (prop.PropertyType == typeof(byte))
                    {
                        var value = Mem.ReadByte(address).ToString();
                        prop.SetValue(player, byte.Parse(value));
                    }
                    else
                    {

                    }
                }
            }

            return player;
        }

        #region IDisposable Support
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
