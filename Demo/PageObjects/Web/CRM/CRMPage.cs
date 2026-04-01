using Demo.BaseFramework;
using Demo.SeleniumFramework;
using OpenQA.Selenium;

namespace Demo.PageObjects.Web.CRM;

public class CRMPage
{
    #region Elements
    private WebItemWrap btnRobot => new WebItemWrap("//a[@class=\"ui-btn ui-btn-light-border ui-btn-no-caps" +
                                                    " ui-btn-themes ui-btn-round crm-robot-btn\"]",
        "Кнопка Роботы");
    private WebItemWrap btnNewQuickDeal => new WebItemWrap("//div[@class=\"crm-kanban-column-add-item-button\"" +
                                                           " and text()=\"Быстрая сделка\"]",
        "Кнопка Быстрая сделка");
    private WebItemWrap quickDealName => new WebItemWrap("//input[@id=\"title_text\"]",
        "Поле для названия сделки");
    private WebItemWrap btnSaveQuickDeal => new WebItemWrap("//input[@class=\"ui-btn ui-btn-xs ui-btn-primary\"]",
        "Кнопка Сохранить для сделки");
    private WebItemWrap btnCloseUnwantedPopUp => new WebItemWrap("//span[@class=\"popup-window-close-icon\"]",
        "Кнопка для закрытия появляющегося поп апа");
    WebItemWrap btnOpenDealCard(string name) => new WebItemWrap($"//a[contains(@class,'crm-kanban-item-title') and contains(.,'{name}')]",
        "Кнопка для открытия карты сделки");
    #endregion
    
    public CRMPage(IWebDriver driver = default)
    {
        Driver = driver;
    }

    public IWebDriver Driver { get; }

    /// <summary>
    /// Открывает страницу "Роботы" в CRM.
    /// </summary>
    public RobotPage OpenRobotPage()
    {
        btnRobot.Click(Driver);
        return new RobotPage(Driver);
    }

    /// <summary>
    /// Создаёт новую быструю сделку с указанным названием.
    /// </summary>
    public CRMPage CreateNewQuickDeal(string name)
    {
        btnNewQuickDeal.Click(Driver);
        quickDealName.Click(Driver);
        quickDealName.SendKeys(name);
        btnSaveQuickDeal.Click(Driver);
        return new CRMPage(Driver);
    }

    /// <summary>
    /// Открывает карточку сделки по её названию.
    /// </summary>
    public DealCard OpenDealCard(string dealname)
    {
        btnOpenDealCard(dealname).Click(Driver);
        return new DealCard(Driver);
    }

    /// <summary>
    /// Закрывает всплывающее окно, если оно появилось и мешает работе.
    /// </summary>
    public CRMPage CloseUnwantedPopup()
    {
        bool isPopUpExist = WaitersCore.WaitForConditionReached(
            () => btnCloseUnwantedPopUp.WaitDisplayed(),
            2, 4,
            $"Ожидание появления Pop Up");
        
        //Если поп ап есть, то закрываем его, ибо она мешает создать новую сделку
        if (isPopUpExist)
        {
            btnCloseUnwantedPopUp.Click(Driver);
        }
        return this;
    }
}