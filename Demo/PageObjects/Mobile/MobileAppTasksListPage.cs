using Demo.BaseFramework;
using Demo.SeleniumFramework;
using Demo.TestEntities;

namespace Demo.PageObjects.Mobile
{
    public class MobileAppTasksListPage
    {
        public MobileAppTasksListPage CreateTask(B24TaskEntity task)
        {
            var createTaskBtn = new MobileElement("//android.view.ViewGroup" +
                "[@content-desc=\"task-list_ADD_BTN\"]",
                "Кнопка добавления новой задачи");
            createTaskBtn.Click();

            var taskNameField = new MobileElement("//android.view.ViewGroup" +
                "[@content-desc=\"title_FIELD\"]//android.widget.EditText",
                "Поле названия задачи");
            var createBtn = new MobileElement("//android.view.ViewGroup" +
                "[@content-desc=\"taskCreateToolbar_createButton\"]",
                "Кнопка подтверждения создания задачи");
            taskNameField.SendKeys(task.Name);
            createBtn.Click();

            return this;
        }

        public bool IsTaskDisplayed(B24TaskEntity task)
        {
            var taskTitle = new MobileElement($"//android.widget.TextView" +
                $"[@content-desc=\"task-list_SECTION_TITLE\" and @text=\"{task.Name}\"]",
                $"Заголовок задачи с текстом {task.Name}");

            bool isTaskDisplayed = WaitersCore.WaitForConditionReached(
                () => taskTitle.WaitDisplayed(), 2, 6,
                $"Ожидание появления задачи '{task.Name}' в списке задач");
            return isTaskDisplayed;
        }
    }
}
