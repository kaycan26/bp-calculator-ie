namespace BPCalculator
{
    public static class BMICalculator
    {
        // New feature: BMI calculation + category
        public static (double bmi, string category) Calculate(double heightCm, double weightKg)
        {
            var heightM = heightCm / 100.0;
            var bmi = System.Math.Round(weightKg / (heightM * heightM), 1);

            string category =
                bmi < 18.5 ? "Underweight" :
                bmi < 25.0 ? "Normal" :
                bmi < 30.0 ? "Overweight" :
                             "Obese";

            return (bmi, category);
        }
    }
}

