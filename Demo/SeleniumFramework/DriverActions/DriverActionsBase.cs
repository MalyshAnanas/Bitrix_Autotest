using Demo.BaseFramework.LogTools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

namespace Demo.SeleniumFramework.DriverActions
{
    public class DriverActionsBase
    {
        public static void ExecuteJavaScript(string scriptCode, IWebDriver driver = default)
        {
            Log.Info($"{nameof(ExecuteJavaScript)}: попытка выполнения JS:\r\n{scriptCode}");
            driver ??= WebItemWrap.DefaultDriver;
            driver.ExecuteJavaScript(scriptCode);
        }
    }
}
