using Demo.BaseFramework;
using Demo.BaseFramework.LogTools;
using Demo.PageObjects.Mobile;
using Demo.TestEntities;

namespace Demo.TestCases
{
    public class Case_Tasks_Mobile : TestCaseCollectionBuilder
    {
        protected override List<ExecutableTestCase> GetCases()
        {
            var caseCollection = new List<ExecutableTestCase>();
            caseCollection.Add(
                new ExecutableTestCase("Базовое создание задачи", mobileHomePage => CreateTask(mobileHomePage)));
            return caseCollection;
        }

        void CreateTask(MobileAppHomePage homePage)
        {
            string taskName = "testTask" + DateTime.Now.Ticks;
            var testTask = new B24TaskEntity(taskName);

            bool isTaskPresent = homePage.TabsPanel
                .SelectTasks()
                .CreateTask(testTask)
                .IsTaskDisplayed(testTask);

            if (!isTaskPresent)
            {
                Log.Error($"Задача с названием {taskName} не отображается");
            }
        }
    }
}
