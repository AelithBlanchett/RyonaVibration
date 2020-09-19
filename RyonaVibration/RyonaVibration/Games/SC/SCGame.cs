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
    public class SCGame : Game<SCPlayerStats>
    {
        public SCGame() : base("SoulcaliburVI", "SoulCaliburVI")
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
                vibratorController.SendVibration(new SpeedTime(val * 3, 2000));
                vibratorController.PublishLogs($"{nameof(player.HPHitReceived)}: {val}");
            };

            player.RoundEndedLoss += (s, val) =>
            {
                vibratorController.SendVibration(new SpeedTime(1, 10000, true));
                vibratorController.PublishLogs($"{nameof(player.RoundEndedLoss)}: {val}");
            };
        }
    }
}
