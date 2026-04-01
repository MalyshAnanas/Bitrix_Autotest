using Demo.SeleniumFramework;
using OpenQA.Selenium;

namespace Demo.PageObjects.Web.Disk;

public class TopMenuDisk
{
    public IWebDriver Driver { get; }

    public TopMenuDisk(IWebDriver driver = default)
    {
        Driver = driver;
    }
    
    WebItemWrap btnSharedData = new WebItemWrap(
        "//div[@id='top_menu_id_docs_414529032']//a[@class='main-buttons-item-link']",
        "Кнопка 'Общий диск' в верхнем меню");

    public SharedDataPage OpenSharedData()
    {
        btnSharedData.Click(Driver);
        return new SharedDataPage(Driver);
    }
}