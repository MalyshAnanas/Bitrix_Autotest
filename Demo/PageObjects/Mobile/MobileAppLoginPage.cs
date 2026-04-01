using Demo.SeleniumFramework;
using Demo.TestEntities;

namespace Demo.PageObjects.Mobile
{
    public class MobileAppLoginPage : LoginPageBase
    {
        public MobileAppLoginPage(PortalData portal) : base(portal)
        {
        }

        public MobileAppHomePage Login(User admin)
        {
            var enterAddresBtn = new MobileElement("//android.widget.TextView" +
                "[@content-desc='authFormEnterAddressButton']",
                "Кнопка 'введите адрес'");
            var portalAddresField = new MobileElement("//android.widget.EditText" +
                "[@content-desc='signInPortalInput']",
                "Поле для ввода адреса портала");
            var loginField = new MobileElement("//android.widget.EditText" +
                "[@content-desc='signInPortalFormPhoneInput']",
                "Поле для ввода логина");
            var pwdField = new MobileElement("//android.widget.EditText" +
                "[@content-desc='passwordFormInput']",
                "Поле для ввода пароля");
            var nextBtn = new MobileElement("//android.widget.Button" +
                "[@resource-id='com.bitrix24.android:id/btnNext']",
                "Кнопка 'Далее'");

            //переход к форме ввода адреса портала
            enterAddresBtn.Click();

            //вводим адрес портала и дальше
            portalAddresField.SendKeys(portalData.Adress.ToString());
            nextBtn.Click();

            //вводим логин и дальше
            loginField.SendKeys(admin.Login);
            nextBtn.Click();

            //вводим пароль и дальше
            pwdField.SendKeys(admin.Password, logInputtedText: false);
            nextBtn.Click();

            return new MobileAppHomePage();
        }
    }
}
