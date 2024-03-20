using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using EEMC.Views;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EEMC.ViewModels
{
    public class CoursesListVM : ViewModelBase
    {
        private Course _courses;
        public Course Courses
        {
            get => _courses;

            set
            {
                _courses = value;

                RaisePropertyChanged(() => Courses);
            }
        }

        private readonly MessageBus _messageBus;
        private Explorer? _chosenCourse = null;

        private async void OnDirectoryChanged(object sender, FileSystemEventArgs e)
        {
            string fileExt = Path.GetExtension(e.Name);

            if (e.Name.Contains("~$") || fileExt == "")
                return;

            if (_chosenCourse != null)
            {
                foreach (var course in Courses.Courses)
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

        private MainWindowVM _mainWindowVM;
        IServiceProvider _serviceProvider;

        public CoursesListVM(
            Course courses,
            MessageBus messageBus,
            IServiceProvider serviceProvider
        )
        {
            _courses = courses;
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;

            _courses.AddWatcherHandler(OnDirectoryChanged);
        }

        public ICommand AddCourse_Click
        {
            get => new Commands.DelegateCommand(async (obj) =>
            {

                Window window = new Window
                {
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Добавление курса",
                    Content = new AddCourse()
                };

                await _messageBus.SendTo<AddCourseVM>(new WindowMessage(window));

                window.ShowDialog();
            }
            );
        }

        public ICommand RenameCourse_Click
        {
            get => new Commands.DelegateCommand(async (chosenCourse) =>
            {
                Window window = new Window
                {
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Переименование курса",
                    Content = new RenameCourse()
                };

                await _messageBus.SendTo<RenameCourseVM>(new ExplorerWindowMessage(window, chosenCourse as Explorer));

                window.ShowDialog();
            }
            );
        }

        public ICommand RemoveCourse_Click
        {
            get => new Commands.DelegateCommand(async (chosenCourse) =>
            {
                Window window = new Window
                {
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Удаление курса",
                    Content = new RemoveCourse()
                };

                await _messageBus.SendTo<RemoveCourseVM>(new ExplorerWindowMessage(window, chosenCourse as Explorer));

                window.ShowDialog();
            }
            );
        }

        public ICommand bMenu_Click
        {
            get => new Commands.DelegateCommand(async (ChosenCourse) =>
                {
                    //_chosenCourse = ChosenCourse as Explorer;
                    _mainWindowVM = _serviceProvider.GetService(typeof(MainWindowVM)) as MainWindowVM;

                    await _mainWindowVM.ChangeCurrentCourse(ChosenCourse as Explorer);
                }
            );
        }
    }
}