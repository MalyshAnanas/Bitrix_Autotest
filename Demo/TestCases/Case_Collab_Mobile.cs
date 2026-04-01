using Demo.BaseFramework;
using Demo.BaseFramework.LogTools;
using Demo.PageObjects.Mobile;
using Demo.TestEntities;

namespace Demo.TestCases;

public class Case_Collab_Mobile : TestCaseCollectionBuilder
{
    protected override List<ExecutableTestCase> GetCases()
    {
        var caseCollection = new List<ExecutableTestCase>();
        caseCollection.Add(
            new ExecutableTestCase("Создание Коллабы в мессенджере ", mobileHomePage => CreateCollab(mobileHomePage)));
        return caseCollection;
    }

    void CreateCollab(MobileAppHomePage homePage)
    {
        string collabName = "testCollab" + DateTime.Now.Ticks;
        string collabDescription = "testCollab" + DateTime.Now.Ticks;
        var collab = new B24CollabEntity(collabName, collabDescription); 

        // Находимся на главной странице
        bool isCollabPresent = homePage.TabsPanel
            // Переходим в мессенджер
            .SelectMessenger()
            // Нажимаем на +
            // Нажимаем на "Коллаба"
            .OpenCreateCollab()
            // Продолжить + Вводим название и описание
            .FillCollabInfo(collab)
            // Нажимаем на "Управление коллабой" и выбираем модератора (Выбираем нас же + продолжить)
            .GoToSettings()
            .SetModerator(collab)
            // Не разрешаем приглашать гостей (выбираем "Нет")
            .SetNoGuests()
            // Кнопка назад
            // Кнопка Создать коллабу
            .Submit()
            .IsCollabDisplayed(collab);
        
        
        //Проверка создания
        if (!isCollabPresent)
        {
            Log.Error($"Коллаба с названием {collabName} не отображается");
        }
    }
}