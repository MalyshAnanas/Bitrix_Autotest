
using Demo.SeleniumFramework;

namespace Demo.PageObjects.Mobile
{
    /// <summary>
    /// Главная панель приложения
    /// </summary>
    public class MobileAppMainPanel
    {
        public MobileAppTasksListPage SelectTasks()
        {
            var tasksTab = new MobileElement("//android.widget.TextView" +
                "[@resource-id=\"com.bitrix24.android:id/bb_bottom_bar_title\" and @text=\"Tasks\"]",
                "Таб 'Задачи'");
            tasksTab.Click();

            return new MobileAppTasksListPage();
        }

        public MobileAppMessengerListPage SelectMessenger()
        {
            chatsTab.Click();
            return new MobileAppMessengerListPage();
        }

        #region Elements
        MobileElement chatsTab => new MobileElement("//android.widget.FrameLayout"+
                                                    "[contains(\"@content-desc,\"bottombar_tab_chats\"]",
            "Таб 'Мессенджер'");
        #endregion
    }
}