using OpenQA.Selenium;

namespace Demo.PageObjects.Web
{
    public class B24TasksListPage
    {
        public B24TasksListPage(IWebDriver driver = default)
        {
            Driver = driver;
        }

        public IWebDriver Driver { get; }
    }
}