using Demo.BaseFramework.LogTools;
using Demo.PageObjects.Web;
using Demo.PageObjects.Web.Feed;
using Demo.BaseFramework;

namespace Demo.TestCases
{
    public class Case_Portal_Settings : TestCaseCollectionBuilder
    {
        protected override List<ExecutableTestCase> GetCases()
        {
            var caseCollection = new List<ExecutableTestCase>();
            caseCollection.Add(new ExecutableTestCase("Настройка 'всем по умолчанию'", homePage => SendToAllSetting(homePage)));
            return caseCollection;
        }

        void SendToAllSetting(WebHomePage homePage)
        {
            string assertPhrase = "Всем " +
                "сотрудникам";
            //Подготовка к кейсу, если галочка снята, то надо её установить обратно
            if (new FeedPage().OpenAddPostForm().IsRecipientPresent(assertPhrase) == false)
            {
                homePage
                    .SideMenu
                    .OpenSettings()
                    .EnableSendToAllDefault()
                    .Save();

                bool isAllRecipientsDisplayed2 = homePage
                    .SideMenu
                    .OpenNews()
                    .OpenAddPostForm()
                    .IsRecipientPresent(assertPhrase);

                if (!isAllRecipientsDisplayed2)
                {
                    Log.Error("Не Отображается 'Всем сотрудникам' в получателях поста," +
                        " но при этом галочка в настройках установлена");
                }
            }

            //открыть настройки
            //снять галку адресовать всем по умолчанию
            //применить настройки
            //перейти в ленту
            //начать создавать пост в ленту и проверить что получатель 'Все сотрудники' пропал
            homePage
                .SideMenu
                .OpenSettings()
                .DisableSendToAllDefault()
                .Save();

            bool isAllRecipientsDisplayed = homePage
                .SideMenu
                .OpenNews()
                .OpenAddPostForm()
                .IsRecipientPresent(assertPhrase);

            if (isAllRecipientsDisplayed)
            {
                Log.Error("Отображается 'Всем сотруднкам' в получателях поста," +
                    " но не должно, потому что галка в настройках снята");
            }
        }
    }
}
