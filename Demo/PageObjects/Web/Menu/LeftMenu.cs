using Demo.PageObjects.Web.CRM;
using Demo.PageObjects.Web.Disk;
using OpenQA.Selenium;
using Demo.SeleniumFramework;
using Demo.PageObjects.Web.Feed;

namespace Demo.PageObjects.Web.Menu
{
    public class LeftMenu
    {
        #region Elements
        private WebItemWrap btnCRM =>
            new WebItemWrap("//li[@id=\"bx_left_menu_menu_crm_favorite\"]",
                "Пункт левого меню CRM");

        #endregion
        
        public IWebDriver Driver { get; }

        public LeftMenu(IWebDriver driver = default)
        {
            Driver = driver;
        }

        private void ClickMenuItem(WebItemWrap menuItem)
        {
            var menuItemsArea = new WebItemWrap("//div[@id='menu-items-block']", "Область с пунктами левого меню");
            if (menuItemsArea.Size(Driver).Width < 150)
            {
                var expandMenuButton = new WebItemWrap("//div[@class='menu-switcher']", "Кнопка сворачивания левого меню");
                expandMenuButton.Hover(Driver);
                var menuHeader = new WebItemWrap("//div[@class='menu-items-header-title']", "Кнопка сворачивания левого меню");
                menuHeader.Click(Driver);
            }

            if (menuItem.WaitDisplayed(driver: Driver) == false)
            {
                var teamWorkGroupEtc = new WebItemWrap("//li[@data-type='system_group' and not(@id='bx_left_menu_menu_marketplace_group')]", 
                    "Кнопка группы пунктов меню главного инструмента");
                if(teamWorkGroupEtc.WaitDisplayed(2, driver: Driver) && teamWorkGroupEtc.GetAttribute("data-collapse-mode") == "collapsed")
                    teamWorkGroupEtc.Click(Driver);

                if (menuItem.WaitDisplayed(driver: Driver) == false)
                {
                    //развернуть меню Ещё
                    var btnMore = new WebItemWrap("//span[@id='menu-more-btn-text']", "Кнопка Ещё левого меню");
                    btnMore.Click(Driver);
                }
            }
            //клик в пункт меню
            menuItem.Click(Driver);
        }

        public B24TasksListPage OpenTasks()
        {
            ClickMenuItem(new WebItemWrap("//li[@id='bx_left_menu_menu_tasks']", "Пункт левого меню 'Задачи'"));
            return new B24TasksListPage(Driver);
        }

        public B24SiteListPage OpenSites()
        {
            ClickMenuItem(new WebItemWrap("//li[@id='bx_left_menu_menu_sites']", "Пункт левого меню 'Сайты'"));
            return new B24SiteListPage(Driver);
        }

        public B24SettingsMainPage OpenSettings()
        {
            var btnSettings = new WebItemWrap("//li[@id='bx_left_menu_menu_configs_sect']", "Пункт левого меню настройки");
            ClickMenuItem(btnSettings);
            return new B24SettingsMainPage(Driver);
        }

        public FeedPage OpenNews()
        {
            //клик в пункт меню Новости
            var btnNews = new WebItemWrap("//li[@id='bx_left_menu_menu_live_feed']", "Пункт левого меню Новости");
            ClickMenuItem(btnNews);
            return new FeedPage(Driver);
        }

        public DiskPage OpenDisk()
        {
            var leftMenuTeamWork = new WebItemWrap("//li[@id='bx_left_menu_menu_teamwork']",
                "Блок Совместная работа в левом меню");
            string actualClass = leftMenuTeamWork.GetAttribute("class");
            string expectedClass = "menu-item-active"; // Появляется, когда свёрнут блок "Совместная работа"
            bool hasClass = actualClass
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Contains(expectedClass);

            var btnDisk = new WebItemWrap("//li[@id='bx_left_menu_menu_files']", "Кнопка Диск в левом меню");

            // Если не содержится, то блок открыт
            if (!hasClass)
            {
                btnDisk.Click(Driver);
            }
            else
            {
                leftMenuTeamWork.Click(Driver);
                btnDisk.Click(Driver);
            }
            
            return new DiskPage(Driver);
        }

        /// <summary>
        /// Открывает страницу CRM.
        /// </summary>
        public CRMPage OpenCRM()
        {
            btnCRM.Click(Driver);
            return new CRMPage(Driver);
        }
    }
}
