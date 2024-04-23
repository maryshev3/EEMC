using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
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
    public class MainWindowVM : CoursesListVMBase
    {
        public override Course Courses
        {
            get => _courses;

            set
            {
                _courses = value;

                if (_chosenCourse != null)
                {
                    CurrentPage = new SwitcherCourseView();

                    _messageBus.SendTo<SwitcherCourseViewVM>(new CourseMessage(_chosenCourse));
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

        protected override async void OnDirectoryChanged(object sender, FileSystemEventArgs e)
        {
            string fileExt = Path.GetExtension(e.Name);

            if (_chosenCourse == null || fileExt == "" && e.ChangeType == WatcherChangeTypes.Deleted && e.Name == _chosenCourse.Name)
            {
                _chosenCourse = null;
                await _messageBus.SendTo<SwitcherCourseViewVM>(new CourseMessage(_chosenCourse));
            }

            if (e.Name.Contains("~$"))
                return;

            if (_chosenCourse != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CurrentPage = new SwitcherCourseView();
                });

                foreach(var course in Courses.Courses)
                {
                    if (course.Name == _chosenCourse.Name)
                    {
                        _chosenCourse = course;
                        break;
                    }
                }

                await _messageBus.SendTo<SwitcherCourseViewVM>(new CourseMessage(_chosenCourse));
            }
        }

        public MainWindowVM(
            Course courses,
            MessageBus messageBus
        )
        {
            _courses = courses;
            _messageBus = messageBus;

            _courses.AddWatcherHandler(OnDirectoryChanged);

            OpenHomePage();
        }

        private Visibility _visibilityHomeButton;
        public Visibility VisibilityHomeButton
        {
            get => _visibilityHomeButton;
            set
            {
                _visibilityHomeButton = value;
                RaisePropertyChanged(() => VisibilityHomeButton);
            }
        }

        private void OpenHomePage()
        {
            CurrentPage = new CoursesList();
            _chosenCourse = null;
            VisibilityHomeButton = Visibility.Collapsed;
        }

        public async Task ChangeCurrentCourse(Explorer chosenCourse)
        {
            _chosenCourse = chosenCourse;

            CurrentPage = new SwitcherCourseView();
            VisibilityHomeButton = Visibility.Visible;

            await _messageBus.SendTo<SwitcherCourseViewVM>(new CourseMessage(_chosenCourse));
        }

        public override ICommand OpenCourse_Click
        {
            get => new Commands.DelegateCommand(async (ChosenCourse) => 
                {
                    await ChangeCurrentCourse(ChosenCourse as Explorer);
                }
            );
        }

        public ICommand OpenHomePage_Click
        {
            get => new Commands.DelegateCommand(async (obj) =>
            {
                OpenHomePage();
            }
            );
        }
    }
}