using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

// page model

namespace BPCalculator.Pages
{
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

        // setup initial data
        public void OnGet()
        {
            BP = new BloodPressure() { Systolic = 100, Diastolic = 60 };
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
                var (bmi, category) = BMICalculator.Calculate(
                    HeightCm.Value,
                    WeightKg.Value);

                BMI = bmi;
                BMICategory = category;
            }
            else
            {
                BMI = null;
                BMICategory = null;
            }

            return Page();
        }
    }
}
