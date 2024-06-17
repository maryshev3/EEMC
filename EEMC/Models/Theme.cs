using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EEMC.Models
{
    public class Theme
    {
        [JsonIgnore]
        public Visibility UpVisibility { get => ThemeNumber != 1 ? Visibility.Visible : Visibility.Collapsed; }
        [JsonIgnore]
        public Visibility DownVisibility { get => ThemeNumber != ReadAllThemes().Where(x => x.CourseName == CourseName).Count() ? Visibility.Visible : Visibility.Collapsed; }
        public string ThemeName { get; set; }
        [JsonIgnore]
        public string NameWithNumber { get => $"{ThemeNumber}. {ThemeName}"; }
        public string ThemeDescription { get; set; }
        public int ThemeNumber { get; set; }
        public ObservableCollection<ThemeFile>? Files { get; set; }
        public Boolean IsHiden { get; set; }
        //Воспринимать как внешний ключ к Course
        public string CourseName { get; set; }

        public static Theme[] ReadAllThemes()
        {
            string json = File.ReadAllText("./themes.json");

            var themes = JsonConvert.DeserializeObject<Theme[]>(json);

            return themes;
        }

        public static void RewriteAllThemes(Theme[] themes)
        {
            string json = JsonConvert.SerializeObject(themes.OrderBy(x => x.ThemeNumber));

            File.WriteAllText("./themes.json", json);

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

        public void RenameTheme(string newThemeName)
        {
            var allThemes = Theme.ReadAllThemes();

            bool isExisting = allThemes.Where(x => x.ThemeName == newThemeName && x.CourseName == CourseName).Any();

            if (isExisting)
            {
                throw new Exception("Новое имя темы не может совпадать с уже существующей");
            }

            allThemes.First(x => x.ThemeName == ThemeName && x.CourseName == CourseName).ThemeName = newThemeName;

            Theme.RewriteAllThemes(allThemes);
        }

        public void ChangeDescription(string newDescription)
        {
            if (ThemeDescription == newDescription)
                return;

            var allThemes = Theme.ReadAllThemes();

            allThemes.First(x => x.ThemeName == ThemeName && x.CourseName == CourseName).ThemeDescription = newDescription;

            Theme.RewriteAllThemes(allThemes);
        }

        public void RemoveTheme()
        {
            var allThemes = Theme.ReadAllThemes();

            var allThemesFiltered = allThemes.Where(x => x.ThemeName != ThemeName || x.CourseName != CourseName).ToArray();
            var courseThemes = allThemesFiltered.Where(x => x.CourseName == CourseName && x.ThemeNumber > ThemeNumber);

            //Удаляем эту тему из всех итоговых тестов
            foreach (var courseTheme in courseThemes )
            {
                if (courseTheme.Files == null || !courseTheme.Files.Any())
                    continue;

                var files = courseTheme.Files.Where(x => x.IsTotalTest());

                foreach (var file in files)
                {
                    //Перезаписываем файл с итоговым тестом
                    string filePath = Environment.CurrentDirectory + file.NameWithPath;

                    string json = File.ReadAllText(filePath);

                    var tests = JsonConvert.DeserializeObject<TotalTestItem[]>(json);

                    var filteredTests = tests.Where(x => x.ThemeName != ThemeName);

                    if (filteredTests.Count() == tests.Count())
                        continue;

                    string result = JsonConvert.SerializeObject(filteredTests);

                    File.WriteAllText(filePath, result);
                }
            }

            //Уменьшаем номера
            foreach (var theme in courseThemes)
            {
                theme.ThemeNumber--;
            }

            Theme.RewriteAllThemes(allThemesFiltered);
        }

        public void ChangeHidenMode()
        {
            var allThemes = Theme.ReadAllThemes();

            allThemes.First(x => x.ThemeName == ThemeName && x.CourseName == CourseName).IsHiden = !IsHiden;

            Theme.RewriteAllThemes(allThemes);
        }

        public void AddFile(string path)
        {
            var file = File.ReadAllBytes(path);

            string savePath = Path.Combine(
                Environment.CurrentDirectory,
                "Файлы тем",
                CourseName,
                ThemeName,
                Path.GetFileName(path)
            );

            FileInfo fileInfo = new FileInfo(savePath);
            fileInfo.Directory.Create();

            File.WriteAllBytes(savePath, file);

            var allThemes = Theme.ReadAllThemes();
            var thisTheme = allThemes.First(x => x.CourseName == CourseName && x.ThemeName == ThemeName);

            if (thisTheme.Files == default)
                thisTheme.Files = new();
            else
            {
                if (thisTheme.Files.Select(x => x.Name).ToHashSet().Contains(Path.GetFileName(savePath)))
                {
                    throw new Exception("Добавляемый файл уже есть в теме");
                }
            }

            thisTheme.Files.Add(
                new ThemeFile()
                {
                    Name = Path.GetFileName(savePath),
                    NameWithPath = savePath.Substring(Environment.CurrentDirectory.Length)
                }
            );

            Theme.RewriteAllThemes(allThemes);
        }
    }
}
