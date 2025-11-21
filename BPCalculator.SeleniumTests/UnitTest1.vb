Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome

' Alias to avoid Assert ambiguity if xUnit is ever referenced
Imports MsAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert

Namespace BPCalculator.SeleniumTests

    <TestClass>
    Public Class BloodPressureSeleniumTests

        Private _driver As IWebDriver

        <TestInitialize>
        Public Sub SetUp()
            Dim options As New ChromeOptions()
            options.AddArgument("--headless=new")       ' run headless for CI
            options.AddArgument("--no-sandbox")
            options.AddArgument("--disable-dev-shm-usage")

            _driver = New ChromeDriver(options)

            ' Simple implicit wait to give the page time to render elements
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5)
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

            ' Read the BP result with a small retry loop to avoid stale-element issues
            Dim text As String = GetResultTextWithRetry()

            MsAssert.AreEqual("Ideal", text, $"Expected BP category 'Ideal' but got '{text}'")
        End Sub

        ' Helper to handle StaleElementReferenceException if the result div is re-rendered
        Private Function GetResultTextWithRetry() As String
            Dim attempts As Integer = 0

            While True
                Try
                    Dim bpResultDiv = _driver.FindElement(By.Id("bpResult"))
                    Return bpResultDiv.Text
                Catch ex As StaleElementReferenceException
                    attempts += 1
                    If attempts >= 3 Then
                        Throw
                    End If

                    ' wait a tiny bit before trying again
                    Threading.Thread.Sleep(500)
                End Try
            End While
        End Function

    End Class

End Namespace
