using Demo.TestEntities;
using Newtonsoft.Json;

namespace Demo.BaseFramework.ScriptInterraction
{
    public class DatabaseExecutor
    {
        public static List<dynamic> ExecuteQuery(string query, Uri siteUri, User user)
        {
            var phpExecutor = new PHPexecutor(siteUri, user.Login, user.Password);
            string php = "global" +
                " $DB;\r\n" +
                $"$res = $DB->Query(\"{query}\");";
            php += @"
            $rows = [];
            while ($row = $res->Fetch()) 
	            $rows[] = $row;
            echo json_encode($rows);";
            string json = phpExecutor.Execute(php);
            return JsonConvert.DeserializeObject<List<dynamic>>(json);
        }
    }
}
