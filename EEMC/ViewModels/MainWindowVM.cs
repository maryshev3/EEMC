
using EEMC.Models;
using EEMC.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

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

        private Page _currentPage;
        public Page CurrentPage 
        {
            get => _currentPage;
            set 
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        public MainWindowVM()
        {
            Courses = new Course();
        }

        public ICommand bMenu_Click 
        {
            get => new DelegateCommand((obj) => CurrentPage = new CourseWindow());
        }
    }
}