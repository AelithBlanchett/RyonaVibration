using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RyonaVibration.Games
{
    public class AmazonBrawlHardcoreGame : Game<AmazonBrawlPlayerStats>
    {
        public AmazonBrawlHardcoreGame() : base("Amazon Brawl", "AmazonBrawl")
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
                vibratorController.SendVibration(new SpeedTime(val * 2, 2000));
                vibratorController.PublishLogs($"{nameof(player.HPHitReceived)}: {val}");
            };

            player.LPUpdated += (s, val) =>
            {
                vibratorController.PublishLogs($"{nameof(player.LPUpdated)}: {val}");
            };

            player.LPHitReceived += (s, val) =>
            {
                vibratorController.SendVibration(new SpeedTime(val * 2, 3000));
                vibratorController.PublishLogs($"{nameof(player.LPHitReceived)}: {val}");
            };

            player.HumiliationHPUpdated += (s, val) =>
            {
                vibratorController.PublishLogs($"{nameof(player.HumiliationHPUpdated)}: {val}");
            };

            player.HumiliationHPHitReceived += (s, val) =>
            {
                vibratorController.SendVibration(new SpeedTime(val, 3000));
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
        }
    }
}
