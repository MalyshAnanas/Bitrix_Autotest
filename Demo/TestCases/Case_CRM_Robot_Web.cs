using Demo.BaseFramework;
using Demo.BaseFramework.LogTools;
using Demo.PageObjects.Web;
using Demo.PageObjects.Web.CRM;

namespace Demo.TestCases;

public class Case_CRM_Robot_Web : TestCaseCollectionBuilder
{
    protected override List<ExecutableTestCase> GetCases()
    {
        var caseCollection = new List<ExecutableTestCase>();
        caseCollection.Add(new ExecutableTestCase("Запуск робота 'Запланировать дело' в CRM",
            homePage => CreateCRMRobot(homePage)));
        return caseCollection;
    }

    /// <summary>
    /// Полный сценарий проверки робота "Запланировать дело":
    /// создание робота, создание сделки и проверка его выполнения.
    /// </summary>
    void CreateCRMRobot(WebHomePage homePage)
    {
        string robotName = "Тестовый робот" + HelperMethodsCore.GetDateTimeSalt();
        string dealName = "Тестовая сделка" + HelperMethodsCore.GetDateTimeSalt();
        
        // Находимся на главной странице
        RobotPage robotPage = homePage
            // Переходим в левое меню
            .SideMenu
            // Заходим в CRM
            .OpenCRM()
            // Нажимаем на кнопку "Роботы"
            .OpenRobotPage()
            // "Создать"
            .OpenCreateRobotForm()
            // Вкладка "Повторные продажи"
            // "Запланировать дело"
            .ChooseScheduleCaseRobot()
            // Вводим название
            .FillRobotInfo(robotName)
            // Сохраняем робота
            .SaveRobotInfo();
            
        bool isRobotCreate = robotPage.IsRobotCreate();
            
            // Сохраняем робота и закрываем страницу
        CRMPage crmPage = robotName.SaveRobot();

        bool isRobotCreate = crmPage.IsRobotCreate(robotName);

        // Создаём новую сделку 
        DealCard dealPage = crmPage
                // Закрываем pop up, закрывающий кнопку "Быстрая сделка"
            .CloseUnwantedPopup()
                // Создаём новую сделку по кнопке "Быстрая сделка"
            .CreateNewQuickDeal(dealName)
            // Заходим в карточку сделки
            .OpenDealCard(dealName);
        // смотрим, чтоб дело, запланированным роботом, появилось в правой части карточки
        bool isRobotDone = dealPage.IsRobotDone(robotName);
            
        // Заходим во вкладку "Роботы
        bool isRobotDoneMark = dealPage.OpenRobotPageFromDealCard()
        // Смотрим, чтоб была голочка
        .IsRobotMarkComplete();
        
        // Проверка, что дело и галочка появились
        if (!isRobotDone || !isRobotDoneMark || isRobotCreate)
        {
            if (!isRobotDone && !isRobotDoneMark && isRobotCreate)
            {
                Log.Error($"Робот '{robotName}' не выполнился: дело не создано и отсутствует отметка выполнения.");
            }
            else if (!isRobotDone)
            {
                Log.Error($"Робот '{robotName}' не выполнился: дело не появилось в карточке сделки.");
            }
            else if (!isRobotDoneMark)
            {
                Log.Error($"Робот '{robotName}' не выполнился: отсутствует отметка выполнения (галочка).");
            }
            else if (!isRobotCreate)
            {
                Log.Error($"Робот '{robotName}' не создался");
            }
        }
    }
}