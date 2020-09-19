using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RyonaVibration.Games
{
    public class SCPlayerStats : PlayerStats
    {

        public const float MaxPhysicalHP = 240;

        private float physicalHP = 240;

        public float PhysicalHP { get => physicalHP;
            set
            {
                OnHPUpdated((int)physicalHP, (int)value, (int)MaxPhysicalHP);
                physicalHP = (int)value;
            }
        }
    }
}
