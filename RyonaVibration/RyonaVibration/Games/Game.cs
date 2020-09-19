using ManagedWinapi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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

        int readCounts = 0;

        public async Task StartListening(int playerNumber, VibratorController vibratorController)
        {
            while (Attached && Mem.theProc != null && !Mem.theProc.HasExited)
            {
                var stats = ReadEventForPlayerNumber(playerNumber);

                if(readCounts < 10)
                {
                    readCounts++;
                    vibratorController.IgnoreCommands = true;
                }
                else if(vibratorController.IgnoreCommands)
                {
                    vibratorController.IgnoreCommands = false;
                }

                await Task.Delay(PollingRate);
            }
        }

        public void StopListening()
        {
            Attached = false;
        }

        public void AttachToGame()
        {
            int gameProcId = Mem.GetProcIdFromName(ProcessName.Replace(".exe", ""));

            if (gameProcId != 0)
            {
                ProcessId = gameProcId.ToString();
                if (!Mem.OpenProcess(gameProcId))
                {
                    MessageBox.Show("Couldn't attach to game.");
                }
                else
                {
                    Attached = true;
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

        private const long HumStartAddress = 0x1B0674600;
        private const long HumEndAddress = 0x1B0674800;

        private static long GetMemoryAddressOfString(byte[] searchedBytes, Process p)
        {
            //List<int> addrList = new List<int>();
            long addr = 0;
            int speed = 1024 * 64;
            for (long j = HumStartAddress; j < HumEndAddress; j += speed)
            {
                ManagedWinapi.ProcessMemoryChunk mem = new ProcessMemoryChunk(p, (IntPtr)j, speed + searchedBytes.Length);

                byte[] bigMem = mem.Read();

                for (int k = 0; k < bigMem.Length - searchedBytes.Length; k++)
                {
                    bool found = true;
                    for (int l = 0; l < searchedBytes.Length; l++)
                    {
                        if (bigMem[k + l] != searchedBytes[l])
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        addr = k + j;
                        break;
                    }
                }
                if (addr != 0)
                {
                    //addrList.Add(addr);
                    //addr = 0;
                    break;
                }
            }
            //return addrList;
            return addr;
        }

        public static string FoundSubmissionAddress = "";

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

                    if(prop.Name == "SubmissionPercent" && FoundSubmissionAddress == "")
                    {
                        var addr = GetMemoryAddressOfString(new byte[] { 0x3F, 0x50, 0x00, 0x00 }, Mem.theProc);
                        FoundSubmissionAddress = (addr - 4).ToString("X"); // -4 because its 4 bytes before
                    }

                    if (prop.Name == "SubmissionPercent" && FoundSubmissionAddress != "")
                    {
                        address = FoundSubmissionAddress;
                    }

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
                        //var value = Mem.ReadFloat(address, round:false);
                        var value = Mem.ReadBytes(address, 4);
                        if (value != null)
                        {
                            var beValue = BitConverter.EndianBitConverter.BigEndian.ToSingle(value, 0);
                            var leValue = BitConverter.EndianBitConverter.LittleEndian.ToSingle(value, 0);
                            if (beValue > -1000 && beValue < 1000)
                            {
                                prop.SetValue(player, beValue);
                            }
                            else
                            {
                                prop.SetValue(player, leValue);
                            }
                        }
                        
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
