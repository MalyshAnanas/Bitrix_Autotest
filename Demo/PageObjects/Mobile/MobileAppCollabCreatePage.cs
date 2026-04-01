using Demo.SeleniumFramework;
using Demo.TestEntities;

namespace Demo.PageObjects.Mobile;

public class MobileAppCollabCreatePage
{
        public MobileAppCollabCreatePage FillCollabInfo(B24CollabEntity collab)
        {
            continueBtn.Click();

            nameInput.SendKeys(collab.Name);
            descriptionInput.SendKeys(collab.Description);

            return this;
        }

        public MobileAppCollabSettingsPage GoToSettings()
        {
            settingsBtn.Click();
            return new MobileAppCollabSettingsPage();
        }

        #region Elements
        private MobileElement continueBtn => new MobileElement(
            "//android.view.ViewGroup[@content-desc=\"CollabCreate_IntroScreen_Button\"]",
            "Кнопка Продолжить");

        private MobileElement nameInput => new MobileElement(
            "//android.view.ViewGroup[@content-desc=\"undefined-edit-name-input-\"]",
            "Поле Название");

        private MobileElement descriptionInput => new MobileElement(
            "//android.widget.EditText[@content-desc=\"undefined-edit-description-textarea-placeholder\"]",
            "Поле Описание");

        private MobileElement settingsBtn => new MobileElement(
            "//android.view.ViewGroup[@content-desc=\"undefined-edit-area-settings\"]",
            "Настройки чата");
        #endregion
}