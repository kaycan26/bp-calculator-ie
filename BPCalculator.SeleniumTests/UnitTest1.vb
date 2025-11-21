Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome

' Alias to avoid Assert ambiguity if xUnit is referenced anywhere
Imports MsAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert

Namespace BPCalculator.SeleniumTests

    <TestClass>
    Public Class BloodPressureSeleniumTests

        Private _driver As IWebDriver

        <TestInitialize>
        Public Sub SetUp()
            Dim options As New ChromeOptions()
            options.AddArgument("--headless=new")   ' run headless for CI
            options.AddArgument("--no-sandbox")
            options.AddArgument("--disable-dev-shm-usage")

            _driver = New ChromeDriver(options)
        End Sub

        <TestCleanup>
        Public Sub TearDown()
            If _driver IsNot Nothing Then
                _driver.Quit()
            End If
        End Sub

        <TestMethod>
        Public Sub BpCalculator_Displays_Ideal_For_100_60()
            ' NOTE: Make sure the BPCalculator web app is running at http://localhost:5000
            _driver.Navigate().GoToUrl("http://localhost:5000/")

            ' Fill in BP values
            Dim systolic = _driver.FindElement(By.Name("BP.Systolic"))
            systolic.Clear()
            systolic.SendKeys("100")

            Dim diastolic = _driver.FindElement(By.Name("BP.Diastolic"))
            diastolic.Clear()
            diastolic.SendKeys("60")

            ' Submit the form
            Dim submit = _driver.FindElement(By.CssSelector("input[type='submit']"))
            submit.Click()

            ' Simple wait for the result to render
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2)

            ' Read the BP result
            Dim bpResultDiv = _driver.FindElement(By.Id("bpResult"))
            Dim text = bpResultDiv.Text

            MsAssert.AreEqual("Ideal", text, $"Expected BP category 'Ideal' but got '{text}'")
        End Sub

    End Class

End Namespace
