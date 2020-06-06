using Buttplug.Client;
using Buttplug.Core;
using Buttplug.Core.Logging;
using Buttplug.Core.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        private void btnConnect_Click(object sender, EventArgs e)
        {
            Connect();
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

        private async void btnScan_Click(object sender, EventArgs e)
        {
            await VibratorController.ScanForDevices();
        }

        private void rtbLogs_TextChanged(object sender, EventArgs e)
        {
            rtbLogs.SelectionStart = rtbLogs.Text.Length;
            rtbLogs.ScrollToCaret();
        }
    }
}
