﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RyonaVibration.Games
{
    public class RRXXPlayerStats : PlayerStats
    {

        public const int MaxLethalBarPoints = 100;
        public const int MaxHumHPPercent = 20;
        public const int MaxPinningOrgasm = 30;
        public const int MaxSubmissionPercent = 85;

        private int lethalBarPoints = 0;
        private float submissionPercent = 0;
        private bool isPinned = false;
        private float humHPPercent = 0;

        public bool hasGottenRecentLethalChange = true;

        public int LethalBarPoints { get => lethalBarPoints;
            set
            {
                var tempValue = MaxLethalBarPoints - value;
                OnHPUpdated(lethalBarPoints, tempValue, MaxLethalBarPoints);
                if(lethalBarPoints != tempValue)
                {
                    hasGottenRecentLethalChange = true;
                }
                else
                {
                    hasGottenRecentLethalChange = false;
                }
                lethalBarPoints = tempValue;
            }
        }

        int countOfSameValuesForSubmission = 0;
        private bool isHavingHumiliationOrgasm;

        public float SubmissionPercent
        {
            get => submissionPercent;
            set
            {
                var originalValue = (int)(100 - ((submissionPercent * 100f / MaxSubmissionPercent) * 100f));
                var newValue = (int)(100 - ((value * 100f / MaxSubmissionPercent) * 100f));
                OnLPUpdated(originalValue, newValue, MaxSubmissionPercent);

                if(originalValue != newValue && IsPinned == false)
                {
                    IsPinned = true;
                }
                else if(originalValue == newValue && IsPinned == true)
                {
                    countOfSameValuesForSubmission++;
                    if(countOfSameValuesForSubmission > 100)
                    {
                        countOfSameValuesForSubmission = 0;
                        IsPinned = false;
                    }
                }

                submissionPercent = value;
            }
        }

        protected bool IsPinned
        {
            get => isPinned;
            set
            {
                //if (isPinned == true && value == false)
                //{
                //    OnSubmissionEnded();
                //}
                //else if (isPinned == false && value == true)
                //{
                //    OnSubmissionStarted();
                //}
                isPinned = value;
            }
        }

        public float HumHPPercent
        {
            get => humHPPercent;
            set
            {
                OnHumHPUpdated((int)humHPPercent, (int)value, MaxHumHPPercent);

                if(value >= MaxHumHPPercent && !IsHavingHumiliationOrgasm)
                {
                    IsHavingHumiliationOrgasm = true;
                }
                else if (value < MaxHumHPPercent && IsHavingHumiliationOrgasm)
                {
                    IsHavingHumiliationOrgasm = false;
                }

                humHPPercent = value;
            }
        }

        protected bool IsHavingHumiliationOrgasm
        {
            get => isHavingHumiliationOrgasm;
            set
            {
                if (isHavingHumiliationOrgasm == true && value == false)
                {
                    OnHumOrgasmEnded();
                }
                else if (isHavingHumiliationOrgasm == false && value == true)
                {
                    OnHumOrgasmStarted();
                }
                
                isHavingHumiliationOrgasm = value;
            }
        }

        //public byte PinningOrgasm
        //{
        //    get => pinningOrgasm;
        //    set
        //    {
        //        if(value == MaxPinningOrgasm)
        //        {
        //            OnOrgasmStarted();
        //        }
        //        else if (value < pinningOrgasm)
        //        {
        //            OnOrgasmEnded();
        //        }
        //        pinningOrgasm = value;
        //    }
        //}

        //public int PinningOrgasmPercentage
        //{
        //    get
        //    {
        //        return 100 * PinningOrgasm / 30;
        //    }
        //}
    }
}
