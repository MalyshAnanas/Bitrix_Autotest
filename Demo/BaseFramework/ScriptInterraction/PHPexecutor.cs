using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Demo.BaseFramework.LogTools;

namespace Demo.BaseFramework.ScriptInterraction
{
    public class PHPexecutor
    {
        public PHPexecutor(Uri siteUri, string adminLogin, string adminPassword)
        {
            SiteUri = siteUri;
            Login = adminLogin;
            Password = adminPassword;
        }

        public Uri SiteUri { get; }
        public string Login { get; }
        public string Password { get; }

        const string cliPath = "php_command_line.php";
        const string adminPath = "bitrix/admin/";
        Uri CLI_Uri => new Uri($"{SiteUri}{adminPath}{cliPath}?lang=ru");

        public string Execute(string php)
        {
            Log.Info($"Run php:\r\n{php}");
            HttpClient client = GetHttpClient(out string sessionId);
            string result = MakeRequest(php, client, sessionId);
            Log.Info($"PHP run result:\r\n{result}");
            return result;
        }

        private static string MakeRequest(string php, HttpClient client, string sessionId)
        {
            var requestParameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(
                    "query", php),
                new KeyValuePair<string, string>(
                    "result_as_text"
                    , "y"),
                new KeyValuePair<string, string>(
                    "ajax"
                    , "y"),
                new KeyValuePair<string, string>(
                    "sessid", sessionId)
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "");
            requestMessage.Content = new FormUrlEncodedContent(requestParameters);
            var response = client.SendAsync(requestMessage).Result;
            string responseBody = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode != HttpStatusCode.OK
                || responseBody?.Contains("Время") != true
                || responseBody?.Contains("выполнения:") != true
                )
            {
                throw new Exception("php run failed, response:\r\n" + responseBody);
            }

            string result = default;
            if (responseBody != null)
            {
                result = HttpUtility.HtmlDecode(responseBody);
                if (result.Contains("<pre>"))
                {
                    result = HtmlParser.GetInnerTextFromHtml(
                        new List<string> { "//pre" }, responseBody, true);
                    result = HttpUtility.HtmlDecode(result);
                }
            }

            return result;
        }

        private HttpClient GetHttpClient(out string sessionId)
        {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            var client = new HttpClient(handler) { BaseAddress = CLI_Uri };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            var rqParameters = new List<KeyValuePair<string, string>>();
            rqParameters.Add(new KeyValuePair<string, string>(
                "grant_type",
                "client_credentials"));
            var content = new FormUrlEncodedContent(rqParameters);
            var authenticationString = $"{Login}:{Password}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(authenticationString));
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            requestMessage.Content = content;
            var response = client.SendAsync(requestMessage).Result;
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            sessionId = ParseSessionId(responseBody);
            if (string.IsNullOrEmpty(sessionId))
                throw new Exception("Failed to obtain " + nameof(sessionId));
            return client;
        }

        public static string ParseSessionId(string html)
        {
            string sessid = default;
            string anchor = "var" +
                " phpVars" +
                " = {";
            int startIndex = html.IndexOf(anchor);

            if (startIndex != -1)
            {
                html = html.Substring(startIndex);
                var res = html.TakeWhile(x => x != '}').ToArray();
                string clean = new string(res);
                var splitted = clean.Split('\n').ToList();
                string sidString = splitted.Find(x => x.Contains("bi" +
                    "trix_" +
                    "sessid"));
                sessid = sidString
                    .Split(':')[1]
                    .Trim()
                    .Replace(",", "")
                    .Replace("'", "");
            }
            else
            {
                string[] anchors = {
                        "(window" +
                        ".BX||top.BX).message({'" +
                        "LANGUAGE_ID':",
                        "(window" +
                        ".BX||top.BX)" +
                        ".message({\"LANGUAGE_ID\":"
                    };

                foreach (string item in anchors)
                {
                    startIndex = html.IndexOf(item);
                    if (startIndex != -1)
                    {
                        string idTitle = "bi" +
                            "trix_" +
                            "sessid";
                        var lines = html.Split('\n').ToList();
                        var targetLine = lines.Find(x => !string.IsNullOrEmpty(x) && x.Contains(item) && x.Contains(idTitle));

                        if (targetLine != default)
                        {
                            string tale = targetLine.Remove(0, targetLine.IndexOf(idTitle) - 1)
                                .Replace("'" + idTitle
                                + "':'", "")
                                .Replace("\"" + idTitle
                                + "\":\"", "");
                            int endIndex = tale.IndexOfAny(new char[] { '\'', '"' });
                            if (endIndex != -1)
                            {
                                sessid = tale.Remove(endIndex);
                                break;
                            }
                        }
                    }
                }
            }

            return sessid;
        }
    }
}
