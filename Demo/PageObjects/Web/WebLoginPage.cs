using Demo.SeleniumFramework;
using Demo.SeleniumFramework.DriverActions;
using Demo.TestEntities;
using OpenQA.Selenium;

namespace Demo.PageObjects.Web
{
    class WebLoginPage : LoginPageBase
    {
        IWebDriver Driver { get; }

        public WebLoginPage(PortalData portal, IWebDriver driver = default) : base(portal)
        {
            Driver = driver;
        }

        public WebHomePage Login(User admin)
        {
            DriverActionsWeb.OpenUri(portalData.Adress, Driver);
            var loginField = new WebItemWrap("//input[@id='login' or @name='USER_LOGIN']",
                "Поле для ввода логина");
            var pwdField = new WebItemWrap("//input[@id='password' or @name='USER_PASSWORD']",
                "Поле для ввода пароля");
            loginField.SendKeys(admin.Login, Driver);
            if (!pwdField.WaitDisplayed(1, Driver))
                loginField.SendKeys(Keys.Enter, Driver);
            pwdField.SendKeys(admin.Password, Driver, logInputtedText: false);
            pwdField.SendKeys(Keys.Enter, Driver);
            return new WebHomePage(Driver);
        }
    }
}
