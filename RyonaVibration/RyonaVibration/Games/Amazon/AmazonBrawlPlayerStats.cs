using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RyonaVibration.Games
{
    public class AmazonBrawlPlayerStats : PlayerStats
    {

        public const int MaxPhysicalHP = 1000;
        public const int MaxLustPoints = 200;
        public const int MaxPinningOrgasm = 30;

        private int physicalHP = 1000;
        private byte lustPercentage = 0;
        private byte isPinned = 0;
        private byte pinningOrgasm = 0;

        public int PhysicalHP { get => physicalHP;
            set
            {
                OnHPUpdated(physicalHP, value, MaxPhysicalHP);
                physicalHP = value;
            }
        }

        public byte LustPercentage { get => lustPercentage;
            set
            {
                OnLPUpdated(lustPercentage, value, MaxLustPoints);
                lustPercentage = value;
            }
        }

        public byte IsPinned { get => isPinned;
            set
            {
                if(isPinned == 6 && value != 6)
                {
                    OnSubmissionEnded();
                }
                else if (isPinned != 6 && value == 6)
                {
                    OnSubmissionStarted();
                }
                isPinned = value;
            }
        }

        public byte PinningOrgasm
        {
            get => pinningOrgasm;
            set
            {
                if(value == MaxPinningOrgasm)
                {
                    OnOrgasmStarted();
                }
                else if (value < pinningOrgasm)
                {
                    OnOrgasmEnded();
                }
                pinningOrgasm = value;
            }
        }

        public int PinningOrgasmPercentage
        {
            get
            {
                return 100 * PinningOrgasm / 30;
            }
        }
    }
}
