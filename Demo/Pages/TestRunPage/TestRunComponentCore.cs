using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Drawing;
using Demo.BaseFramework;
using Demo.TestEntities;
using System.IO;

namespace Demo.Pages.TestRunPage
{
    public class TestRunComponentCore : ComponentBase
    {
        private readonly string _configFilePath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Demo",
                "settings.txt"
            );

        const string configFileName = "settings.txt";
        CaseCollectionCreator caseColBuilder = new CaseCollectionCreator();

        protected bool RunButtonDisabled { get; set; }
        protected List<ExecutableTestCase> ExecCaseCollection { get; set; }
        protected string PortalUri { get; set; }
        protected string PortalUriBgColor { get; set; }
        protected string LoginBgColor { get; set; }
        protected string DisplayedError { get; set; }
        protected string PwdBgColor { get; set; }
        protected User PortalUser { get; set; } = new User();

        [CascadingParameter] 
        public IModalService Modal { get; set; }

        protected void ShowLog(ExecutableTestCase testCase)
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(LogViewComponent.TestCase), testCase);
            Modal.Show<LogViewComponent>($"Лог '{testCase.Title}'", parameters);
        }

        protected void OnInputClick()
        {
            PortalUriBgColor = HelperMethodsCore.ConvertToHex(Color.White);
            LoginBgColor = HelperMethodsCore.ConvertToHex(Color.White);
            PwdBgColor = HelperMethodsCore.ConvertToHex(Color.White);
            DisplayedError = null;
        }

        protected async Task RunSelectedTests()
        {
            RunButtonDisabled = true;
            Uri portalUri = default;

            if (string.IsNullOrEmpty(PortalUri) || !Uri.TryCreate(PortalUri, UriKind.Absolute, out portalUri))
                PortalUriBgColor = HelperMethodsCore.ConvertToHex(Color.Red);
            else if (string.IsNullOrEmpty(PortalUser.Login) || !IsEmail(PortalUser.Login))
            {
                LoginBgColor = HelperMethodsCore.ConvertToHex(Color.Red);
                if (!IsEmail(PortalUser.Login))
                    DisplayedError = "Логин должен быть email-ом";
            }
            else if (string.IsNullOrEmpty(PortalUser.Password))
                PwdBgColor = HelperMethodsCore.ConvertToHex(Color.Red);
            else
            {
                File.WriteAllText(_configFilePath, $"{PortalUri}\r\n{PortalUser.Login}\r\n{PortalUser.Password}");
                var selectedCases = ExecCaseCollection.FindAll(x => x.Node.IsChecked);

                if (selectedCases.Any())
                {
                    selectedCases.ForEach(x => x.Status = TestCaseRunStatus.waitingForExecute);
                    var portalInfo = new PortalData(portalUri, PortalUser);

                    foreach (var testCase in selectedCases)
                    {
                        await Task.Run(() =>
                        {
                            try
                            {
                                testCase.Execute(portalInfo, () => InvokeAsync(StateHasChanged));
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine(ex);
                                throw;
                            }
                        });

                    }

                    return;
                }
            }

            RunButtonDisabled = false;
            StateHasChanged();
        }

        private bool IsEmail(string email)
        {
            return email?.Contains("@") == true && email?.Contains(".") == true;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Directory.CreateDirectory(
                Path.GetDirectoryName(_configFilePath)!
            );

            ExecCaseCollection = caseColBuilder.AllCaseCollection;
            OnInputClick();
            
            if(File.Exists(_configFilePath))
            {
                string configContent = File.ReadAllText(_configFilePath);
                
                if (!string.IsNullOrEmpty(configContent))
                {
                    var parts = configContent.Split("\r\n", StringSplitOptions.None);
                    
                    if(parts.Count() > 2)
                    {
                        PortalUri = parts[0];
                        PortalUser.Login = parts[1];
                        PortalUser.Password = parts[2];
                    }
                }
            }
        }
    }
}
