using Demo.BaseFramework;
using Demo.BaseFramework.LogTools;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Drawing;


namespace Demo.SeleniumFramework
{
    public class WebItemWrap : BaseItem
    {
        public WebItemWrap(string xpath, string description) 
            : this(new List<string> {xpath}, description)
        {
        }

        public WebItemWrap(List<string> xpathes, string description) 
            : base(xpathes, description)
        {
            XPathes = xpathes;
            Description = description;
        }
        
        /// <summary>
        /// Наведение на элемент
        /// </summary>
        /// <param name="driver"></param>
        public void Hover(IWebDriver driver = default)
        {
            LogActionInfo("Наведение курсора мыши");

            PerformAction((element, drv) =>
            {
                Actions action = new Actions(drv);
                action.MoveToElement(element).Build().Perform();
            }, driver);

            WaitersCore.Wait_s(WaitAfterActiveAction_s);
        }

        /// <summary>
        /// Переключение контекста драйвера в iframe
        /// </summary>
        /// <param name="driver"></param>
        public void SwitchToFrame(IWebDriver driver = default)
        {
            LogActionInfo(nameof(SwitchToFrame));
            PerformAction((frame, d) => { d.SwitchTo().Frame(frame); }, driver);
        }

        /// <summary>
        /// Выбирает элемент в списке по его тексту
        /// </summary>
        /// <param name="listItemToSelect">Текст элемента</param>
        /// <param name="driver"></param>
        /// <exception cref="Exception"></exception>
        public void SelectListItemByText(
            string listItemToSelect,
            IWebDriver driver = default)
        {
            WaitDisplayed(driver: driver);
            LogActionInfo($"Выбор пункта '{listItemToSelect}' в списке");

            PerformAction((el, drv) =>
            {
                var selEl = new SelectElement(el);
                string optionToSelectResultText = default;
                bool optionFound = selEl.Options.ToList()
                    .Find(x => x.Text == listItemToSelect) != null;

                if (!optionFound)
                    optionToSelectResultText = selEl.Options.ToList().Find(x => x.Text.Contains(listItemToSelect))?.Text;
                else
                    optionToSelectResultText = listItemToSelect;

                if (optionToSelectResultText != null)
                    selEl.SelectByText(optionToSelectResultText);
                else
                    throw new Exception($"Пункт '{listItemToSelect}' не найден в списке {DescriptionFull}");
            }, driver);

            WaitersCore.Wait_s(WaitAfterActiveAction_s);
        }

        /// <summary>
        /// Отмечен ли чекбокс/элемент списка
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public bool Checked(IWebDriver driver = default)
        {
            WaitDisplayed(driver: driver);
            bool @checked = false;

            PerformAction((checkBox, drv) => { @checked = checkBox.Selected; }, driver);

            LogActionInfo($"{(@checked ? "Отмечен" : "Снят")}. Элемент");
            return @checked;
        }

        /// <summary>
        /// Получает значение аттрибута элемента
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        public string GetAttribute(string attributeName, IWebDriver driver = default)
        {
            string resultAttrValue = default;

            PerformAction((el, drv) => { resultAttrValue = el.GetAttribute(attributeName); }, driver);

            LogActionInfo($"{attributeName}='{resultAttrValue}'. Элемент");
            return resultAttrValue;
        }

        /// <summary>
        /// Проверяет наличие заданной подстроки в свойстве Text элемента
        /// </summary>
        /// <param name="expectedText"></param>
        /// <param name="failMessage">Если задано, то выведет ошибку в случае отсутствия искомой подстроки в тексте элемента</param>
        /// <param name="driver"></param>
        /// <returns>true if expectedText present at element's innerText</returns>
        public bool AssertTextContaining(string expectedText, string failMessage = default, IWebDriver driver = default)
        {
            LogActionInfo(nameof(AssertTextContaining));
            bool result = false;
            string factText = default;
            
            PerformAction((targetElement, drv) =>
            {
                factText = targetElement.Text;
                result = !string.IsNullOrEmpty(factText) && factText.Contains(expectedText);
            }, driver);

            if(!result && failMessage != default)
                Log.Error(failMessage + $"\r\nОжидалось вхождение '{expectedText}'\r\nНо было: '{factText}'");
            return result;
        }

        /// <summary>
        /// Получает весь текст, содержащийся среди потомков элемента
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public string InnerText(IWebDriver driver = default)
        {
            string elementText = default;

            PerformAction((targetElement, drv) => { elementText = targetElement.Text; }, driver);

            LogActionInfo($"Текст '{elementText}'. Элемент");
            return elementText;
        }
        

        /// <summary>
        /// Ждёт пока элемент отображается
        /// </summary>
        /// <param name="maxWait_s"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        public bool WaitWhileDisplayed(int maxWait_s = 5, IWebDriver driver = default)
        {
            return WaitDisplayedBase(driver, maxWait_s, false, "Ожидание пропадания элемента " + DescriptionFull);
        }

        /// <summary>
        /// Получает размеры элемента
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="noLog"></param>
        /// <returns></returns>
        public Size Size(IWebDriver driver = default, bool noLog = false)
        {
            Size elementSize = default;

            PerformAction((targetElement, drv) => { elementSize = targetElement.Size; }, driver);

            if (!noLog)
                LogActionInfo($"Размер '{elementSize}'. Элемент");
            return elementSize;
        }
    }
}
