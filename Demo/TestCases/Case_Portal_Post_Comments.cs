using System.Security.Cryptography;
using Demo.BaseFramework;
using Demo.PageObjects.Web;
using Demo.SeleniumFramework.DriverActions;

namespace Demo.TestCases
{
    public class Case_Portal_Post_Comments : TestCaseCollectionBuilder
    {
        protected override List<ExecutableTestCase> GetCases()
        {
            var caseCollection = new List<ExecutableTestCase>();
            caseCollection.Add(new ExecutableTestCase("Создание ответа на комментарий от другого юзера",
                homePage => CreatePostAndComments(homePage)));
            return caseCollection;
        }

        void CreatePostAndComments(WebHomePage homePage)
        {
            string postText = "Тестовый текст поста " + HelperMethodsCore.GetDateTimeSalt();
            string commentText = "Ответ на пост " + HelperMethodsCore.GetDateTimeSalt();
            string replyText = "Ответ на комментарий " + HelperMethodsCore.GetDateTimeSalt();
            
            homePage
                //нажимаем на "написать сообщение"
                .FeedPage.OpenAddPostForm()
                //пишем текст
                .PostText(postText)
                //нажимаем на "Отправить"
                .SendPost();

            var user = ExecutableTestCase.RunningTestCase.CreatePortalTestUser(false);
            //заходим через пользователя2
            var driver2 = DriverActionsWeb.CreateNewDriver();
            var homePage2 = new WebLoginPage(ExecutableTestCase.RunningTestCase.TestPortal, driver2).Login(user);
            //находим нужный пост
            homePage2
                .FeedPage.FindPost(postText)
                //нажимаем на "добавить комментарий"
                .CommentClick()
                //пишем текст
                .AddCommentText(commentText)
                //нажимаем на "Отправить"
                .SendComment();
            
            //заходим через пользователя1
            homePage
                //перезагружаем страницу
                .FeedPage.RefreshFeedPage()
                //находим наш пост
                .FindPost(postText)
                //находим комментарий
                .FindComment(commentText)
                //нажимаем на "Ответить"
                .ReplyClick()
                //пишем текст
                .AddCommentText(replyText)
                //нажимаем на "Отправить"
                .SendComment();
            
            //проверка по созданию ответа на комменатрий
            homePage.FeedPage
                .IsReplyExists(postText, commentText, replyText);
        }
        
    }
}

