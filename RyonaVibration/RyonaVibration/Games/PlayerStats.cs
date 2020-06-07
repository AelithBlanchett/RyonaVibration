﻿using System;
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
        public event EventHandler<double> HPUpdated;

        //Percentage of HP lost / Total HP
        public event EventHandler<double> HPHitReceived;

        //Percentage of LP lost
        public event EventHandler<double> LPUpdated;

        //Percentage of LP lost / Total LP
        public event EventHandler<double> LPHitReceived;

        //Percentage of HumHP lost
        public event EventHandler<double> HumiliationHPUpdated;

        //Percentage of HumHP lost / Total HumHP
        public event EventHandler<double> HumiliationHPHitReceived;

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
            var percentageHit = Math.Abs(oldValue - newValue) * 1d / maxValue;
            HPHitReceived?.Invoke(this, percentageHit);

            var percentageHP = 1d * newValue / maxValue;
            HPUpdated?.Invoke(this, percentageHP);
        }

        protected virtual void OnLPUpdated(int oldValue, int newValue, int maxValue)
        {
            if (oldValue == newValue || oldValue < newValue) { return; }
            var percentageHit = Math.Abs(oldValue - newValue) * 1d / maxValue;
            LPHitReceived?.Invoke(this, percentageHit);

            var percentageLP = 1d * newValue / maxValue;
            LPUpdated?.Invoke(this, percentageLP);
        }

        protected virtual void OnHumHPUpdated(int oldValue, int newValue, int maxValue)
        {
            if (oldValue == newValue || oldValue < newValue) { return; }
            var percentageHit = Math.Abs(oldValue - newValue) * 1d / maxValue;
            HumiliationHPHitReceived?.Invoke(this, percentageHit);

            var percentageLP = 1d * newValue / maxValue;
            HumiliationHPUpdated?.Invoke(this, percentageLP);
        }
    }
}
