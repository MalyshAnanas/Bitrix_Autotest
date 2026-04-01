using Demo.BaseFramework;
using Demo.SeleniumFramework;
using Demo.SeleniumFramework.DriverActions;
using OpenQA.Selenium;

namespace Demo.PageObjects.Web.Feed
{
    public class FeedPage
    {
        private WebItemWrap BtnPostCreate =>
            new WebItemWrap("//div[@id='microoPostFormLHE_blogPostForm_inner']",
                "Область в ленте 'Написать сообщение'");
        
        private WebItemWrap PostRoot(string postText) =>
            new WebItemWrap(
                $"//div[contains(@class,'feed-post-block')]\n" +
                $"    [.//div[contains(@class,'feed-post-text') and normalize-space()='{postText}']]\n",
                "Корень поста"
            );
        
        private WebItemWrap ReplyRoot (string postText, string commentText, string replyText) =>
            new WebItemWrap($"//div[contains(@class,'feed-post-block')]" 
                            +$"[.//div[contains(@class,'feed-post-text') and normalize-space()='{postText}']]"
                            +$"//div[contains(@class,'feed-com-main-content')][.//div[normalize-space()='{commentText}']]"
                            +$"//div[contains(@class,'feed-com-main-content')][.//div[normalize-space()='{replyText}']]",
        "Корень созданного  ответа на комментарий");
        
        public FeedPage(IWebDriver driver = default)
        {
            Driver = driver;
        }

        public IWebDriver Driver { get; }

        public FeedPostForm OpenAddPostForm()
        {
            BtnPostCreate.Click(Driver);
            return new FeedPostForm(Driver);
        }
        

        public FeedPost FindPost(string postText)
        {
            PostRoot(postText).WaitDisplayed(15, Driver);
            return new FeedPost(Driver);
        }

        public FeedPage IsReplyExists(string postText, string commentText, string replyText)
        {
            bool result = ReplyRoot(postText, commentText, replyText).WaitDisplayed(10, Driver);

            if (!result)
                throw new Exception(
                    $"Ответ '{replyText}' не найден под комментарием '{commentText}' в посте '{postText}'"
                );

            return this;
        }

        public FeedPage RefreshFeedPage()
        {
            DriverActionsWeb.Refresh(Driver);
            return new FeedPage(Driver);
        }
    }
}
