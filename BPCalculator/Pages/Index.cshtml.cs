using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BPCalculator.Pages
{
    // page model
    public class BloodPressureModel : PageModel
    {
        [BindProperty] // bound on POST
        public BloodPressure BP { get; set; }

        // ---- BMI NEW FEATURE PROPERTIES ----
        [BindProperty]
        public double? HeightCm { get; set; }

        [BindProperty]
        public double? WeightKg { get; set; }

        public double? BMI { get; private set; }
        public string BMICategory { get; private set; }

        // NEW: for coloured BMI panel + advice text
        public string BMICssClass { get; private set; }
        public string BMIAdvice { get; private set; }

        // setup initial data
        public void OnGet()
        {
            BP = new BloodPressure() { Systolic = 100, Diastolic = 60 };

            BMI = null;
            BMICategory = null;
            BMICssClass = null;
            BMIAdvice = null;
        }

        // POST, validate
        public IActionResult OnPost()
        {
            // extra validation for blood pressure
            if (!(BP.Systolic > BP.Diastolic))
            {
                ModelState.AddModelError("", "Systolic must be greater than Diastolic");
            }

            // ---- BMI new feature logic ----
            if (HeightCm.HasValue && HeightCm > 0 &&
                WeightKg.HasValue && WeightKg > 0)
            {
                // use the updated BMICalculator helpers
                var (bmi, category) = BMICalculator.Calculate(
                    HeightCm.Value,
                    WeightKg.Value);

                BMI = bmi;
                BMICategory = category;
                BMICssClass = BMICalculator.CategoryCssClass(category);
                BMIAdvice = BMICalculator.AdviceText(bmi, category);
            }
            else
            {
                // no valid BMI
                BMI = null;
                BMICategory = null;
                BMICssClass = null;
                BMIAdvice = null;
            }

            return Page();
        }
    }
}
