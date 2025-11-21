using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// Aliases to avoid any Assert name clashes
using MsAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using MsStringAssert = Microsoft.VisualStudio.TestTools.UnitTesting.StringAssert;

namespace BPCalculator.PlaywrightTests
{
    [TestClass]
    public class BmiPagePlaywrightTests
    {
        private IPlaywright _playwright = null!;
        private IBrowser _browser = null!;
        private IPage _page = null!;

        [TestInitialize]
        public async Task SetUp()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });
            _page = await _browser.NewPageAsync();
        }

        [TestCleanup]
        public async Task TearDown()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }

        [TestMethod]
        public async Task BmiResult_IsCalculatedAndVisible()
        {
            // Arrange
            await _page.GotoAsync("http://localhost:5000/");

            // Fill BP (values don’t matter for BMI)
            await _page.FillAsync("input[name='BP.Systolic']", "120");
            await _page.FillAsync("input[name='BP.Diastolic']", "80");

            // Act – fill BMI fields
            await _page.FillAsync("input[name='HeightCm']", "170");
            await _page.FillAsync("input[name='WeightKg']", "65");

            // Click submit
            await _page.ClickAsync("input[type='submit']");

            // Assert – BMI result box should be visible
            var bmiResult = _page.Locator("#bmiResult");
            await Assertions.Expect(bmiResult).ToBeVisibleAsync();

            var text = await bmiResult.InnerTextAsync();

            // text should not be empty
            MsAssert.IsFalse(string.IsNullOrWhiteSpace(text), $"BMI result text was: '{text}'");

            // and should include a category in parentheses, e.g. "(Normal)"
            MsStringAssert.Contains(text, "(", $"BMI result text was: '{text}'");


        }
    }
}
