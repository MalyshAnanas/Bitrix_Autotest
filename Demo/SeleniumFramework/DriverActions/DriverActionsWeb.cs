using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Demo.BaseFramework.LogTools;
using Demo.PageObjects.Web;
using Demo.TestEntities;

namespace Demo.SeleniumFramework.DriverActions
{
    public class DriverActionsWeb : DriverActionsBase
    {
        /// <summary>
        /// Создаёт объект вебдрайвера
        /// </summary>
        /// <returns></returns>
        public static IWebDriver CreateNewDriver()
        {
            IWebDriver driver;
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return driver;
        }

        /// <summary>
        /// Закрывает текущий стоковый драйвер, 
        /// создаёт новый стоковый драйвер
        /// и авторизует заданного юзера на заданный портал
        /// </summary>
        /// <param name="user"></param>
        /// <param name="portal"></param>
        /// <returns></returns>
        public static WebHomePage ReloginAndReplaceDefaultDriver(User user, PortalData portal)
        {
            WebItemWrap.DefaultDriver.Quit();
            WebItemWrap.DefaultDriver = default;
            return new WebLoginPage(portal)
                .Login(user);
        }

        /// <summary>
        /// Обновляет текущую страницу
        /// </summary>
        /// <param name="driver"></param>
        public static void Refresh(IWebDriver driver = default)
        {
            Log.Info($"{nameof(Refresh)}");
            driver ??= WebItemWrap.DefaultDriver;
            driver.Navigate().Refresh();
        }

        /// <summary>
        /// Переход на заданный адрес
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="driver"></param>
        public static void OpenUri(Uri uri, IWebDriver driver = default)
        {
            Log.Info($"{nameof(OpenUri)}: {uri}");
            driver ??= WebItemWrap.DefaultDriver;
            driver.Navigate().GoToUrl(uri);
        }

        /// <summary>
        /// Обрабатывает алерт на странице (да\нет). 
        /// Если алерта нет, то сгенерирует исключение.
        /// </summary>
        /// <param name="accept"></param>
        /// <param name="driver"></param>
        public static void BrowserAlert(bool accept, IWebDriver driver = default)
        {
            driver ??= WebItemWrap.DefaultDriver;
            IAlert alert = driver.SwitchTo().Alert();
            string alertText = alert.Text;
            string result = $"Алерт браузера '{alertText}': нажата кнопка ";

            if (accept)
            {
                alert.Accept();
                result += "ОK";
            }
            else
            {
                alert.Dismiss();
                result += "Отмена";
            }

            Log.Info(result);
        }

        /// <summary>
        /// Переключает контекст драйвера в исходное состояние
        /// </summary>
        /// <param name="driver"></param>
        public static void SwitchToDefaultContent(IWebDriver driver = default)
        {
            Log.Info($"{nameof(SwitchToDefaultContent)}");
            driver ??= WebItemWrap.DefaultDriver;
            driver.SwitchTo().DefaultContent();
        }
    }
}
