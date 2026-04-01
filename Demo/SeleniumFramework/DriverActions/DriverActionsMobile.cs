using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace Demo.SeleniumFramework.DriverActions
{
    public class DriverActionsMobile : DriverActionsBase
    {
        /// <summary>
        /// Creates a configured mobile driver object
        /// </summary>
        /// <returns></returns>
        public static AppiumDriver CreateNewMobileDriver()
        {
            var appiumOpts = new AppiumOptions();
            appiumOpts.PlatformName = "Android";
            appiumOpts.AutomationName = "UiAutomator2";//Driver for automation
            appiumOpts.App = "D:\\Proga\\Bitriks\\Mobile\\bitrix24_univer (1).apk";//Path to the application
            appiumOpts.AddAdditionalAppiumOption(MobileCapabilityType.Udid, "emulator-5554");//Unique device identifier
            appiumOpts.AddAdditionalAppiumOption(MobileCapabilityType.NoReset, true);//Do not reset application state
            appiumOpts.AddAdditionalAppiumOption(MobileCapabilityType.FullReset, false);//Perform full state reset
            appiumOpts.AddAdditionalAppiumOption(MobileCapabilityType.NewCommandTimeout, 60);//Timeout for new command
            appiumOpts.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AutoGrantPermissions, true);//Automatically confirm notifications
            appiumOpts.AddAdditionalAppiumOption("appium:appWaitActivity", "*");
            var appiumHost = "http://127.0.0.1:4723";
            AppiumDriver driver = new AndroidDriver(new Uri("http://127.0.0.1:4725"), appiumOpts);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return driver;
        }
    }
}
