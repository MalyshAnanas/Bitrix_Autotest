using Demo.BaseFramework.LogTools;
using Demo.PageObjects.Web;
using Demo.SeleniumFramework.DriverActions;
using Demo.BaseFramework;
using Demo.SeleniumFramework;
using Demo.TestEntities;

namespace Demo.TestCases
{
    public class Case_Tasks_Web : TestCaseCollectionBuilder
    {
        protected override List<ExecutableTestCase> GetCases()
        {
            return new List<ExecutableTestCase>
            {
                new ExecutableTestCase("Базовое создание задачи",
                    homePage => CreateTask(homePage)),
                new ExecutableTestCase("Базовое редактирование задачи",
                    (WebHomePage homePage) => throw new NotImplementedException("Тест редактирования задачи не реализован")),
                new ExecutableTestCase("Базовое удаление задачи",
                    (WebHomePage homePage) => { Thread.Sleep(5000); Log.Error("some error"); }),
            };
        }

        public static void CreateTask(WebHomePage homePage)
        {
            //пример использования API для добавления юзеров на портал, а также использования второго драйвера
            var regularEmployee = ExecutableTestCase.RunningTestCase.CreatePortalTestUser(false);//обычный сотрудник портала
            var collaber = ExecutableTestCase.RunningTestCase.CreatePortalTestUser(true);//коллабер

            foreach (var user in new[] { regularEmployee, collaber })
            {
                var driver2 = DriverActionsWeb.CreateNewDriver();
                var homePage2 = new WebLoginPage(ExecutableTestCase.RunningTestCase.TestPortal, driver2).Login(user);
                homePage2.SideMenu.OpenTasks();
            }

            //пример кода тесткейса здорового человека:
            homePage
                .SideMenu
                .OpenTasks();

            //пример кода тесткейса курильщика (отсутствие PO):
            var btnAddTask = new WebItemWrap("//a[@id='tasks" +
                "-buttonAdd']", "Кнопка создания задачи");
            btnAddTask.Click();
            //переключиться в контекст слайдера задачи
            var sliderFrame = new WebItemWrap("//iframe[@class='side" +
                "-panel-iframe']", "Элемент фрейма слайдера");
            sliderFrame.SwitchToFrame();
            //ввести название и описание
            var inputTaskTitle = new WebItemWrap("//input[@data-" +
                "bx-id='task-edit-title']", "Инпут названия задачи");
            var task = new B24TaskEntity("testTask" + DateTime.Now.Ticks)
            {
                Description = "Сварить макароны" + DateTime.Now.Ticks
            };
            inputTaskTitle.SendKeys(task.Name);
            var editorFrame = new WebItemWrap("//iframe[@class='bx" +
                "-editor-iframe']", "Фрейм редактора текста");
            editorFrame.SwitchToFrame();
            var body = new WebItemWrap("//body", "Just body");
            body.SendKeys(task.Description);

            //тут должен быть код сохрания и перехода в грид задач

            DriverActionsWeb.SwitchToDefaultContent();
            var taskLink = new WebItemWrap($"//a[contains(text(), " +
                $"'{task.Name}') and contains(@class, 'task-title')]",
                $"Название задачи '{task.Name}' в гриде");
            taskLink.WaitDisplayed();
            taskLink.Click();
            sliderFrame.SwitchToFrame();
            //открыть задачу, ассертнуть название и описание
            var taskNameArea = new WebItemWrap($"//div[@class='tasks-iframe-header']//span[@id='pagetitle']",
                "Заголовок задачи");
            taskNameArea.WaitDisplayed(10);
            taskNameArea.AssertTextContaining(task.Name, "Название задачи не соответствует ожидаемому");
            var taskDescription = new WebItemWrap($"//div[@id='task-detail-description']",
                "Описание задачи");
            taskDescription.AssertTextContaining(task.Description, "Описание задачи не соответствует ожидаемому");
        }
    }
}
