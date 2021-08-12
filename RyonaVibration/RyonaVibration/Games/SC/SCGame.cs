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

        public bool HasRecentlyTriggeredRoundLoss { get; set; } = false;

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
                if (HasRecentlyTriggeredRoundLoss)
                {
                    HasRecentlyTriggeredRoundLoss = false;
                    return;
                }
                vibratorController.SendVibration(new SpeedTime(1, 15000, true));
                vibratorController.PublishLogs($"{nameof(player.RoundEndedLoss)}: {val}");
                HasRecentlyTriggeredRoundLoss = true;
            };

            for (int i = 1; i < 5; i++)
            {
                if(i != playerNumber)
                {
                    var tempPlayer = GetPlayerByNumber(i);
                    if (tempPlayer != null)
                    {
                        tempPlayer.UnsubscribeFromAllEvents();
                    }
                }
            }
        }
    }
}
