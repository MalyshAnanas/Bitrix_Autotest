using Demo.BaseFramework.ScriptInterraction;
using Demo.BaseFramework.LogTools;
using Demo.SeleniumFramework;
using Demo.PageObjects.Web;
using Demo.TestEntities;
using Demo.PageObjects.Mobile;

namespace Demo.BaseFramework
{
    public class ExecutableTestCase
    {
        public static ExecutableTestCase RunningTestCase { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title">Название тесткейса</param>
        /// <param name="body">Ссылка на метод тела кейса</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ExecutableTestCase(string title, Action<WebHomePage> body)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Body = body ?? throw new ArgumentNullException(nameof(body));
            Node = new ExecutableTestCaseTreeNode(title);
            EnvType = TestCaseEnvType.Web;
        }

        public ExecutableTestCase(string title, Action<MobileAppHomePage> body)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            MobileBody = body ?? throw new ArgumentNullException(nameof(body));
            Node = new ExecutableTestCaseTreeNode(title);
            EnvType = TestCaseEnvType.Mobile;
        }

        int logCounter = 0;

        public void Execute(PortalData testPortal, Action uiRefresher)
        {
            TestPortal = testPortal;
            Status = TestCaseRunStatus.running;
            uiRefresher.Invoke();
            RunningTestCase = this;
            logCounter++;
            CaseLogPath = Path.Combine(Environment.CurrentDirectory, $"caselog{DateTime.Now:ddMMyyyyHHmmss}{logCounter}.html");
            Log.WriteHtmlHeader(CaseLogPath);
            uiRefresher.Invoke();

            try
            {
                Log.Info($"--------------Запуск теста '{Title}'--------------");
                if (TestPortal.Adress.Scheme == Uri.UriSchemeHttps)
                    IsCloud = true;

                if (EnvType == TestCaseEnvType.Web)
                {
                    var portalLoginPage = new WebLoginPage(TestPortal);
                    var homePage = portalLoginPage.Login(TestPortal.Admin);
                    Body.Invoke(homePage);
                }
                else
                {
                    var loginPage = new MobileAppLoginPage(TestPortal);
                    var homePage = loginPage.Login(TestPortal.Admin);
                    MobileBody.Invoke(homePage);
                }

            }
            catch (Exception e)
            {
                Log.Error($"Кейс не прошёл, причина:{Environment.NewLine}{e}");
            }

            Log.Info($"------------Тест '{Title}' завершён------------");

            try
            {
                if (BaseItem._defaultDriver != default)
                {
                    BaseItem.DefaultDriver.Quit();
                    BaseItem.DefaultDriver = default;
                }
            }
            catch (Exception) { }

            if (CaseLog.Any(x => x is LogMessageError))
                Status = TestCaseRunStatus.failed;
            else
                Status = TestCaseRunStatus.passed;

            RunningTestCase = default;
            uiRefresher.Invoke();
        }

        /// <summary>
        /// Выполняет php код в админке портала текущего кейса через "Настройки -> Командная PHP строка"
        /// </summary>
        /// <param name="phpCode"></param>
        /// <returns>Результат выполнения кода (если код в принципе что-то выводит)</returns>
        public string ExecutePHP(string phpCode)
        {
            if (IsCloud)
                throw new Exception("Выполнение php на облаке невозможно");
            var phpExecutor = new PHPexecutor(TestPortal.Adress, TestPortal.Admin.Login, TestPortal.Admin.Password);
            return phpExecutor.Execute(phpCode);
        }

        /// <summary>
        /// Генерирует нового сотрудника на портале
        /// </summary>
        /// <param name="extranetUser">Если задано, то создаст пользователя эксранета</param>
        /// <returns></returns>
        public User CreatePortalTestUser(bool extranetUser)
        {
            if (IsCloud)
                throw new Exception("Генерация юзеров на облаке невозможна");
            var user = Employee_Tools.GenerateValidUserData();
            if(extranetUser)
                Employee_Tools.AddUserExtranet(user, TestPortal.Admin, TestPortal.Adress);
            else
                Employee_Tools.AddUserIntranet(user, TestPortal.Admin, TestPortal.Adress);
            return user;
        }

        /// <summary>
        /// Создаёт и отправляет в корзину файл в "Общем диске".
        /// </summary>
        public void CreateAndDeleteFileInCommonFolder()
        {
            for (int i = 0; i < 51; i++)
            {
                int idFile = File_Tools.CreateFileInCommonFolder(TestPortal.Admin, TestPortal.Adress, "testFile");
                File_Tools.DeleteFileInCommonFolder(TestPortal.Admin, TestPortal.Adress, idFile);
            }

        }

        public string Title { get; set; }
        Action<WebHomePage> Body { get; set; }
        Action<MobileAppHomePage> MobileBody { get; set; }
        public ExecutableTestCaseTreeNode Node { get; set; }
        public string CaseLogPath { get; set; }
        public List<LogMessage> CaseLog { get; } = new List<LogMessage>();
        public TestCaseRunStatus Status { get; set; }
        public TestCaseEnvType EnvType { get; set; }
        public bool IsCloud { get; set; }
        public PortalData TestPortal { get; set; }
    }

    public enum TestCaseEnvType
    {
        Web,
        Mobile
    }
}
