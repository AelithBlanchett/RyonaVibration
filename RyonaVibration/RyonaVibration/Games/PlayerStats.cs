using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RyonaVibration.Games
{
    public class PlayerStats
    {

        public event EventHandler OrgasmStarted;

        public event EventHandler OrgasmEnded;

        public event EventHandler SubmissionStarted;

        public event EventHandler SubmissionEnded;

        public event EventHandler RoundEndedLoss;

        //Percentage of total HP lost
        public event EventHandler<int> HPUpdated;

        //Percentage of HP lost / Total HP
        public event EventHandler<int> HPHitReceived;

        //Percentage of LP lost
        public event EventHandler<int> LPUpdated;

        //Percentage of LP lost / Total LP
        public event EventHandler<int> LPHitReceived;

        //Percentage of HumHP lost
        public event EventHandler<int> HumiliationHPUpdated;

        //Percentage of HumHP lost
        public event EventHandler<int> HumiliationHPHitReceived;

        protected virtual void OnOrgasmStarted()
        {
            OrgasmStarted?.Invoke(this, null);
        }

        protected virtual void OnOrgasmEnded()
        {
            OrgasmEnded?.Invoke(this, null);
        }

        protected virtual void OnSubmissionStarted()
        {
            SubmissionStarted?.Invoke(this, null);
        }

        protected virtual void OnSubmissionEnded()
        {
            SubmissionEnded?.Invoke(this, null);
        }

        protected virtual void OnHPUpdated(int oldValue, int newValue, int maxValue)
        {
            if(oldValue == newValue || oldValue < newValue) { return; }
            var percentageHit = Math.Abs(oldValue - newValue) * 1m / maxValue * 100m;
            HPHitReceived?.Invoke(this, (int)percentageHit);

            var percentageHP = 1m * newValue / maxValue * 100m;
            HPUpdated?.Invoke(this, (int)percentageHP);
        }

        protected virtual void OnLPUpdated(int oldValue, int newValue, int maxValue)
        {
            if (oldValue == newValue || oldValue < newValue) { return; }
            var percentageHit = Math.Abs(oldValue - newValue) * 1m / maxValue * 100m;
            LPHitReceived?.Invoke(this, (int)percentageHit);

            var percentageLP = 1m * newValue / maxValue * 100;
            LPUpdated?.Invoke(this, (int)percentageLP);
        }

        protected virtual void OnHumHPUpdated(int oldValue, int newValue, int maxValue)
        {
            if (oldValue == newValue || oldValue < newValue) { return; }
            var percentageHit = Math.Abs(oldValue - newValue) * 1m / maxValue * 100m;
            HumiliationHPHitReceived?.Invoke(this, (int)percentageHit);

            var percentageLP = 1m * newValue / maxValue * 100m;
            HumiliationHPUpdated?.Invoke(this, (int)percentageLP);
        }
    }
}
