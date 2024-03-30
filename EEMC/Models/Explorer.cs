using Newtonsoft.Json;
using System;
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

        private Theme[] ReadAllThemes()
        {
            string json = File.ReadAllText("./themes.json");

            var themes = JsonConvert.DeserializeObject<Theme[]>(json);

            return themes;
        }

        private void RewriteAllThemes(Theme[] themes)
        {
            string json = JsonConvert.SerializeObject(themes);

            File.WriteAllText("./themes.json", json);
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
                var allThemes = ReadAllThemes();

                foreach (var theme in allThemes)
                    if (theme.ThemeName == Name)
                        theme.ThemeName = newName;

                RewriteAllThemes(allThemes);
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
                    var allThemesWithoutThis = ReadAllThemes().Where(x => x.ThemeName != Name).ToArray();

                    RewriteAllThemes(allThemesWithoutThis);
                }

                Directory.Delete(courseDirectory, true);
            }

            //Будет автоматически вызван пересбор класса Course
        }
    }
}
