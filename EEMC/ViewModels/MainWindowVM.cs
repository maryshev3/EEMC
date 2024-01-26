using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using EEMC.Views;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EEMC.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        private Course _courses;
        public Course Courses
        {
            get => _courses;

            set
            {
                _courses = value;

                if (_chosenCourse != null)
                {
                    CurrentPage = new CourseWindow();

                    _messageBus.SendTo<CourseWindowVM>(new CourseMessage(_chosenCourse));
                }

                RaisePropertyChanged(() => Courses);
            }
        }

        private Page _currentPage = null;
        public Page CurrentPage 
        {
            get => _currentPage;
            set 
            {
                _currentPage = value;
                RaisePropertyChanged(() => CurrentPage);
            }
        }

        private readonly MessageBus _messageBus;
        private Explorer? _chosenCourse = null;

        private async void OnDirectoryChanged(object sender, FileSystemEventArgs e)
        {
            string fileExt = Path.GetExtension(e.Name);

            if (fileExt == "" && e.ChangeType == WatcherChangeTypes.Deleted && e.Name == _chosenCourse.Name)
            {
                _chosenCourse = null;
                await _messageBus.SendTo<CourseWindowVM>(new CourseMessage(_chosenCourse));
            }

            if (e.Name.Contains("~$") || fileExt == "")
                return;

            if (_chosenCourse != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CurrentPage = new CourseWindow();
                });

                foreach(var course in Courses.Courses)
                {
                    if (course.Name == _chosenCourse.Name)
                    {
                        _chosenCourse = course;
                        break;
                    }
                }

                await _messageBus.SendTo<CourseWindowVM>(new CourseMessage(_chosenCourse));
            }
        }

        public MainWindowVM(Course courses, MessageBus messageBus)
        {
            _courses = courses;
            _messageBus = messageBus;

            _courses.AddWatcherHandler(OnDirectoryChanged);
        }

        public ICommand bMenu_Click 
        {
            get => new Commands.DelegateCommand(async (ChosenCourse) => 
            {
                CurrentPage = new CourseWindow();

                _chosenCourse = ChosenCourse as Explorer;

                await _messageBus.SendTo<CourseWindowVM>(new CourseMessage(_chosenCourse));
            });
        }
    }
}