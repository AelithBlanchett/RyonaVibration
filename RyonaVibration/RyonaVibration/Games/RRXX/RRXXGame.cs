﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RyonaVibration.Games
{
    public class RRXXGame : Game<RRXXPlayerStats>
    {
        public RRXXGame() : base("xenia-canary", "RRXX")
        {

        }

        public override void AttachListenersForPlayerNumber(VibratorController vibratorController, int playerNumber)
        {
            var player = GetPlayerByNumber(playerNumber);

            player.HPUpdated += (s, val) =>
            {
                vibratorController.PublishLogs($"{nameof(player.HPUpdated)}: {val}");
            };

            player.HPHitReceived += (s, val) =>
            {
                vibratorController.SendVibration(new SpeedTime(val * 4, 5000));
                vibratorController.PublishLogs($"{nameof(player.HPHitReceived)}: {val}");
            };

            player.LPUpdated += (s, val) =>
            {
                vibratorController.SendVibration(new SpeedTime(val, 5000));
                vibratorController.PublishLogs($"{nameof(player.LPUpdated)}: {val}");
            };

            player.LPHitReceived += (s, val) =>
            {
                vibratorController.PublishLogs($"{nameof(player.LPHitReceived)}: {val}");
            };

            player.HumiliationHPUpdated += (s, val) =>
            {
                vibratorController.PublishLogs($"{nameof(player.HumiliationHPUpdated)}: {val}");
            };

            player.HumiliationHPHitReceived += (s, val) =>
            {
                vibratorController.SendVibration(new SpeedTime(val * 4, 5000));
                vibratorController.PublishLogs($"{nameof(player.HumiliationHPHitReceived)}: {val}");
            };

            player.OrgasmStarted += (s, val) =>
            {
                vibratorController.SendVibration(new SpeedTime(1, 60000));
                vibratorController.PublishLogs($"{nameof(player.OrgasmStarted)}: {val}");
            };

            player.OrgasmEnded += (s, val) =>
            {
                vibratorController.PublishLogs($"{nameof(player.OrgasmEnded)}: {val}");
            };

            player.SubmissionStarted += (s, val) =>
            {
                vibratorController.SendVibration(new SpeedTime(0.75, 60000));
                vibratorController.PublishLogs($"{nameof(player.SubmissionStarted)}: {val}");
            };

            player.SubmissionEnded += (s, val) =>
            {
                vibratorController.SendVibration(new SpeedTime(0, 1));
                vibratorController.PublishLogs($"{nameof(player.SubmissionEnded)}: {val}");
            };

            player.RoundEndedLoss += (s, val) =>
            {
                vibratorController.SendVibration(new SpeedTime(1, 4000));
                vibratorController.PublishLogs($"{nameof(player.RoundEndedLoss)}: {val}");
            };

            player.HumOrgasmStarted += (s, val) =>
            {
                vibratorController.SendVibration(new SpeedTime(1, 600000));
                vibratorController.PublishLogs($"{nameof(player.OrgasmStarted)}: {val}");
            };

            player.HumOrgasmEnded += (s, val) =>
            {
                vibratorController.PublishLogs($"{nameof(player.OrgasmEnded)}: {val}");
            };

            //This means the opponent got a point
            player.LifeRefilled += (s, val) =>
            {
                vibratorController.SendVibration(new SpeedTime(0.75, 3000));
                vibratorController.PublishLogs($"{nameof(player.LifeRefilled)}: {val}");
            };
        }
    }
}
