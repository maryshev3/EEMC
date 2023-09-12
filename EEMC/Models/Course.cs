using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public class Course : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Explorer>? _courses;

        public Course(ObservableCollection<Explorer>? Courses) 
        {
            _courses = Courses;

            var watcher = new FileSystemWatcher(Path.Combine(Environment.CurrentDirectory, "Курсы"))
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true
            };

            watcher.Changed += OnDirectoryChanged;
            watcher.Created += OnDirectoryChanged;
            watcher.Deleted += OnDirectoryChanged;
            watcher.Renamed += OnDirectoryChanged;
        }

        private void OnDirectoryChanged(object sender, FileSystemEventArgs e) 
        {
            _courses = CourseBuilder.Build(new ExplorerBuilder())._courses;
            OnPropertyChanged("Courses");
        }

        public IEnumerable<Explorer> Courses 
        {
            get => _courses;
        }
    }
}
