using Aspose.Zip.Saving;
using Aspose.Zip;
using EEMC.Models;
using EEMC.ToXPSConverteres;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps.Packaging;

namespace EEMC.Services
{
    public class ImportExportService
    {
        private readonly ConverterUtils _converterUtils;

        private readonly Theme[] _allThemes;
        private readonly Course _allCourses;

        public ImportExportService(
            ConverterUtils converterUtils,
            Theme[] allThemes,
            Course allCourses
        ) 
        {
            _converterUtils = converterUtils;

            _allThemes = allThemes;
            _allCourses = allCourses;
        }

        private Task<XpsDocument> CreateTaskOnConvert(string originNameWithPath, bool isTheme)
        {
            //Определяем дирректирию сохранения файла темы
            string savePathOriginFile = isTheme
                ? originNameWithPath.Replace("Файлы тем", "Файлы тем конвертированные")
                : originNameWithPath.Replace("Курсы", "Курсы конвертированные");

            string savePathXpsFile = Environment.CurrentDirectory + savePathOriginFile.Substring(0, savePathOriginFile.LastIndexOf('.')) + ".xps";

            //Создаём дирректорию для сохранения файла
            FileInfo fileInfo = new FileInfo(savePathXpsFile);
            fileInfo.Directory.Create();

            //Конвертируем
            string fileExt = Path.GetExtension(originNameWithPath);

            return _converterUtils
                .GetInstanceByFileExtension(fileExt)
                .ToXpsConvertAsync(
                    Environment.CurrentDirectory + originNameWithPath,
                    savePathXpsFile
                );
        }

        private void AddIfExist(List<string> savedPathes, string path)
        {
            if (Directory.Exists(path) || File.Exists(path))
                savedPathes.Add(path);
        }

        private List<string> CreatePathesForSave()
        {
            List<string> savedPathes = new();

            savedPathes.Add(Path.Combine(Environment.CurrentDirectory, "Курсы"));

            AddIfExist(savedPathes, Path.Combine(Environment.CurrentDirectory, "Курсы конвертированные"));
            AddIfExist(savedPathes, Path.Combine(Environment.CurrentDirectory, "Файлы тем"));
            AddIfExist(savedPathes, Path.Combine(Environment.CurrentDirectory, "Файлы тем конвертированные"));
            AddIfExist(savedPathes, Path.Combine(Environment.CurrentDirectory, "themes.json"));
            AddIfExist(savedPathes, Path.Combine(Environment.CurrentDirectory, "templates.json"));
            AddIfExist(savedPathes, Path.Combine(Environment.CurrentDirectory, "versions.json"));

            return savedPathes;
        }

        public async Task Export(string description = "Описание отсутствует")
        {
            if (_allCourses == null || _allCourses.Courses == default || !_allCourses.Courses.Any())
                throw new Exception("Версия экспорта не содержит никаких данных");

            //Создаём сервисную информацию (дата создания, ид версии, описание (параметр метода))
            DateTimeOffset exportDate = DateTimeOffset.Now;
            Guid idVersion = Guid.NewGuid();

            string versionsJsonPath = Path.Combine(Environment.CurrentDirectory, "versions.json");
            List<Models.Version> versions;

            if (File.Exists(versionsJsonPath))
            {
                string json = File.ReadAllText(versionsJsonPath);

                versions = JsonConvert.DeserializeObject<List<Models.Version>>(json);
                versions.Add(
                    new Models.Version() {
                        Id = idVersion,
                        CreatedDate = exportDate,
                        Description = description
                    }
                );
            }
            else
                versions = new() {
                    new Models.Version()
                    {
                        Id = idVersion,
                        CreatedDate = exportDate,
                        Description = description
                    }
                };

            string serJson = JsonConvert.SerializeObject(versions);

            File.WriteAllText(versionsJsonPath, serJson);

            //Проходимся по всем темам. Файлы тем, которые можем конвертировать - конвертируем
            Stack<Task<XpsDocument>> tasks = new();

            foreach (var theme in _allThemes)
                foreach (var file in theme.Files)
                    if (file.IsSupportedExtension() && !file.IsVideoOrAudio() && !file.IsImage())
                        tasks.Push(CreateTaskOnConvert(file.NameWithPath, true));

            //Проходимся по всем курсам. Файлы курсов, которые можем конвертировать - конвертируем
            foreach (var course in _allCourses.Courses)
            {
                //Получаем все файлы курса
                var filePathes = course.GetAllFiles();

                if (filePathes == null)
                    continue;

                foreach (var filePath in filePathes)
                    tasks.Push(CreateTaskOnConvert(filePath, false));
            }

            //Дожидаемся выполнения всех задач на конвертацию
            foreach (var task in tasks)
            {
                XpsDocument doc = await task;

                //Метод конвертации возвращает открытый документ (его надо закрыть)
                doc.Close();
            }

            //Создаём архив версии
            List<string> savedPathes = CreatePathesForSave();

            using (FileStream zipFile = File.Open(Path.Combine(Environment.CurrentDirectory, $"{idVersion}.ce"), FileMode.Create))
                using (Archive archive = new Archive(new ArchiveEntrySettings(new StoreCompressionSettings())))
                {
                    foreach (string savedPath in savedPathes)
                    {
                        string fileName = savedPath.Substring(savedPath.LastIndexOf('\\') + 1);

                        archive.CreateEntry(fileName, savedPath);
                    }

                    archive.Save(zipFile, new ArchiveSaveOptions() { Encoding = Encoding.ASCII });
                }
        }
    }
}
