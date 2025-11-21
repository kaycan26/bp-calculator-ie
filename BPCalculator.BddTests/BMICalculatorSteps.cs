using BPCalculator;                 // to use BMICalculator
using FluentAssertions;
using TechTalk.SpecFlow;

namespace BPCalculator.BddTests.StepDefinitions
{
    [Binding]
    public class BMICalculatorSteps
    {
        private double _heightCm;
        private double _weightKg;
        private (double bmi, string category) _result;

        // Matches: "Given a height of <heightCm> cm"
        [Given(@"a height of (.*) cm")]
        public void GivenAHeightOfCm(double heightCm)
        {
            _heightCm = heightCm;
        }

        // Matches: "And a weight of <weightKg> kg"
        [Given(@"a weight of (.*) kg")]
        public void GivenAWeightOfKg(double weightKg)
        {
            _weightKg = weightKg;
        }

        // Matches: "When I calculate BMI"
        [When(@"I calculate BMI")]
        public void WhenICalculateBMI()
        {
            _result = BMICalculator.Calculate(_heightCm, _weightKg);
        }

        // Matches: Then the BMI category should be "<expectedCategory>"
        [Then(@"the BMI category should be ""(.*)""")]
        public void ThenTheBMICategoryShouldBe(string expectedCategory)
        {
            _result.category.Should().Be(expectedCategory);
            _result.bmi.Should().BeGreaterThan(0);
        }
    }
}
