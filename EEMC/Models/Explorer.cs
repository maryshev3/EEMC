﻿using Microsoft.VisualBasic.ApplicationServices;
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

        public string NameWithoutExtension
        {
            get => Path.GetFileNameWithoutExtension(Name);
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

        public bool IsText()
        {
            string extension = Path.GetExtension(Name).ToLower();

            return extension is ".docx" or ".doc" or ".txt" or ".cpp" or ".h" or ".py"
                or ".cs" or ".json" or ".xml" or ".html" or ".css" or ".ppt" or ".pptx";
        }

        public bool IsPdf()
        {
            string extension = Path.GetExtension(Name).ToLower();

            return extension == ".pdf";
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
            int? lastNum = allThemes.Where(x => x.CourseName == Name)?.MaxBy(x => x.ThemeNumber)?.ThemeNumber;

            allThemes.Add(
                new Theme()
                {
                    CourseName = Name,
                    ThemeName = themeName,
                    ThemeNumber = lastNum == null ? 1 : lastNum.Value + 1
                }
            );

            Theme.RewriteAllThemes(allThemes.ToArray());
        }

        public bool IsEnabledDown(Theme theme)
        {
            return theme.ThemeNumber < Themes.Count;
        }

        public bool IsEnabledUp(Theme theme)
        {
            return theme.ThemeNumber > 1;
        }

        /// <summary>
        /// Повышает номер вопроса в общем списке (понижает в визуальном плане)
        /// </summary>
        public void Down(Theme theme)
        {
            if (!IsEnabledDown(theme))
                throw new Exception("Невозможно поднять вверх по списку");

            var allThemes = Theme.ReadAllThemes().ToList();

            Theme upper = allThemes.Where(x => x.CourseName == theme.CourseName).First(x => x.ThemeNumber == theme.ThemeNumber + 1);
            Theme thisTheme = allThemes.Where(x => x.CourseName == theme.CourseName && x.ThemeName == theme.ThemeName).First();

            //В upper могли быть итоговые тесты, которые опирались на thisTheme. Надо удалить из итоговых тестов привязки к тестам thisTheme
            if (upper.Files != null && upper.Files.Any())
            {
                var files = upper.Files.Where(x => x.IsTotalTest());

                foreach (var file in files)
                {
                    //Перезаписываем файл с итоговым тестом
                    string filePath = Environment.CurrentDirectory + file.NameWithPath;

                    string json = File.ReadAllText(filePath);

                    var tests = JsonConvert.DeserializeObject<TotalTestItem[]>(json);

                    var filteredTests = tests.Where(x => x.ThemeName != thisTheme.ThemeName);

                    if (filteredTests.Count() == tests.Count())
                        continue;

                    string result = JsonConvert.SerializeObject(filteredTests);

                    File.WriteAllText(filePath, result);
                }
            }

            upper.ThemeNumber--;
            thisTheme.ThemeNumber++;

            Theme.RewriteAllThemes(allThemes.ToArray());
        }

        /// <summary>
        /// Понижает номер вопроса в общем списке (повышает в визуальном плане)
        /// </summary>
        public void Up(Theme theme)
        {
            if (!IsEnabledUp(theme))
                throw new Exception("Невозможно спустить вниз по списку");

            var allThemes = Theme.ReadAllThemes().ToList();

            Theme downer = allThemes.Where(x => x.CourseName == theme.CourseName).First(x => x.ThemeNumber == theme.ThemeNumber - 1);
            Theme thisTheme = allThemes.Where(x => x.CourseName == theme.CourseName && x.ThemeName == theme.ThemeName).First();

            //В thisTheme могли быть итоговые тесты, которые опирались на downer. Надо удалить из итоговых тестов привязки к тестам downer
            if (thisTheme.Files != null && thisTheme.Files.Any())
            {
                var files = thisTheme.Files.Where(x => x.IsTotalTest());

                foreach (var file in files)
                {
                    //Перезаписываем файл с итоговым тестом
                    string filePath = Environment.CurrentDirectory + file.NameWithPath;

                    string json = File.ReadAllText(filePath);

                    var tests = JsonConvert.DeserializeObject<TotalTestItem[]>(json);

                    var filteredTests = tests.Where(x => x.ThemeName != downer.ThemeName);

                    if (filteredTests.Count() == tests.Count())
                        continue;

                    string result = JsonConvert.SerializeObject(filteredTests);

                    File.WriteAllText(filePath, result);
                }
            }

            downer.ThemeNumber++;
            thisTheme.ThemeNumber--;

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

                if (content.Type == ContentType.File && content.IsText())
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
