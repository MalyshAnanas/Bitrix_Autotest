using OpenQA.Selenium;

namespace Demo.PageObjects.Web.Menu
{
    public class TopMenu
    {
        public IWebDriver Driver { get; }
        
        public TopMenu(IWebDriver driver = default)
        {
            Driver = driver;
        }

        public void ExitButton()
        {
            throw new NotImplementedException();
        }
    }
}

    