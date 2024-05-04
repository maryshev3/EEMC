using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace EEMC.Models
{
    public class Course: INotifyPropertyChanged
    {
        private static Templates _templates;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Explorer>? _courses;
        private static FileSystemWatcher? _watcher = null;

        public Course(Templates templates)
        {
            if (_templates == default)
                _templates = templates;

            _courses = CourseBuilder.Build(new ExplorerBuilder())._courses;

            if (_watcher == null)
            {
                string courseDirectory = Path.Combine(Environment.CurrentDirectory, "Курсы");

                //Если директории курсов не существует - создаём
                if (!Directory.Exists(courseDirectory))
                    Directory.CreateDirectory(courseDirectory);

                _watcher = new FileSystemWatcher(courseDirectory)
                {
                    EnableRaisingEvents = true,
                    IncludeSubdirectories = true,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName
                };

                AddWatcherHandler(OnDirectoryChanged);
            }
        }

        public void AddWatcherHandler(FileSystemEventHandler handler)
        {
            if (_watcher != null)
            {
                //_watcher.Changed += handler;
                _watcher.Created += handler;
                _watcher.Deleted += handler;
                _watcher.Renamed += new RenamedEventHandler(handler);
            }
        }

        private void OnDirectoryChanged(object sender, FileSystemEventArgs e)
        {
            string fileExt = Path.GetExtension(e.Name);

            if (e.Name.Contains("~$"))
                return;

            //bool wereBuilded = false;
            //while (!wereBuilded)
            //{
            //    try
            //    {
                    Courses = CourseBuilder.Build(new ExplorerBuilder())._courses;
            //    }
            //    catch 
            //    {
            //        continue;
            //    }

            //    wereBuilded = true;
            //}
        }

        public Course(ObservableCollection<Explorer>? Courses) 
        {
            _courses = Courses;
        }

        public ObservableCollection<Explorer> Courses 
        {
            get => _courses;
            set
            {
                _courses = value;
                OnPropertyChanged("Courses");
            }
        }

        public void CreateCourse(string CourseName)
        {
            //Проверка на имя курса
            if (String.IsNullOrWhiteSpace(CourseName))
                throw new Exception("Название курса не содержит символов");

            string courseDirectory = Path.Combine(Environment.CurrentDirectory, "Курсы", CourseName);

            //Если директории курсов не существует - создаём
            if (!Directory.Exists(courseDirectory))
            {
                //Создаём курс
                Directory.CreateDirectory(courseDirectory);

                //Реализуем шаблон
                foreach (var template in _templates.TemplatesList)
                {
                    string templateDirectory = Path.Combine(courseDirectory, template);

                    Directory.CreateDirectory(templateDirectory);
                }
            }
            else
            {
                throw new Exception("Курс уже существует");
            }
        }
    }
}
