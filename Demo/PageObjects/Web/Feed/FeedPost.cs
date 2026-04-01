
using Demo.BaseFramework;
using Demo.SeleniumFramework;
using Demo.SeleniumFramework.DriverActions;
using OpenQA.Selenium;

namespace Demo.PageObjects.Web.Feed;

public class FeedPost
{
    public FeedPost(IWebDriver driver)
    {
        Driver = driver;
    }

    public IWebDriver Driver { get; }

    private WebItemWrap BtnCommentCreate => new WebItemWrap(
        "//a[contains(@class,'feed-com-add-link') and normalize-space(text())='Добавить комментарий']",
        "Область в посте 'добавить комментарий'");
    
    private WebItemWrap CommentRoot(string commentText) =>
        new WebItemWrap(
            $"//div[contains(@class,'feed-com-main-content')][.//div[normalize-space()='{commentText}']]"
            , "Корень поста"
        );
    
    public FeedCommentForm CommentClick()
    {
        DriverActionsWeb.SwitchToDefaultContent(Driver);
        BtnCommentCreate.Click(Driver);
        return new FeedCommentForm(Driver);
    }

    public FeedComment FindComment(string commentText)
    {
        CommentRoot(commentText).WaitDisplayed(15, Driver);
        return new FeedComment(Driver);
    }
}