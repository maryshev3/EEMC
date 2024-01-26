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
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Explorer>? _courses;
        private static FileSystemWatcher? _watcher = null;

        public Course() 
        {
            _courses = CourseBuilder.Build(new ExplorerBuilder())._courses;

            if (_watcher == null)
            {
                _watcher = new FileSystemWatcher(Path.Combine(Environment.CurrentDirectory, "Курсы"))
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

            Courses = CourseBuilder.Build(new ExplorerBuilder())._courses;
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

        public ObservableCollection<Explorer>? GetCourseContent(string CourseName) 
        {
            return Courses.First(x => x.Name == CourseName).Content;
        }
    }
}
