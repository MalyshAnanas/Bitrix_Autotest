using Demo.BaseFramework.ScriptInterraction;
using Demo.BaseFramework.LogTools;

namespace Demo.TestEntities
{
    public class BxPortalGroup
    {
        public static BxPortalGroup Freelancee
        {
            get => new BxPortalGroup
            {
                GroupName = "Фри" +
                "ланс",
                IsExternal = true,
            };
        }

        public string GroupName { get; set; }
        public bool IsExternal { get; set; }

        string GetIdByName(Uri portalUri, User portalAdmin)
        {
            var result = DatabaseExecutor.ExecuteQuery("SELECT ID from b_s" +
                "onet_group " +
                "WHERE NAME = '" + GroupName + "'", portalUri, portalAdmin);
            return result.Count == 0 ? null 
                : result[0].ID;
        }

        public void AddUserToGroupByScript(
            User user,
            Uri portalUri,
            User portalAdmin)
        {
            Log.Info("Добавляем юзераы " + user.Login + " в группу " + GroupName + " скриптом");
            string groupId = CreateGroupIfNotPresent(portalUri, portalAdmin);
            string userId = user.GetDBid(portalUri, portalAdmin);
            if (string.IsNullOrEmpty(userId))
                throw new Exception($"Юзер {user.Login} не добавился на портал");
            AddUserToGroup(userId, groupId, 
                portalUri, portalAdmin);
        }

        /// <summary>
        /// Создаёт группу, если её там нет
        /// </summary>
        /// <param name="portalUri"></param>
        /// <param name="portalAdmin"></param>
        /// <returns>Id группы</returns>
        public string CreateGroupIfNotPresent(Uri portalUri, User portalAdmin)
        {
            string groupId = GetIdByName(portalUri, portalAdmin);

            if (string.IsNullOrEmpty(groupId))
            {
                string php =
                    $"$arFields[\"NAME\"] ='{GroupName}';\r\n" +
                    $"$arFields[\"SITE_ID\"] ='{(IsExternal ? "co" : "s1")}';\r\n" +
                    $"$arFields[\"OPENED\"] ='Y';\r\n" +
                    $"$arFields[\"VISIBLE\"] ='Y';\r\n" +
                    $"$arFields[\"PROJECT\"] ='N';\r\n";

                php += @"
                    global $USER;
	                CModule::IncludeModule('socialnetwork');
	                $arFields[""SUBJECT_ID""] = 1;
	                $arFields[""OWNER_ID""] = $USER->GetID();
	                $arFields[""NUMBER_OF_MEMBERS""] = 1;
	                $arFields[""NUMBER_OF_MODERATORS""] = 1;
	                $arFields[""INITIATE_PERMS""] = ""K"";
	                $arFields[""SPAM_PERMS""] = ""K"";
	                $arFields[""CLOSED""] = ""N"";

	                $id = CSocNetGroup::CreateGroup(
                        $arFields[""OWNER_ID""], 
                        $arFields, 
                        true
                    );
	                CSocNetFeatures::SetFeature(
		                SONET_ENTITY_GROUP,
		                $id,
		                'files',
		                true,
		                false
	                );
                    if($id > 0){
	                    echo ""<br/> - Добавлена  группа: <b>"" . $arFields['NAME'] . ""</b>"";
                    }";
                var executor = new PHPexecutor(portalUri, portalAdmin.Login, portalAdmin.Password);
                string res = executor.Execute(php);
                if(res?.Contains("Добавлена  группа") != true)
                    throw new Exception($"'{GroupName}' не создалась:\r\n{res}");
                groupId = GetIdByName(portalUri, portalAdmin);
            }

            return groupId;
        }

        static void AddUserToGroup(string userId, string groupId, Uri portalUri, User portalAdmin)
        {
            string php =
                $"$arFields[\"USER_ID\"]  = '{userId}';\r\n" +
                $"$arFields[\"GROUP_ID\"]  = '{groupId}';\r\n" +
                $"$arFields[\"ROLE\"]  = 'K';\r\n" +
                "$arFields[\"=DATE_CREATE\"]  = $GLOBALS[\"DB\"]->CurrentTimeFunction();\r\n" +
                "$arFields[\"=DATE_UPDATE\"]  = $GLOBALS[\"DB\"]->CurrentTimeFunction();\r\n" +
                $"$arFields[\"INITIATED_BY_TYPE\"]  = 'G';\r\n" +
                "$arFields[\"INITIATED_BY_USER_ID\"]  = $USER->GetID();\r\n" +
                $"$arFields[\"MESSAGE\"]  = false;\r\n";

            php += @"
                CModule::IncludeModule(""socialnetwork"");
                $ID = CSocNetUserToGroup::Add($arFields);

		        if ($ID == true) {
			        echo ""Пользователь с ID = "";
			        echo $arFields[""USER_ID""];
			        echo "" успешно  добавлен в группу с ID = "";
			        echo $arFields[""GROUP_ID""];
		        } else {
			        echo ""Добавление  пользователя в группу не сработало"";
		        }";

            var phpExecutor = new PHPexecutor(portalUri, portalAdmin.Login, portalAdmin.Password);
            string res = phpExecutor.Execute(php);

            if (res?.Contains("успешно  добавлен в группу") == true)
                Log.Info("Юзер " + userId + " добавлен в группу " + groupId);
            else
                throw new Exception($"Юзер с ID='{userId}' не добавлен в группу с ID='{groupId}':\r\n{res}");
        }
    }
}
