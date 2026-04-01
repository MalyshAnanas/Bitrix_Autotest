using Demo.BaseFramework;
using Demo.BaseFramework.ScriptInterraction;
using Demo.PageObjects.Web;
using Demo.PageObjects.Web.Disk;

namespace Demo.TestCases;

public class Case_Portal_DeleteRecycleBin : TestCaseCollectionBuilder
{
    protected override List<ExecutableTestCase> GetCases()
    {
        var caseCollection = new List<ExecutableTestCase>();
        caseCollection.Add(new ExecutableTestCase("Удаление файла из корзины вторым пользователем",
            homePage => DeleteFileRecycleBin(homePage)));
        return caseCollection;
    }

    void DeleteFileRecycleBin(WebHomePage homePage)
    {
        // Создаём файлы и удаляем их в корзину (занимает примерно 2 минуты)
        ExecutableTestCase.RunningTestCase.CreateAndDeleteFileInCommonFolder();
        // Находимся на главной странице

        RecycleBinPage test = homePage
            // Переходим в левое меню
            .SideMenu
            // Нажимаем кнопку "Диск"
            .OpenDisk()
            // Переходим в верхнее меню
            .GoToTopMenu()
            // Нажимаем на вкладку "Общий диск"
            .OpenSharedData()
            // Переходим в корзину
            .OpenRecycleBin();
        // Получаем максимальное количество страниц
        RecycleBinPage test2 = test;
        int countFile = test.GetCountFile();
        

            // Переходим на вторую страницу корзину
            test2.OpenPage(2)
            // Удаляем навсегда первый файл на странице
            .Delete();
            
        // Смотрим, что файл отсутствует в корзине
        // Переходим на первую страницу
        test2.OpenPage(1);
        // Пересчитываем количество файлов
        int countFileAfterDelete = test2.GetCountFile();
        if (countFileAfterDelete != countFile)
        {
            throw new Exception($"Не удалился файл");
        }

    }
}