using Demo.SeleniumFramework;
using OpenQA.Selenium;

namespace Demo.PageObjects.Web
{
    public class B24SettingsMainPage
    {
        public B24SettingsMainPage(IWebDriver driver = default)
        {
            Driver = driver;
        }

        public IWebDriver Driver { get; }

        public B24SettingsMainPage DisableSendToAllDefault()
        {
            ChangeSendToAllState(false);
            return new B24SettingsMainPage(Driver);
        }

        public B24SettingsMainPage EnableSendToAllDefault()
        {
            ChangeSendToAllState(true);
            return new B24SettingsMainPage(Driver);
        }

        B24SettingsMainPage ChangeSendToAllState(bool mustBeSelected)
        {
            //снять галочку
            var cbSendToAllByDefault = new WebItemWrap("//input[@id=" +
                "'default_livefeed_toall']", "Чекбокс Адресация всем по умолчанию");
            bool @checked = cbSendToAllByDefault.Checked(Driver);
            if(@checked != mustBeSelected)
                cbSendToAllByDefault.Click(Driver);
            return new B24SettingsMainPage(Driver);
        }

        public B24SettingsMainPage Save()
        {
            //клик в кнопку сохранить
            var saveBtn = new WebItemWrap("//span[contains(text(), " +
                "'Сохранить настройки')]", "Кнопка Сохранить");
            saveBtn.Click(Driver);
            return new B24SettingsMainPage(Driver);
        }
    }
}
