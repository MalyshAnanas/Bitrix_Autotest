using OpenQA.Selenium;

namespace Demo.PageObjects.Web.Disk;

public class DiskPage
{
    public IWebDriver Driver { get; }

    public DiskPage(IWebDriver driver = default)
    {
        Driver = driver;
    }

    public TopMenuDisk GoToTopMenu()
    {
        return new TopMenuDisk(Driver);
    }
}