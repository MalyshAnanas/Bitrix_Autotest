using Demo.SeleniumFramework;
using OpenQA.Selenium;

namespace Demo.PageObjects.Web.CRM;

public class SettingsRobotForm
{
    #region Elements
    private WebItemWrap nameRobot => new WebItemWrap("//input[@class=\"bizproc-type-control bizproc-type-control-string\"]",
        "Поле Название");
    private WebItemWrap btnSaveRobotSettings => new WebItemWrap("//div[@class=\"popup-window-buttons\"]" +
                                                                "//button[@class=\"ui-btn ui-btn-success\"]",
        "Кнопка Сохранить в форме настройки робота");
    #endregion
    
    public SettingsRobotForm(IWebDriver driver = default)
    {
        Driver = driver;
    }

    public IWebDriver Driver { get; }

    /// <summary>
    /// Заполняет поле названия робота.
    /// </summary>
    public SettingsRobotForm FillRobotInfo(string robotName)
    {
        nameRobot.Click(Driver);
        // Очищаем поле
        nameRobot.SendKeys(Keys.Control + "a", Driver);
        nameRobot.SendKeys(Keys.Delete, Driver);
        
        // Вставляем наше название
        nameRobot.SendKeys(robotName, Driver);
        return new SettingsRobotForm(Driver);
    }
    
    /// <summary>
    /// Сохраняет настройки робота.
    /// </summary>
    public RobotPage SaveRobotInfo()
    {
        btnSaveRobotSettings.Click(Driver);
        return new RobotPage(Driver);
    }
}