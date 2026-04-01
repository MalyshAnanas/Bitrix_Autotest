using Demo.SeleniumFramework;
using Demo.TestEntities;

namespace Demo.PageObjects.Mobile;

public class MobileAppCollabSettingsPage
{
    public MobileAppCollabSettingsPage SetModerator(B24CollabEntity collab)
    {
        moderatorTab.Click();

        // лучше выбирать по имени, но пока выбираем первого пользователя, так как он всегда будет в списке
        firstUser.Click();
        selectBtn.Click();

        return this;
    }

    public MobileAppCollabSettingsPage SetNoGuests()
    {
        guestsTab.Click();
        noGuestsOption.Click();

        return this;
    }

    public MobileAppMessengerListPage Submit()
    {
        backBtn.Click();
        createBtn.Click();

        return new MobileAppMessengerListPage();
    }

    #region Elements
    private MobileElement moderatorTab => new MobileElement(
        "//android.view.ViewGroup[@content-desc=\"collab-create-security-area-permissions-list\"]" +
        "/android.view.ViewGroup/android.view.ViewGroup[2]",
        "Настройки Модератора");

    private MobileElement firstUser => new MobileElement(
        ["//android.widget.LinearLayout[@content-desc=\"section_{Recent chats}\"])[1]",
            "(//android.widget.LinearLayout[@content-desc=\"section_{Недавний поиск}\"])[1]"],
        "Первый пользовательв списке");

    private MobileElement selectBtn => new MobileElement(
        "//android.widget.FrameLayout[@resource-id=\"com.bitrix24.android:id/apply_button\"]",
        "Кнопка Выбрать");

    private MobileElement guestsTab => new MobileElement(
        "//android.view.ViewGroup[@content-desc=\"collab-create-security-area-permissions-list\"]" +
        "/android.view.ViewGroup/android.view.ViewGroup[5]",
        "Настройки приглашения гостей");

    private MobileElement noGuestsOption => new MobileElement(
        "//android.view.ViewGroup[@content-desc=\"popover_menu_N\"]",
        "Нет");

    private MobileElement backBtn => new MobileElement(
        "(//android.widget.FrameLayout[@resource-id=\"com.bitrix24.android:id/dynamicLeftButtonsContainer\"])[3]",
        "Назад");

    private MobileElement createBtn => new MobileElement(
        "//android.view.ViewGroup[@content-desc=\"undefined-edit-main-screen-box\"]",
        "Кнопка Создать");
    #endregion
}