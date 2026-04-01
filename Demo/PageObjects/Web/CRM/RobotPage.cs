using Demo.BaseFramework;
using Demo.PageObjects.Web.CRM;
using Demo.SeleniumFramework;
using Demo.SeleniumFramework.DriverActions;
using OpenQA.Selenium;

public class RobotPage
{
    #region Elements
    private WebItemWrap iframeRobot => new WebItemWrap("//iframe[contains(@src,'/crm/deal/automation/')]",
        "iframe страницы Роботы");
    private WebItemWrap btnCreateNewRobot => new WebItemWrap("//button[contains(@class,'ui-btn-success') and contains(.,'Создать')]",
            "Кнопка Создать робота");
    private WebItemWrap btnSaveRobot => new WebItemWrap("//button[@id=\"ui-button-panel-save\"]",
        "Кнопка Сохранить изменения всех роботов");
    private WebItemWrap btnCloseRobotPage => new WebItemWrap("//div[@class=\"side-panel-label-icon side-panel-label-icon-close\"]",
        "Кнопка для закрытия страницы");
    private WebItemWrap robotMark => new WebItemWrap("//div[contains(@class,'bizproc-automation-robot-information') and contains(@class,'--complete')]",
        "Галочка у робота");
    
    private WebItemWrap robotContainer => new WebItemWrap("//div[@class=\"bizproc-automation-robot-container ui-tour-selector\"]",
        "Контейнер нашего робота");
    #endregion

    public RobotPage(IWebDriver driver = default)
    {
        Driver = driver;
    }

    public IWebDriver Driver { get; }
        
    /// <summary>
    /// Открывает форму создания нового робота.
    /// </summary>
    public ChoiceRobotTypeMenu OpenCreateRobotForm()
    {
        iframeRobot.SwitchToFrame(Driver);
        
        WaitersCore.WaitForConditionReached(
            () => btnCreateNewRobot.WaitDisplayed(),
            1, 3,
            $"Ожидание кнопки Создать");
        btnCreateNewRobot.Click(Driver);
        return new ChoiceRobotTypeMenu(Driver);
    }
    
    
    /// <summary>
    /// Сохраняет робота и закрывает страницу роботов.
    /// </summary>
    public CRMPage SaveRobot()
    {
        btnSaveRobot.Click(Driver);
        DriverActionsWeb.SwitchToDefaultContent(Driver);
        btnCloseRobotPage.Click(Driver);
        return new CRMPage(Driver);
    }
    
    /// <summary>
    /// Проверяет, что робот создан.
    /// </summary>
    public bool IsRobotCreate()
    {
        DriverActionsWeb.SwitchToDefaultContent(Driver);
        iframeRobot.SwitchToFrame(Driver);
        return WaitersCore.WaitForConditionReached(
            () => robotContainer.WaitDisplayed(),
            2, 6,
            $"Ожидание контейнера робота");
    }

    /// <summary>
    /// Проверяет, что робот отмечен как выполненный (отображается галочка).
    /// </summary>
    public bool IsRobotMarkComplete()
    {
        DriverActionsWeb.SwitchToDefaultContent(Driver);
        iframeRobot.SwitchToFrame(Driver);
        return WaitersCore.WaitForConditionReached(
            () => robotMark.WaitDisplayed(),
            2, 6,
            $"Ожидание галочки у робота Запланировать дело");
    }
}