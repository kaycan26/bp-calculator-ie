using Xunit;

namespace BPCalculator.UnitTests
{
    public class BMICalculatorTests
    {
        // Height is constant so the weight decides category
        [Theory(DisplayName = "BMI: category scenarios")]
        [InlineData(170, 50, "Underweight")]
        [InlineData(170, 65, "Normal")]
        [InlineData(170, 80, "Overweight")]
        [InlineData(170, 100, "Obese")]
        public void Calculate_ReturnsExpectedCategory(
            double heightCm,
            double weightKg,
            string expectedCategory)
        {
            // Act
            var (bmi, category) = BMICalculator.Calculate(heightCm, weightKg);

            // Assert
            Assert.True(bmi > 0);
            Assert.Equal(expectedCategory, category);
        }

        [Fact]
        public void Calculate_RoundsToOneDecimalPlace()
        {
            // Act
            var (bmi, _) = BMICalculator.Calculate(180, 80); // ~24.69

            // Assert – should be rounded to 1 decimal (24.7)
            Assert.Equal(24.7, bmi);
        }

        [Fact]
        public void Calculate_HeavierWeightProducesHigherBMI()
        {
            // Arrange
            double height = 170;

            // Act
            var (bmiLighter, _) = BMICalculator.Calculate(height, 65);
            var (bmiHeavier, _) = BMICalculator.Calculate(height, 80);

            // Assert
            Assert.True(bmiHeavier > bmiLighter);
        }

    }
}
