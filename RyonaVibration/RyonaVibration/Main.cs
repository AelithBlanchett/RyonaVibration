using Buttplug.Client;
using Buttplug.Core;
using Buttplug.Core.Logging;
using Buttplug.Core.Messages;
using RyonaVibration.Games;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RyonaVibration
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            await Connect();
        }

        public static StringBuilder Logs { get; set; } = new StringBuilder();


        public VibratorController VibratorController { get; set; }

        private async Task Connect()
        {
            VibratorController = new VibratorController();
            VibratorController.NewLogsPublished += Client_NewLogsPublished;
            await VibratorController.Initialize();
        }

        private void Client_NewLogsPublished(object sender, string e)
        {
            rtbLogs.AppendText(e+"\n");
        }

        private async void btnTestVibrate_Click(object sender, EventArgs e)
        {
            await VibratorController.TestDevice(1);
        }

        private void rtbLogs_TextChanged(object sender, EventArgs e)
        {
            rtbLogs.SelectionStart = rtbLogs.Text.Length;
            rtbLogs.ScrollToCaret();
        }

        private async void btnEmergency_Click(object sender, EventArgs e)
        {
            if(VibratorController != null)
            {
                await VibratorController.EmergencyStop();
            }
            Application.Exit();
        }

        public int PlayerNumber
        {
            get
            {
               return rbLeft.Checked ? 1 : 2;
            }
        }

        public AmazonBrawlHardcoreGame AmazonBrawlHardcoreGame { get; set; }

        private async void btnReadMemory_Click(object sender, EventArgs e)
        {
            if(VibratorController == null)
            {
                MessageBox.Show("No sextoys paired yet.");
                return;
            }
            if (rbAMAZON.Checked)
            {
                AmazonBrawlHardcoreGame = new AmazonBrawlHardcoreGame();
                AmazonBrawlHardcoreGame.AttachToGame();

                AmazonBrawlHardcoreGame.Player1.HPUpdated += (s, val) =>
                {
                    Client_NewLogsPublished(this, $"{nameof(AmazonBrawlHardcoreGame.Player1.HPUpdated)}: {val}");
                };

                AmazonBrawlHardcoreGame.Player1.HPHitReceived += (s, val) =>
                {
                    VibratorController.SendVibration(new SpeedTime(val*2, 2000));
                    Client_NewLogsPublished(this, $"{nameof(AmazonBrawlHardcoreGame.Player1.HPHitReceived)}: {val}");
                };

                AmazonBrawlHardcoreGame.Player1.LPUpdated += (s, val) =>
                {
                    Client_NewLogsPublished(this, $"{nameof(AmazonBrawlHardcoreGame.Player1.LPUpdated)}: {val}");
                };

                AmazonBrawlHardcoreGame.Player1.LPHitReceived += (s, val) =>
                {
                    VibratorController.SendVibration(new SpeedTime(val*2, 3000));
                    Client_NewLogsPublished(this, $"{nameof(AmazonBrawlHardcoreGame.Player1.LPHitReceived)}: {val}");
                };

                AmazonBrawlHardcoreGame.Player1.HumiliationHPUpdated += (s, val) =>
                {
                    Client_NewLogsPublished(this, $"{nameof(AmazonBrawlHardcoreGame.Player1.HumiliationHPUpdated)}: {val}");
                };

                AmazonBrawlHardcoreGame.Player1.HumiliationHPHitReceived += (s, val) =>
                {
                    VibratorController.SendVibration(new SpeedTime(val, 3000));
                    Client_NewLogsPublished(this, $"{nameof(AmazonBrawlHardcoreGame.Player1.HumiliationHPHitReceived)}: {val}");
                };

                AmazonBrawlHardcoreGame.Player1.OrgasmStarted += (s, val) =>
                {
                    VibratorController.SendVibration(new SpeedTime(1, 60000));
                    Client_NewLogsPublished(this, $"{nameof(AmazonBrawlHardcoreGame.Player1.OrgasmStarted)}: {val}");
                };

                AmazonBrawlHardcoreGame.Player1.OrgasmEnded += (s, val) =>
                {
                    Client_NewLogsPublished(this, $"{nameof(AmazonBrawlHardcoreGame.Player1.OrgasmEnded)}: {val}");
                };

                AmazonBrawlHardcoreGame.Player1.SubmissionStarted += (s, val) =>
                {
                    VibratorController.SendVibration(new SpeedTime(0.75, 60000));
                    Client_NewLogsPublished(this, $"{nameof(AmazonBrawlHardcoreGame.Player1.SubmissionStarted)}: {val}");
                };

                AmazonBrawlHardcoreGame.Player1.SubmissionEnded += (s, val) =>
                {
                    VibratorController.SendVibration(new SpeedTime(0, 1));
                    Client_NewLogsPublished(this, $"{nameof(AmazonBrawlHardcoreGame.Player1.SubmissionEnded)}: {val}");
                };

                AmazonBrawlHardcoreGame.Player1.RoundEndedLoss += (s, val) =>
                {
                    VibratorController.SendVibration(new SpeedTime(1, 4000));
                    Client_NewLogsPublished(this, $"{nameof(AmazonBrawlHardcoreGame.Player1.RoundEndedLoss)}: {val}");
                };

                while (AmazonBrawlHardcoreGame.Mem.theProc != null && !AmazonBrawlHardcoreGame.Mem.theProc.HasExited)
                {
                    var stats = AmazonBrawlHardcoreGame.ReadEventForPlayerNumber(PlayerNumber);
                    rtbLogs.AppendText("\n--------------------\n");
                    foreach (var prop in stats.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        if(prop.GetValue(stats) != null)
                        {
                            //rtbLogs.AppendText("\n" + prop.Name + ": " + prop.GetValue(stats).ToString());
                        }
                    }  
                    await Task.Delay(250);
                }
            }
        }
    }
}
