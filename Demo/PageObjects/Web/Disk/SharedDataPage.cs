using Demo.SeleniumFramework;
using OpenQA.Selenium;

namespace Demo.PageObjects.Web.Disk;

public class SharedDataPage
{
    public IWebDriver Driver { get; }

    public SharedDataPage(IWebDriver driver = default)
    {
        Driver = driver;
    }

    private WebItemWrap btnRecycleBin = new WebItemWrap(
        "//a[@class='ui-btn js-disk-trashcan-button ui-btn-light-border ui-btn-themes']",
        "Кнопка 'Корзина' на странице общего диска");

    public RecycleBinPage OpenRecycleBin()
    {
        btnRecycleBin.Click(Driver);
        return new RecycleBinPage(Driver);
    }
}