Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Support.UI

<TestClass>
Public Class BloodPressureSeleniumTests

    Private driver As IWebDriver
    Private wait As WebDriverWait
    Private baseUrl As String = "http://localhost:5000"

    <TestInitialize>
    Public Sub SetUp()
        Dim options As New ChromeOptions()
        options.AddArgument("--headless=new")
        options.AddArgument("--no-sandbox")
        options.AddArgument("--disable-gpu")

        driver = New ChromeDriver(options)
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5)
        wait = New WebDriverWait(driver, TimeSpan.FromSeconds(10))
    End Sub

    <TestCleanup>
    Public Sub TearDown()
        If driver IsNot Nothing Then
            driver.Quit()
        End If
    End Sub

    <TestMethod>
    Public Sub BpCalculator_Displays_Ideal_For_100_60()

        driver.Navigate().GoToUrl(baseUrl)

        ' Fill systolic
        Dim systolic = driver.FindElement(By.Id("BP_Systolic"))
        systolic.Clear()
        systolic.SendKeys("100")

        ' Fill diastolic
        Dim diastolic = driver.FindElement(By.Id("BP_Diastolic"))
        diastolic.Clear()
        diastolic.SendKeys("60")

        ' Submit
        Dim submitButton = driver.FindElement(By.CssSelector("input[type='submit']"))
        submitButton.Click()

        ' Wait for the BP result panel
        Dim bpResultDiv = wait.Until(Function(d)
                                         Try
                                             Dim el = d.FindElement(By.Id("bpResult"))
                                             Return If(el.Displayed, el, Nothing)
                                         Catch
                                             Return Nothing
                                         End Try
                                     End Function)

        Assert.IsNotNull(bpResultDiv, "BP result div not found.")

        Dim resultText As String = bpResultDiv.Text

        ' Expected: "Ideal"
        Assert.IsTrue(resultText.Contains("Ideal"),
                      $"Expected BP category 'Ideal' but got '{resultText}'")

    End Sub

End Class
