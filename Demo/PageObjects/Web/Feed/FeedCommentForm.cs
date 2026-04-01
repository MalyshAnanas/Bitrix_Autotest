using Demo.BaseFramework;
using Demo.SeleniumFramework;
using Demo.SeleniumFramework.DriverActions;
using OpenQA.Selenium;

namespace Demo.PageObjects.Web.Feed;

public class FeedCommentForm
{
    public FeedCommentForm(IWebDriver driver = default)
    {
        Driver = driver;
    }

    public IWebDriver Driver { get; }

    private WebItemWrap EditorIframe => new WebItemWrap(
        "//iframe[@class='bx-editor-iframe']",
        "iframe редактора комментария"
    );
    private WebItemWrap CommentContentMessage => new WebItemWrap(
        "//body[@contenteditable='true']",
        "Область для написания комментария");
    
    private WebItemWrap ButtomSendComment => new WebItemWrap(
        "//button[contains(@id,'lhe_button_submit_blogCommentForm')]",
        "Кнопка 'Отправить' в форме создания комментария");
    
    public FeedCommentForm AddCommentText(string commentText)
    {
        EditorIframe.SwitchToFrame(Driver);
        // ставим фокус
        CommentContentMessage.Click(Driver);
        // вводим текст
        CommentContentMessage.SendKeys(commentText, Driver);
        // возвращаемся обратно
        DriverActionsWeb.SwitchToDefaultContent(Driver);

        return this;
    }

    public FeedPage SendComment()
    {
        DriverActionsWeb.SwitchToDefaultContent(Driver);
        ButtomSendComment.Click(Driver);
        return new FeedPage(Driver);
    }
}