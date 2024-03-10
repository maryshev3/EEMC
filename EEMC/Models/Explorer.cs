using System;
using System.Collections.ObjectModel;
using System.IO;

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

        public Explorer(string Name, string NameWithPath, ContentType Type, ObservableCollection<Explorer>? Content) 
        {
            this._name = Name;
            this.NameWithPath = NameWithPath;
            this.Type = Type;
            this.Content = Content;
        }

        public void RenameCourse(string NewCourseName)
        {
            //Проверка на имя курса
            if (String.IsNullOrWhiteSpace(NewCourseName))
                throw new Exception("Название курса не содержит символов");

            string courseDirectory = Path.Combine(Environment.CurrentDirectory, "Курсы", NewCourseName);
            string oldCourseDirectory = Environment.CurrentDirectory + NameWithPath;

            if (Directory.Exists(courseDirectory))
                throw new Exception("Курс уже существует");

            Directory.Move(oldCourseDirectory, courseDirectory);

            //Будет автоматически вызван пересбор класса Course
        }

        public void RemoveCourse()
        {
            string courseDirectory = Environment.CurrentDirectory + NameWithPath;

            if (!Directory.Exists(courseDirectory))
                throw new Exception("Курс не существует");

            Directory.Delete(courseDirectory, true);

            //Будет автоматически вызван пересбор класса Course
        }
    }
}
