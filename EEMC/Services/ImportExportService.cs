using EEMC.Models;
using EEMC.ToXPSConverteres;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps.Packaging;
using Ionic.Zip;
using System.Reflection;

namespace EEMC.Services
{
    public class ImportExportService
    {
        private readonly ConverterUtils _converterUtils;

        private readonly Theme[] _themes;
        private readonly List<Explorer> _courses;
        private readonly CourseImage[] _courseImages;

        public ImportExportService(
            ConverterUtils converterUtils,
            Theme[] themes,
            List<Explorer> courses,
            CourseImage[] courseImages
        ) 
        {
            _converterUtils = converterUtils;

            _themes = themes;
            _courses = courses;
            _courseImages = courseImages;
        }

        private Task<XpsDocument> CreateTaskOnConvert(string originNameWithPath, bool isTheme)
        {
            //Определяем дирректирию сохранения файла темы
            string savePathOriginFile = isTheme
                ? originNameWithPath.Replace("Файлы тем", "Файлы тем конвертированные")
                : originNameWithPath.Replace("Курсы", "Курсы конвертированные");

            string savePathXpsFile = Environment.CurrentDirectory + savePathOriginFile + ".xps";

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

            foreach (var course in _courses)
                AddIfExist(savedPathes, Path.Combine(Environment.CurrentDirectory, "Курсы", course.Name));

            AddIfExist(savedPathes, Path.Combine(Environment.CurrentDirectory, "Курсы конвертированные"));

            foreach (var theme in _themes.DistinctBy(x => x.CourseName))
                AddIfExist(savedPathes, Path.Combine(Environment.CurrentDirectory, "Файлы тем", theme.CourseName));

            AddIfExist(savedPathes, Path.Combine(Environment.CurrentDirectory, "Файлы тем конвертированные"));

            AddIfExist(savedPathes, Path.Combine(Environment.CurrentDirectory, "themes.json"));

            AddIfExist(savedPathes, Path.Combine(Environment.CurrentDirectory, "course_img.json"));

            return savedPathes;
        }

        private static void RemoveConvertedPathes()
        {
            if (Directory.Exists("./Файлы тем конвертированные"))
                Directory.Delete("./Файлы тем конвертированные", true);

            if (Directory.Exists("./Курсы конвертированные"))
                Directory.Delete("./Курсы конвертированные", true);
        }

        /// <summary>
        /// Возвращает id версии, обновляет файл версий
        /// </summary>
        private Guid UpdateVersionsFile(string versionName, string savedFolder)
        {
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
                    new Models.Version()
                    {
                        Id = idVersion,
                        CreatedDate = exportDate,
                        VersionName = versionName,
                        SavedFolder = savedFolder
                    }
                );
            }
            else
                versions = new() {
                    new Models.Version()
                    {
                        Id = idVersion,
                        CreatedDate = exportDate,
                        VersionName = versionName,
                        SavedFolder = savedFolder
                    }
                };

            string serJson = JsonConvert.SerializeObject(versions);

            File.WriteAllText(versionsJsonPath, serJson);

            return idVersion;
        }
        
        private async Task ExportCourses(string versionName, string savedFolder)
        {
            if (_courses == default || !_courses.Any())
                throw new Exception("Версия экспорта не содержит никаких данных");

            //Удаляем старые конвертированные файлы
            RemoveConvertedPathes();

            Guid idVersion = UpdateVersionsFile(versionName, savedFolder);

            //Проходимся по всем темам. Файлы тем, которые можем конвертировать - конвертируем
            Stack<Task<XpsDocument>> tasks = new();

            foreach (var theme in _themes)
            {
                if (theme.Files == null)
                    continue;

                foreach (var file in theme.Files)
                    if (file.IsSupportedExtension() && !file.IsVideoOrAudio() && !file.IsImage())
                        tasks.Push(CreateTaskOnConvert(file.NameWithPath, true));
            }

            //Проходимся по всем курсам. Файлы курсов, которые можем конвертировать - конвертируем
            foreach (var course in _courses)
            {
                //Получаем все файлы курса
                var filePathes = course.GetAllSupportedFiles()?.Where(x => !x.Contains("~$"));

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

            //Переформировываем themes.json для загрузки в архив
            string themesJsonPath = Path.Combine(Environment.CurrentDirectory, "themes.json");
            string tmpThemesJsonPath = Path.Combine(Environment.CurrentDirectory, "themes_tmp.json");
            if (File.Exists(themesJsonPath))
            {
                //Временно переименовываем themes.json в themes_tmp.json
                File.Move(themesJsonPath, tmpThemesJsonPath);

                //Формируем themes.json сохранённые
                string json = JsonConvert.SerializeObject(_themes);
                File.WriteAllText(themesJsonPath, json);
            }

            //Переформировываем course_img.json для загрузки в архив
            string courseImgPath = Path.Combine(Environment.CurrentDirectory, "course_img.json");
            string tmpCourseImgPath = Path.Combine(Environment.CurrentDirectory, "course_img_tmp.json");
            if (File.Exists(courseImgPath))
            {
                File.Move(courseImgPath, tmpCourseImgPath);

                string json = JsonConvert.SerializeObject(_courseImages);
                File.WriteAllText(courseImgPath, json);
            }

            //Создаём архив версии
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            List<string> savedPathes = CreatePathesForSave();

            using (ZipFile zip = new ZipFile(Encoding.UTF8))
            {
                foreach (var dir in savedPathes.Where(x => Directory.Exists(x)))
                {
                    var dirCataloges = dir.Split('\\');

                    zip.AddDirectory(
                        dir,
                        dirCataloges.Last().Contains("конвертированные") 
                            ? dirCataloges.Last() 
                            : new StringBuilder().Append(dirCataloges[dirCataloges.Length - 2]).Append('\\').Append(dirCataloges.Last()).ToString()
                    );
                }

                zip.AddFiles(savedPathes.Where(x => File.Exists(x)), "");

                StringBuilder savePath = new StringBuilder();

                savePath.Append(savedFolder);
                savePath.Append($"\\{idVersion}.ce");

                zip.Save(savePath.ToString());
            }

            if (File.Exists(themesJsonPath))
            {
                //Удаляем текущий themes.json и переименовываем временный обратно
                File.Delete(themesJsonPath);
                File.Move(tmpThemesJsonPath, themesJsonPath);
            }

            if (File.Exists(courseImgPath))
            {
                //Удаляем текущий themes.json и переименовываем временный обратно
                File.Delete(courseImgPath);
                File.Move(tmpCourseImgPath, courseImgPath);
            }
        }

        /// <summary>
        /// Экспортирует выбранные курсы в папку savedFolder вместе с программой-просмотрщкиком
        /// </summary>
        /// <param name="versionName">Название версии экспорта</param>
        /// <param name="savedFolder">Папка для сохранения программы-просмотрщика (уже вместе с названием версии)</param>
        /// <returns></returns>
        public async Task Export(string versionName, string savedFolder)
        {
            //Создаём папку, куда будет экспортирована версия-просмотрщик
            if (Directory.Exists(savedFolder))
            {
                throw new Exception("Папка для создания версии уже существует (удалите её или переименуйте");
            }

            Directory.CreateDirectory(savedFolder);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var assembly = Assembly.GetExecutingAssembly();
            var a = assembly.GetManifestResourceNames();
            var resourceName = "EEMC.Resources.viewer.zip";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (ZipFile zip = ZipFile.Read(stream, options: new ReadOptions() { Encoding = Encoding.GetEncoding(866) }))
                {
                    zip.ExtractAll(savedFolder, ExtractExistingFileAction.Throw);
                }
            }

            //Создаём версию курсов
            await ExportCourses(versionName, savedFolder);
        }

        public static void Import(string cePath, Course courses)
        {
            if (!File.Exists(cePath))
                throw new Exception("Не удаётся найти переданный файл с курсами");

            if (Path.GetExtension(cePath) != ".ce")
                throw new Exception("Переданный файл имеет неверный формат");

            //Удаляем старые курсы и темы
            foreach (var course in courses.Courses)
                course.Remove();

            RemoveConvertedPathes();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (ZipFile zip = ZipFile.Read(cePath, options: new ReadOptions() { Encoding = Encoding.UTF8 }))
            {
                zip.ExtractAll(Environment.CurrentDirectory, ExtractExistingFileAction.OverwriteSilently);
            }

            //Иммитируем пересбор курсов
            string tmpDir = Path.Combine(Environment.CurrentDirectory, "Курсы", "tmpf");

            if (!Directory.Exists(tmpDir))
            {
                Directory.CreateDirectory(tmpDir);
                Directory.Delete(tmpDir);
            }
            else
            {
                Directory.Delete(tmpDir, true);
            }
        }
    }
}
