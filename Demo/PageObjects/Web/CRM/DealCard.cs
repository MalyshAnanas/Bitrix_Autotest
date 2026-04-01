using Demo.BaseFramework;
using Demo.SeleniumFramework;
using OpenQA.Selenium;

namespace Demo.PageObjects.Web.CRM;

public class DealCard
{
    #region Elements

    private WebItemWrap robotTab => new WebItemWrap(
        "//span[@class=\"main-buttons-item-text-box\" and text()=\"Роботы\"]",
        "Вкладка Роботы");

    private WebItemWrap ifarmeDeal => new WebItemWrap("//iframe[@class=\"side-panel-iframe\"]",
        "iframe для карточки сделки");
    WebItemWrap robotTitle(string robotName) => new WebItemWrap(
        $"//span[@class=\"crm-timeline__card-title\" and text()=\"{robotName}\"]",
        $"Названия дела, которое создал робот");
    #endregion
    
    public DealCard(IWebDriver driver = default)
    {
        Driver = driver;
    }

    public IWebDriver Driver { get; }

    /// <summary>
    /// Проверяет, что робот создал запланированное дело в карточке сделки.
    /// </summary>
    public bool IsRobotDone(string robotName)
    {
        ifarmeDeal.SwitchToFrame(Driver);

        return WaitersCore.WaitForConditionReached(
            () => robotTitle(robotName).WaitDisplayed(),
            2, 6,
            $"Ожидание дела, которое создал робот");
    }

    /// <summary>
    /// Переходит во вкладку "Роботы" из карточки сделки.
    /// </summary>
    public RobotPage OpenRobotPageFromDealCard()
    {
        robotTab.Click(Driver);
        return new RobotPage(Driver);
    }
}