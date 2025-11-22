using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BPCalculator.PlaywrightTests
{
    [TestClass]
    public class BmiPagePlaywrightTests
    {
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private IBrowserContext? _context;
        private IPage? _page;

        // Base URL: APP_URL for pipeline, localhost for dev
        private readonly string _baseUrl =
            Environment.GetEnvironmentVariable("APP_URL")
            ?? "http://localhost:5000";

        [TestInitialize]
        public async Task SetUp()
        {
            _playwright = await Playwright.CreateAsync();

            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
        }

        [TestCleanup]
        public async Task TearDown()
        {
            if (_page is not null)
                await _page.CloseAsync();

            if (_context is not null)
                await _context.CloseAsync();

            if (_browser is not null)
                await _browser.CloseAsync();

            _playwright?.Dispose();
        }

        [TestMethod]
        public async Task BmiResult_IsCalculatedAndVisible()
        {
            Assert.IsNotNull(_page, "Playwright page was not initialised.");

            // Go to the home page of the app
            await _page!.GotoAsync(_baseUrl);

            // Fill BMI inputs (height + weight)
            await _page.FillAsync("#HeightCm", "170");
            await _page.FillAsync("#WeightKg", "65");

            // Click Submit
            await _page.ClickAsync("input[type='submit']");

            // Wait for BMI result panel
            var bmiResult = _page.Locator("#bmiResult");
            await bmiResult.WaitForAsync();

            // Assert: panel is visible
            Assert.IsTrue(
                await bmiResult.IsVisibleAsync(),
                "BMI result panel is not visible."
            );

            var text = await bmiResult.InnerTextAsync();

            // Assert: BMI result contains one of the known categories
            Assert.IsTrue(
                text.Contains("Normal", StringComparison.OrdinalIgnoreCase) ||
                text.Contains("Underweight", StringComparison.OrdinalIgnoreCase) ||
                text.Contains("Overweight", StringComparison.OrdinalIgnoreCase) ||
                text.Contains("Obese", StringComparison.OrdinalIgnoreCase),
                $"Unexpected BMI result text: '{text}'"
            );
        }
    }
}
