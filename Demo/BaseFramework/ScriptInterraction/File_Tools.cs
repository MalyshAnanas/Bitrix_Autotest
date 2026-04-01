using System;
using Demo.BaseFramework.ScriptInterraction;
using Demo.TestEntities;

namespace Demo.BaseFramework
{
    public static class File_Tools
    {
        /// <summary>
        /// Создаёт файл в "Общем диске" через PHP на сервере портала.
        /// </summary>
        /// <param name="admin">Админ для PHP выполнения</param>
        /// <param name="siteUri">Адрес портала</param>
        /// <param name="fileName">Имя файла для удаления</param>
        public static int CreateFileInCommonFolder(
            User admin,
            Uri siteUri,
            string fileName
            )
        {
            // Формируем PHP код
            string phpCode = $@"
            use Bitrix\Main\Loader;
            use Bitrix\Disk\Storage;
            use Bitrix\Disk\Internals\StorageTable;
            use Bitrix\Disk\Folder;

            Loader::includeModule('disk');

            $userId = 1; // можно оставить админа

            // Ищем ""Общий диск"" по типу
            $storageData = StorageTable::getList([
                'filter' => [
                    '=ENTITY_TYPE' => 'Bitrix\Disk\ProxyType\Common'
                ]
            ])->fetch();

            if (!$storageData) {{
                die('Общий диск не найден');
            }}

            // Загружаем хранилище
            $storage = Storage::loadById($storageData['ID']);

            if (!$storage) {{
                die('Ошибка загрузки хранилища');
            }}

            // Получаем ID корневой папки
            $rootFolder = $storage->getRootObject();
            $rootFolderId = $rootFolder->getId();

            // Теперь работаем уже через ID папки
            $folder = Folder::loadById($rootFolderId);

            // Создание файла
            $fileContent = ""Файл создаётся независимо от пользователя"";
            $tmpFilePath = $_SERVER[""DOCUMENT_ROOT""] . ""/upload/tmp_test.txt"";
            file_put_contents($tmpFilePath, $fileContent);

            $fileArray = \CFile::MakeFileArray($tmpFilePath);

            $newFile = $folder->uploadFile(
                $fileArray,
                [
                    'NAME' => '{fileName}' . time() . '.txt',
                    'CREATED_BY' => 1
                ]
            );

            if ($newFile) {{
                echo $newFile->getId();
            }} else {{
                echo ""Ошибка создания файла"";
            }}
            ";
            var phpExecutor = new PHPexecutor(siteUri, admin.Login, admin.Password);
            string result = phpExecutor.Execute(phpCode)?.Trim();

            if (!int.TryParse(result, out int execResult))
            {
                throw new Exception($"PHP returned invalid int value. Raw result: {result}");
            }

            return execResult;
        }
        
        /// <summary>
        /// Отправляет файл в корзину из "Общего диска" через PHP на сервере портала.
        /// </summary>
        /// <param name="admin">Админ для PHP выполнения</param>
        /// <param name="siteUri">Адрес портала</param>
        /// <param name="idFile">ID файла для удаления</param>
        public static void DeleteFileInCommonFolder(
            User admin,
            Uri siteUri,
            int idFile
            )
        {
            // Формируем PHP код
            string phpCode = $@"
            use Bitrix\Main\Loader;
            use Bitrix\Disk\File;

            Loader::includeModule('disk');

            $fileId = {idFile};

            $file = File::loadById($fileId);

            if ($file)
            {{
                $file->markDeleted(1); // 1 — ID пользователя (например админ)
                echo 'File moved to trash';
            }}
            else
            {{
                echo 'File not found';
            }}
            ";

            new PHPexecutor(siteUri, admin.Login, admin.Password).Execute(phpCode);
            
        }
    }
}