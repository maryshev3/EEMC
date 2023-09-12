using DevExpress.Mvvm;
using EEMC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.ViewModels
{
    internal class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Course _courses;
        public Course Courses
        {
            get => _courses;

            set
            {
                _courses = value;
                OnPropertyChanged("Courses");
            }
        }

        public MainWindowVM() 
        {
            _courses = CourseBuilder.Build(new ExplorerBuilder());
        }
    }
}
