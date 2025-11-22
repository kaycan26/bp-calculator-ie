using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace BPCalculator
{
    // BP categories
    public enum BPCategory
    {
        [Display(Name = "Low Blood Pressure")] Low,
        [Display(Name = "Ideal Blood Pressure")] Ideal,
        [Display(Name = "Pre-High Blood Pressure")] PreHigh,
        [Display(Name = "High Blood Pressure")] High
    };

    public class BloodPressure
    {
        public const int SystolicMin = 70;
        public const int SystolicMax = 190;
        public const int DiastolicMin = 40;
        public const int DiastolicMax = 100;

        [Range(SystolicMin, SystolicMax, ErrorMessage = "Invalid Systolic Value")]
        public int Systolic { get; set; } // mmHG

        [Range(DiastolicMin, DiastolicMax, ErrorMessage = "Invalid Diastolic Value")]
        public int Diastolic { get; set; } // mmHG

        // Calculate BP category (existing behaviour kept exactly the same)
        public BPCategory Category
        {
            get
            {
                // High blood pressure
                if (Systolic >= 140 || Diastolic >= 90)
                {
                    return BPCategory.High;
                }

                // Pre-high blood pressure
                if ((Systolic >= 120 && Systolic <= 139) ||
                    (Diastolic >= 80 && Diastolic <= 89))
                {
                    return BPCategory.PreHigh;
                }

                // Ideal blood pressure
                if (Systolic >= 90 && Systolic <= 119 &&
                    Diastolic >= 60 && Diastolic <= 79)
                {
                    return BPCategory.Ideal;
                }

                // Otherwise, Low blood pressure
                return BPCategory.Low;
            }
        }

        /// <summary>
        /// Optional helper: returns the Display(Name=...) value for the current category.
        /// </summary>
        public string CategoryDisplayName
        {
            get
            {
                var field = typeof(BPCategory).GetField(Category.ToString());
                var attr = (DisplayAttribute?)Attribute.GetCustomAttribute(field, typeof(DisplayAttribute));
                return attr?.Name ?? Category.ToString();
            }
        }

        /// <summary>
        /// CSS class to apply in the UI for colour-coding the result.
        /// The actual colours are defined in site.css.
        /// </summary>
        public string CategoryCssClass => Category switch
        {
            BPCategory.Low => "bp-low",
            BPCategory.Ideal => "bp-ideal",
            BPCategory.PreHigh => "bp-prehigh",
            BPCategory.High => "bp-high",
            _ => "bp-unknown"
        };

        /// <summary>
        /// Human-readable advice text shown under the blood pressure result.
        /// This is UI-facing but lives here so that tests and views share the same text source.
        /// </summary>
        public string AdviceText
        {
            get
            {
                // We include the reading in the text to make it clearer for the user.
                var readingLine = $"Your reading: {Systolic} / {Diastolic} mmHg";

                return Category switch
                {
                    BPCategory.Low => $@"
{readingLine}

Your blood pressure is lower than the typical range. Some people have naturally low readings and feel well, but others may experience dizziness or fainting.

Practical tips:
• Stand up slowly after sitting or lying down
• Drink enough water during the day
• Avoid skipping meals

If you feel dizzy, faint, or unwell, please contact a doctor or healthcare professional.",

                    BPCategory.Ideal => $@"
{readingLine}

Your blood pressure is in the ideal range. Your current lifestyle is likely supporting your cardiovascular health.

To help keep it in this range:
• Stay physically active (e.g. brisk walking, cycling, swimming)
• Eat a balanced diet with fruit, vegetables and whole grains
• Limit very salty or highly processed foods
• Avoid smoking and keep alcohol intake low

Always discuss changes in exercise or diet with a healthcare professional.",

                    BPCategory.PreHigh => $@"
{readingLine}

Your blood pressure is slightly above the ideal range. This may increase the risk of developing high blood pressure in the future.

Lifestyle changes that may help:
• Reduce salt intake
• Increase daily movement (walking, stairs, active hobbies)
• Maintain a healthy weight
• Limit alcohol and avoid smoking

Discuss your readings with a doctor or nurse, especially if they are often in this range.",

                    BPCategory.High => $@"
{readingLine}

This reading is in the high blood pressure range. Persistent high blood pressure can increase the risk of heart disease and stroke.

Do not ignore this reading:
• Take another reading after resting quietly for 5–10 minutes
• Avoid caffeine and smoking before measuring

Please contact your doctor or healthcare professional to discuss your readings. Do not change any medication based on this calculator alone.",

                    _ => $@"
{readingLine}

Please enter values within the supported range. This tool is for information only and does not replace medical advice."
                };
            }
        }
    }
}
