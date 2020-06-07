﻿using Buttplug.Client;
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


        public VibratorController VibratorController { get; set; } = new VibratorController();

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

        public RRXXGame RRXXGame { get; set; }

        private async void btnReadMemory_Click(object sender, EventArgs e)
        {
            //if(!VibratorController.Client.Devices.Any())
            //{
            //    MessageBox.Show("No sextoys paired yet.");
            //    //return;
            //}

            if (rbAMAZON.Checked)
            {
                if (AmazonBrawlHardcoreGame != null)
                {
                    AmazonBrawlHardcoreGame.Dispose();
                }
                AmazonBrawlHardcoreGame = new AmazonBrawlHardcoreGame();
                AmazonBrawlHardcoreGame.AttachToGame();
                AmazonBrawlHardcoreGame.AttachListenersForPlayerNumber(VibratorController, PlayerNumber);
                AmazonBrawlHardcoreGame.Player1.ValueUpdated += Player1_ValueUpdated;
                await AmazonBrawlHardcoreGame.StartListening(PlayerNumber, VibratorController);
            }
            else if (rbRRXX.Checked)
            {
                if(RRXXGame != null)
                {
                    RRXXGame.Dispose();
                }
                RRXXGame = new RRXXGame();
                RRXXGame.AttachToGame();
                RRXXGame.AttachListenersForPlayerNumber(VibratorController, PlayerNumber);

                //DEBUG
                VibratorController.NewLogsPublished += Client_NewLogsPublished;
                RRXXGame.Player1.ValueUpdated += Player1_ValueUpdated;



                await RRXXGame.StartListening(PlayerNumber, VibratorController);
            }
        }

        private void Player1_ValueUpdated(object sender, Tuple<string, string> e)
        {
            Client_NewLogsPublished(this, e.ToString());
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnEmergency_Click(this, null);
        }
    }
}
