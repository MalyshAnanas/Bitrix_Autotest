using System.Diagnostics;
using OpenQA.Selenium;
using Demo.BaseFramework;
using Demo.BaseFramework.LogTools;
using Demo.SeleniumFramework.DriverActions;

namespace Demo.SeleniumFramework
{
    public abstract class BaseItem
    {
        public static IWebDriver _defaultDriver = default;
        public static IWebDriver DefaultDriver
        {
            get
            {
                if (_defaultDriver == default)
                {
                    _defaultDriver = ExecutableTestCase.RunningTestCase.EnvType == TestCaseEnvType.Web
                        ? DriverActionsWeb.CreateNewDriver()
                        : DriverActionsMobile.CreateNewMobileDriver();
                }

                return _defaultDriver;
            }

            set => _defaultDriver = value;
        }

        protected List<string> XPathes { get; set; } = new List<string>();
        public string Description { get; set; }
        public string DescriptionFull { get => $"'{Description}' икспасы: {string.Join(", ", XPathes)}"; }

        protected BaseItem(List<string> xpathes, string description)
        {
            XPathes = xpathes;
            Description = description;
        }

        public int WaitAfterActiveAction_s { get; set; } = 1;

        public void Click(IWebDriver driver = default)
        {
            WaitDisplayed(driver: driver);
            LogActionInfo(nameof(Click));

            PerformAction((button, drv) =>
            {
                button.Click();
            }, driver);

            WaitersCore.Wait_s(WaitAfterActiveAction_s);
        }

        /// <summary>
        /// Ждёт пока элемент станет видимым
        /// </summary>
        /// <param name="maxWait_s"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        public bool WaitDisplayed(int maxWait_s = 5, IWebDriver driver = default)
        {
            return WaitDisplayedBase(driver, maxWait_s, true, "Ожидание отображения " + DescriptionFull);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="maxWait_s"></param>
        /// <param name="waitDirection">Если true то будет ждать пока элемент отобразится, иначе будет ждать пока элемент отображается</param>
        /// <param name="waitDescription"></param>
        /// <returns></returns>
        protected bool WaitDisplayedBase(
            IWebDriver driver,
            int maxWait_s,
            bool waitDirection,
            string waitDescription)
        {
            driver ??= DefaultDriver;
            var impWait = driver.Manage().Timeouts().ImplicitWait;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

            bool result = WaitersCore.WaitForConditionReached(() =>
            {
                bool expectedState = false;

                PerformAction((el, drv) =>
                {
                    expectedState = el.Displayed == waitDirection;
                }, driver, true);

                return expectedState;
            }, 1, maxWait_s, waitDescription);

            driver.Manage().Timeouts().ImplicitWait = impWait;
            return result;
        }

        /// <summary>
        /// Ввод текста в поле
        /// </summary>
        /// <param name="textToInput"></param>
        /// <param name="driver"></param>
        /// <param name="logInputtedText">Писать ли вводимый текст в лог</param>
        public void SendKeys(
            string textToInput,
            IWebDriver driver = default,
            bool logInputtedText = true)
        {
            WaitDisplayed(driver: driver);
            string log = $"'{textToInput}'";
            if (!logInputtedText)
                log = "[логгирование отключено]";
            LogActionInfo($"Ввод текста {log} в элемент");

            PerformAction((input, drv) => { input.SendKeys(textToInput); }, driver);
            WaitersCore.Wait_s(WaitAfterActiveAction_s);
        }

        protected void PerformAction(
            Action<IWebElement, IWebDriver> seleniumCode,
            IWebDriver driver,
            bool throwAtDebug = false)
        {
            driver ??= DefaultDriver;

            try
            {
                foreach (var xpath in XPathes)
                {
                    IWebElement targetElement = default;
                    int staleRetryMaxCount = 3;
                    bool interceptedFirstTry = true;

                    for (int i = 0; i < staleRetryMaxCount; i++)
                    {
                        try
                        {
                            targetElement = driver.FindElement(By.XPath(xpath));
                            seleniumCode.Invoke(targetElement, driver);
                            break;
                        }
                        catch (WebDriverException ex)
                        {
                            if (ex is NoSuchElementException)
                            {
                                if (xpath == XPathes.Last())
                                    throw;
                            }
                            else if (ex is StaleElementReferenceException)
                            {
                                if (i == staleRetryMaxCount - 1)
                                    throw;
                                Thread.Sleep(2000);
                                continue;
                            }
                            else if (ex is ElementClickInterceptedException)
                            {
                                if (ex.Message.Contains("helpdesk-notification" +
                                    "-popup"))
                                {
                                    new WebItemWrap("//div[contains(@class, " +
                                        "'popup-close-btn')]", "Кнопка закрытия " +
                                        "баннера").Click(driver);
                                    if (interceptedFirstTry)
                                        i++;
                                    interceptedFirstTry = false;
                                    continue;
                                }
                                else
                                    throw;
                            }
                            else
                                throw;
                        }

                        break;
                    }

                    if (targetElement != default)
                        break;
                }
            }
            catch (Exception e)
            {
                if (throwAtDebug || !EnvSettings.IsDebug)
                    throw;
                Debug.Fail(e.ToString());
            }
        }

        protected void LogActionInfo(string actionTitle)
        {
            Log.Info($" {actionTitle}: " + DescriptionFull);
        }
        
        /// <summary>
        /// Проверяет существование объекта
        /// </summary>
        /// <param name="driver"></param>
        public bool Exists(IWebDriver driver = default)
        {
            driver ??= DefaultDriver;

            foreach (var xpath in XPathes)
            {
                var elements = driver.FindElements(By.XPath(xpath));
                if (elements.Any())
                    return true;
            }

            return false;
        }

    }
}
