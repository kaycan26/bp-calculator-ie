Feature: BMI Calculator
  In order to know my BMI category
  As a user
  I want the system to calculate BMI correctly

  Scenario Outline: BMI category scenarios
    Given a height of <heightCm> cm
    And a weight of <weightKg> kg
    When I calculate BMI
    Then the BMI category should be "<expectedCategory>"

    Examples:
      | heightCm | weightKg | expectedCategory |
      | 170      | 50       | Underweight      |
      | 170      | 65       | Normal           |
      | 170      | 80       | Overweight       |
      | 170      | 100      | Obese            |

  Scenario: BMI borderline between Normal and Overweight
    Given a height of 170 cm
    And a weight of 72 kg
    When I calculate BMI
    Then the BMI category should be "Normal"
