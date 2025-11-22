namespace BPCalculator
{
    public static class BMICalculator
    {
        /// <summary>
        /// New feature: BMI calculation + category.
        /// This keeps the original signature so existing tests still compile.
        /// </summary>
        public static (double bmi, string category) Calculate(double heightCm, double weightKg)
        {
            if (heightCm <= 0 || weightKg <= 0)
            {
                // Defensive check – callers can handle "Invalid" if needed.
                return (0.0, "Invalid");
            }

            var heightM = heightCm / 100.0;
            var bmi = System.Math.Round(weightKg / (heightM * heightM), 1);

            string category =
                bmi < 18.5 ? "Underweight" :
                bmi < 25.0 ? "Normal" :
                bmi < 30.0 ? "Overweight" :
                             "Obese";

            return (bmi, category);
        }

        /// <summary>
        /// Returns a CSS class name for styling the BMI result panel.
        /// Define the colours in site.css.
        /// </summary>
        public static string CategoryCssClass(string category) =>
            category switch
            {
                "Underweight" => "bmi-underweight",
                "Normal" => "bmi-normal",
                "Overweight" => "bmi-overweight",
                "Obese" => "bmi-obese",
                _ => "bmi-unknown"
            };

        /// <summary>
        /// Returns advice text for a given BMI result.
        /// This is used by the Razor view to show contextual guidance.
        /// </summary>
        public static string AdviceText(double bmi, string category)
        {
            // For invalid input, do not pretend we have a meaningful BMI.
            if (category == "Invalid")
            {
                return "Please enter a positive height (cm) and weight (kg). "
                     + "This tool is for information only and does not replace medical advice.";
            }

            return category switch
            {
                "Underweight" =>
$@"Your BMI is {bmi} – Underweight.
This suggests you may weigh less than is typical for your height. In some cases this can be normal, but in others it may be linked to health or nutrition issues.

Consider discussing your weight, diet and any symptoms with a healthcare professional.",

                "Normal" =>
$@"Your BMI is {bmi} – Normal.
This is within the generally healthy range. Maintaining regular movement, a balanced diet and sufficient sleep can help keep your BMI and blood pressure in this range.",

                "Overweight" =>
$@"Your BMI is {bmi} – Overweight.
Your weight is above the typical range for your height. Gradual lifestyle changes may help over time, such as increasing activity and adjusting food portions.

Talking to a healthcare professional can give you personalised advice.",

                "Obese" =>
$@"Your BMI is {bmi} – Obese.
This level is associated with an increased risk of health problems such as high blood pressure and type 2 diabetes.

Please speak with a doctor or healthcare professional about safe and realistic options for weight management. Avoid extreme or crash diets without professional support.",

                _ =>
$@"Your BMI is {bmi}.
This tool is for information only and does not replace medical advice."
            };
        }
    }
}
