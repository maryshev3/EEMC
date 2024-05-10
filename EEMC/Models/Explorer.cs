using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace EEMC.Models
{
    public enum ContentType
    {
        File,
        Folder
    }

    public class Explorer
    {
        private string _name;
        public string Name 
        { 
            get => _name;
            set => _name = value;
        }

        /// <summary>
        /// Заполнено для курсов. Является ссылкой на изображение в CoursesList
        /// </summary>
        public string? ImagePath { get; set; }

        public string NameWithPath;

        public ContentType Type;

        public ObservableCollection<Explorer>? Content { get; set; }

        public ObservableCollection<Theme>? Themes { get; set; }

        public Explorer(string name, string nameWithPath, ContentType type, ObservableCollection<Explorer>? content) 
        {
            this._name = name;
            this.NameWithPath = nameWithPath;
            this.Type = type;
            this.Content = content;
        }

        private static HashSet<string> _supportedExtensions = new()
        {
            ".docx",
            ".doc",
            ".txt",
            ".cpp",
            ".h",
            ".py",
            ".cs",
            ".json",
            ".xml",
            ".html",
            ".css",
            ".ppt",
            ".pptx"
        };

        public bool IsSupportedExtension()
        {
            string extension = Path.GetExtension(Name).ToLower();

            return _supportedExtensions.Contains(extension);
        }

        public void AddFile(string filePath)
        {
            if (Type == ContentType.File)
                throw new Exception("Невозможно добавить в файл - файл");

            string fileName = Path.GetFileName(filePath);

            string currentDicrectory = Environment.CurrentDirectory + NameWithPath;
            string resultFileDirectory = Path.Combine(currentDicrectory, fileName);

            if (File.Exists(resultFileDirectory))
            {
                return;
            }

            byte[] file = File.ReadAllBytes(filePath);

            File.WriteAllBytes(resultFileDirectory, file);
        }

        public void AddFolder(string folderName)
        {
            if (Type == ContentType.File)
                throw new Exception("Невозможно добавить раздел в файл");

            string currentDicrectory = Environment.CurrentDirectory + NameWithPath;

            string resultDirectory = Path.Combine(currentDicrectory, folderName);

            if (Directory.Exists(resultDirectory))
                throw new Exception("Раздел уже существует");

            Directory.CreateDirectory(resultDirectory);
        }

        public void AddTheme(string themeName)
        {
            bool isExistingTheme = Themes?.Where(x => x.ThemeName == themeName).Any() ?? false;

            if (isExistingTheme)
            {
                throw new Exception("Производится попытка добавить существующую тему");
            }

            var allThemes = Theme.ReadAllThemes().ToList();

            allThemes.Add(
                new Theme()
                {
                    CourseName = Name,
                    ThemeName = themeName
                }
            );

            Theme.RewriteAllThemes(allThemes.ToArray());
        }

        public void Rename(string newName)
        {
            //Проверка на имя курса
            if (String.IsNullOrWhiteSpace(newName))
                throw new Exception("Пустое название");

            string oldCourseDirectory = Environment.CurrentDirectory + NameWithPath;
            string courseDirectory = Path.Combine(oldCourseDirectory, "..", newName);

            if (Directory.Exists(courseDirectory))
                throw new Exception("Раздел уже существует");

            //Переформировываем список тем (Name теперь - старое название курса) (если список тем для данного курса пуст - то нет смысла переформировывать json тем)
            if (Themes != default && Themes.Any()) 
            {
                var allThemes = Theme.ReadAllThemes();

                foreach (var theme in allThemes)
                    if (theme.CourseName == Name)
                        theme.CourseName = newName;

                Theme.RewriteAllThemes(allThemes);
            }

            //Переименовываем
            Directory.Move(oldCourseDirectory, courseDirectory);

            //Будет автоматически вызван пересбор класса Course
        }

        public void Remove()
        {
            string courseDirectory = Environment.CurrentDirectory + NameWithPath;

            if (Type == ContentType.File)
            {
                if (!File.Exists(courseDirectory))
                    throw new Exception("Файл не существует");

                File.Delete(courseDirectory);
            }
            else
            {
                if (!Directory.Exists(courseDirectory))
                    throw new Exception("Раздел не существует");

                //Переформировываем список тем (удаляем все темы, связанные с курсом) (если список тем для данного курса пуст - то нет смысла переформировывать json тем)
                if (Themes != default && Themes.Any())
                {
                    var allThemesWithoutThis = Theme.ReadAllThemes().Where(x => x.CourseName != Name).ToArray();

                    Theme.RewriteAllThemes(allThemesWithoutThis);
                }

                Directory.Delete(courseDirectory, true);
            }

            //Будет автоматически вызван пересбор класса Course
        }

        /// <summary>Возвращает список полных имён файлов курса</summary>
        public List<string>? GetAllSupportedFiles()
        {
            if (Type == ContentType.File)
                throw new Exception("Произведена попытка получения списка файлов у файла");

            if (Content == null || !Content.Any())
                return null;

            List<string> result = new List<string>();

            foreach (var content in Content)
            {
                if (content == null)
                    continue;

                if (content.Type == ContentType.File && content.IsSupportedExtension())
                {
                    result.Add(content.NameWithPath);

                    continue;
                }

                if (content.Type == ContentType.Folder)
                {
                    var filesForThis = content.GetAllSupportedFiles();

                    if (filesForThis == null || !filesForThis.Any())
                        continue;

                    result.AddRange(filesForThis);
                }
            }

            return result.Any() ? result : null;
        }
    }
}
