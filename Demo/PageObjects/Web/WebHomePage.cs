using OpenQA.Selenium;
using Demo.PageObjects.Web.Feed;
using Demo.PageObjects.Web.Menu;

namespace Demo.PageObjects.Web
{
    public class WebHomePage
    {
        public IWebDriver Driver { get; }

        public WebHomePage(IWebDriver driver = default)
        {
            Driver = driver;
        }

        /// <summary>
        /// Боковое меню.
        /// </summary>
        public LeftMenu SideMenu => new LeftMenu(Driver);

        public FeedPage FeedPage => new FeedPage(Driver);
        
        public TopMenu TopMenu => new TopMenu(Driver);
    }
}
