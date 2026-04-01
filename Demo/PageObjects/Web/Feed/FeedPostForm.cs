using Demo.BaseFramework;
using Demo.SeleniumFramework;
using Demo.SeleniumFramework.DriverActions;
using OpenQA.Selenium;

namespace Demo.PageObjects.Web.Feed
{
    /// <summary>
    /// Форма добавления нового сообщения в ленту
    /// </summary>
    public class FeedPostForm
    {
        public FeedPostForm(IWebDriver driver = default)
        {
            Driver = driver;
        }

        public IWebDriver Driver { get; }
        
        private WebItemWrap EditorIframe => new WebItemWrap(
            "//iframe[@class='bx-editor-iframe']",
            "iframe редактора поста"
        );

        private WebItemWrap PostContentMessage => new WebItemWrap(
            "//body[@contenteditable='true']",
            "Поле ввода текста поста"
        );
        
        private WebItemWrap ButtomSendPost => new WebItemWrap(
            "//span[@id='blog-submit-button-save']",
            "Кнопка 'Отправить' в форме создания поста");

        public bool IsRecipientPresent(string recipientName)
        {
            var recipientsArea = new WebItemWrap("//div[@id='entity-selector-oPostFormLHE" +
                "_blogPostForm']//div[@class='ui-tag-selector-items']",
                "Область получателей поста");
            bool isRecipientPresent = WaitersCore.WaitForConditionReached(() => recipientsArea.AssertTextContaining(recipientName, default, Driver), 2, 6,
                $"Ожидание появления '{recipientName}' " +
                $"в '{recipientsArea.Description}'");
            return isRecipientPresent;
        }

        public FeedPostForm PostText(string postText)
        {
            EditorIframe.SwitchToFrame(Driver);
            // ставим фокус
            PostContentMessage.Click(Driver);
            // вводим текст
            PostContentMessage.SendKeys(postText, Driver);
            // возвращаемся обратно
            DriverActionsWeb.SwitchToDefaultContent(Driver);

            return this;
        }

        public FeedPage SendPost()
        {
            ButtomSendPost.Click(Driver);
            return new FeedPage(Driver);
        }
    }
}
