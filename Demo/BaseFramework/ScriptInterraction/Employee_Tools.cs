using Demo.BaseFramework.LogTools;
using Demo.TestEntities;

namespace Demo.BaseFramework.ScriptInterraction
{
    public class Employee_Tools
    {
        public static void AddUserIntranet(User userToAdd, User admin, Uri siteUri, int departmentId = 1)
        {
            AddUser(userToAdd, admin, siteUri, departmentId);
        }

        public static void AddUserExtranet(User userToAdd, User admin, Uri siteUri)
        {
            AddUser(userToAdd, admin, siteUri, 0);
        }

        static void AddUser(
            User user,
            User admin,
            Uri siteUri,
            int departmentID)
        {
            bool isExtra = departmentID == 0;
            string addPhp = @"
		    if ($arrFields[""DEPARTMENT_ID""] == 0)
			    unset($arrFields[""DEPARTMENT_ID""]);
		    else
			    $arrFields[""DEPARTMENT_ID""] = [$arrFields[""DEPARTMENT_ID""]];
		    $SITE_ID = ""s1"";
		    $strError = """";
		    $userID = 0;
		    $userLogin = $arrFields[""ADD_EMAIL""];
		    $userID = CIntranetInviteDialog::AddNewUser($SITE_ID, $arrFields, $strError);
		
		    if ($userID > 0) {
			    echo ""Пользователь "";
			    echo $userLogin;
			    echo "" добавлен на портал с ID "";
			    if(is_array($userID))
				    echo $userID[0];
			    else
				    echo $userID;
			    echo ""<br/>"";
		    }

		    if ($strError != """") 
		    {
			    if(is_array($strError))
				    print_r($strError);
			    else
				    echo $strError;
		    }";

            addPhp =
                $"$arrFields[\"ADD_NAME\"] = " +
                $"'{user.Name}';\r\n" +
                $"$arrFields[\"ADD_LAST_NAME\"] = " +
                $"'{user.LastName}';\r\n" +
                $"$arrFields[\"ADD_EMAIL\"] = " +
                $"'{user.Login}';\r\n" +
                $"$arrFields[\"DEPARTMENT_ID\"] = " +
                $"'{departmentID}';\r\n" + addPhp;

            var phpExecutor = new PHPexecutor(siteUri, admin.Login, admin.Password);
            string execResult = phpExecutor.Execute(addPhp);
            if (execResult?.Contains("Пользователь " + user.Login + " добавлен на портал с ID") != true)
                throw new Exception($"Ошибка добавления {user.Login} на портал:( \r\n: {execResult}");
            SetUserPassword(user, siteUri, admin);
            ConfirmEmail(user, siteUri, admin);

            if (isExtra)
                BxPortalGroup.Freelancee.AddUserToGroupByScript(user, siteUri, admin);
            else
            {
                var result = DatabaseExecutor.ExecuteQuery("SELECT ID from b_group " +
                    "WHERE STRING_ID = 'EMPLOYEES_s1'", siteUri, admin);
                string employeesGroupId = result.Count == 0 ? null : result[0].ID;
                if (string.IsNullOrEmpty(employeesGroupId))
                    throw new Exception("Failed to get group ID for 'Сотрудники'");
                phpExecutor.Execute($"CUser::" +
                    $"AppendUserGroup({user.GetDBid(siteUri, admin)}, {employeesGroupId})");
            }

            Log.Info($"Пользователь {user.NameLastName} добавлен на портал");
        }

        static void SetUserPassword(
            User user,
            Uri siteUri,
            User admin)
        {
            string userId = user.GetDBid(siteUri, admin);
            string userGroupIds = GetUserGroupIds(userId, siteUri, admin);
            string pwdToSet = user.Password;
            if (string.IsNullOrEmpty(userId))
                throw new Exception($"Юзера {user.Login} нет на портале");

            string php =
                $"$arrFields[\"PASSWORD\"]" +
                $" = '{pwdToSet}';\r\n" +
                $"$groupIDS = " +
                $"'{userGroupIds}';\r\n" +
                $"$userID = " +
                $"'{userId}';\r\n";

            php += @"
                $oUser = new CUser;
		        $arrGroupsIDs = explode("","", $groupIDS);
		        $rUser = CUser::GetByID($userID);
		        $arrUser = $rUser->Fetch();

		        foreach ($arrFields as $key => $value) 
		        {
			        if($key != ""PASSWORD"")
				        $arrUser[$key] = $value;
		        }

		        $fields = 
                    array(
			        ""NAME"" => $arrUser[""NAME""],
			        ""LAST_NAME"" => $arrUser[""LAST_NAME""],
			        ""EMAIL"" => $arrUser[""EMAIL""],
			        ""LOGIN"" => $arrUser[""LOGIN""],
			        ""LID"" => $arrUser[""LID""],
			        ""ACTIVE"" => $arrUser[""ACTIVE""],
			        ""GROUP_ID"" => $arrGroupsIDs,
		        );

		        if($arrFields[""PASSWORD""] != null)
		        {
			        $fields[""PASSWORD""] = $arrFields[""PASSWORD""];
			        $fields[""CONFIRM_PASSWORD""] = $arrFields[""PASSWORD""];
		        }
			
		        $oUser->Update($userID, $fields);
                $res = $oUser->Login($arrUser[""EMAIL""], $newPassword);
		        if (!is_array($res)) {
			        echo ""Успешная авторизация!"";
		        } else {
			        echo ""Авторизация не удалась!:\n"";
			        print_r($res);
		        }";
            var phpExecutor = new PHPexecutor(siteUri, admin.Login, admin.Password);
            string execResult = phpExecutor.Execute(php);
            if (execResult?.Contains("Успешная " +
                "авторизация") != true)
            {
                throw new Exception($"Ошибка установки пароля для {user.Login}:( \r\n: {execResult}");
            }
        }

        static void ConfirmEmail(User userToConfirm, Uri siteUri, User admin)
        {
            DatabaseExecutor.ExecuteQuery("UPDATE b_user " +
                "SET CONFIRM_CODE = '' " +
                "WHERE LOGIN = '" + userToConfirm.Login + "'", siteUri, admin);
        }

        static string GetUserGroupIds(string userId, Uri siteUri, User userToAuth)
        {
            var groupsIDs = DatabaseExecutor.ExecuteQuery("SELECT GROUP_ID " +
                "from b_user_group " +
                "WHERE USER_ID = '" + userId + "'", siteUri, userToAuth);
            if (groupsIDs.Count == 0)
                throw new Exception("Ошибка получения групп пользователя");
            string concatIds = string.Join(",", groupsIDs.Select(x => x.GROUP_ID));
            return concatIds;
        }

        static int generatedMailCounter = 0;
        public static string GenerateMailName(string name, string lastName, string postfix = "test.com")
        {
            generatedMailCounter++;
            string n = name != default && name.Length > 3 ? name[0..4] : name;
            string ln = lastName != default && lastName.Length > 2 ? lastName[0..3] : lastName;
            return ("bx" + generatedMailCounter + Transliteration.Front(n).ToLower() + Transliteration.Front(ln).ToLower() + "-" +
                    DateTime.Now.ToString("ddMMyyyyhhmmss") + "@" + postfix);
        }

        public static string GetPassword()
        {
            return PwdGenerator(8) + ",aA1";
        }

        static int lastTicks = default;
        static string PwdGenerator(int length, int charStart = 33, int charEnd = 125)
        {
            if (lastTicks == default)
                lastTicks = new Random().Next() / 2;
            lastTicks++;
            Random rnd = new Random(lastTicks);
            string result = "";
            for (int i = 0; i < length; i++)
                result += (char)rnd.Next(charStart, charEnd);
            result = result.Replace("'",
                "%");
            result = result.Replace("%",
                "0");
            if (result.StartsWith('^'))
                result = result.Replace("^",
                    "A");
            return result;
        }

        public static User GenerateValidUserData(string namePrefix = default, string lastNamePrefix = default)
        {
            var stock = new List<string> { "Олег", "Цы" };
            var final = new List<string> { namePrefix, lastNamePrefix };
            int maxNameLength = 22;

            for (int i = 0; i < final.Count; i++)
            {
                if (final[i] == default)
                    final[i] = stock[i];
                if (final[i].Length > maxNameLength)
                    final[i] = final[i][..maxNameLength];
                else
                {
                    int saltMaxLength = maxNameLength - final[i].Length;
                    if (saltMaxLength > 0)
                        final[i] = final[i] + HelperMethodsCore.GetDateTimeSalt(true, saltMaxLength);
                }
            }

            var user = GenerateUserData(final[0], final[1]);
            return user;
        }

        static User GenerateUserData(string name, string lastName)
        {
            var user = new User();
            user.Name = name;
            user.LastName = lastName;
            user.Password = GetPassword();
            user.Login = GenerateMailName(name, lastName);
            return user;
        }
    }
}
