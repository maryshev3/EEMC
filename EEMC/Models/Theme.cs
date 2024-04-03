﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public class Theme
    {
        public string ThemeName { get; set; }
        public string ThemeDescription { get; set; }
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
            string json = JsonConvert.SerializeObject(themes);

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

            Theme.RewriteAllThemes(allThemesFiltered);
        }

        public void ChangeHidenMode()
        {
            var allThemes = Theme.ReadAllThemes();

            allThemes.First(x => x.ThemeName == ThemeName && x.CourseName == CourseName).IsHiden = !IsHiden;

            Theme.RewriteAllThemes(allThemes);
        }
    }
}
