using Demo.SeleniumFramework;
using OpenQA.Selenium;

namespace Demo.PageObjects.Web.CRM;

public class ChoiceRobotTypeMenu
{
    #region Elements
    private WebItemWrap repeatSalesTab => new WebItemWrap("//li[@class=\"ui-entity-catalog__menu_item\"][7]",
        "Вкладка Повторные продажи");
    private WebItemWrap scheduleCaseTab => new WebItemWrap("//span[normalize-space()='Запланировать дело']" +
                                                           " /ancestor::div[contains(@class,'ui-entity-catalog__option')]" +
                                                           " /div[contains(@class,'ui-entity-catalog__option-btn-block')]",
        "Кнопка Добавить в блоке Запланировать дело");
    #endregion
    
    public ChoiceRobotTypeMenu(IWebDriver driver = default)
    {
        Driver = driver;
    }

    public IWebDriver Driver { get; }

    /// <summary>
    /// Выбирает робота "Запланировать дело" во вкладке "Повторные продажи".
    /// </summary>
    public SettingsRobotForm ChooseScheduleCaseRobot()
    {
        repeatSalesTab.Click(Driver);
        scheduleCaseTab.Click(Driver);
        return new SettingsRobotForm(Driver);
    }
}