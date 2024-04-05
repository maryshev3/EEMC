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
    public class CoursesListVM : CoursesListVMBase
    {
        public override Course Courses
        {
            get => _courses;

            set
            {
                _courses = value;

                RaisePropertyChanged(() => Courses);
            }
        }

        protected override async void OnDirectoryChanged(object sender, FileSystemEventArgs e)
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

                await _messageBus.SendTo<SwitcherCourseViewVM>(new CourseMessage(_chosenCourse));
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

        public override ICommand OpenCourse_Click
        {
            get => new Commands.DelegateCommand(async (ChosenCourse) =>
                {
                    //_chosenCourse = ChosenCourse as Explorer;
                    _mainWindowVM = _serviceProvider.GetService(typeof(MainWindowVM)) as MainWindowVM;

                    var shell = System.Windows.Application.Current.MainWindow as MainWindow;
                    shell.UpdateChosenCourse((ChosenCourse as Explorer).Name);

                    await _mainWindowVM.ChangeCurrentCourse(ChosenCourse as Explorer);
                }
            );
        }
    }
}