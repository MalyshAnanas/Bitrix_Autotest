using Demo.SeleniumFramework;
using OpenQA.Selenium;

namespace Demo.PageObjects.Web.Feed;

public class FeedComment
{
    public FeedComment(IWebDriver driver = default)
    {
        Driver = driver;
    }

    public IWebDriver Driver { get; }

    private WebItemWrap BtnReplyComment => new WebItemWrap("//a[@class='feed-com-reply feed-com-reply-Y']",
        "Кнопка 'Ответить' в созданном комментарии");

    public FeedCommentForm ReplyClick()
    {
        BtnReplyComment.Click(Driver);
        return new FeedCommentForm(Driver);
    }
    
}