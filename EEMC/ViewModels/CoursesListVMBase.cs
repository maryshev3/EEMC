using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using EEMC.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EEMC.ViewModels
{
    public abstract class CoursesListVMBase : ViewModelBase
    {
        protected Course _courses;
        public abstract Course Courses { get; set; }

        protected MessageBus _messageBus;
        protected Explorer? _chosenCourse = null;

        protected abstract void OnDirectoryChanged(object sender, FileSystemEventArgs e);

        private readonly BitmapImage _icon = new BitmapImage(new Uri("pack://application:,,,/Resources/app_icon.png", UriKind.RelativeOrAbsolute));

        public ICommand AddCourse_Click
        {
            get => new Commands.DelegateCommand(async (obj) =>
            {
                Window window = new Window
                {
                    Icon = _icon,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
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
                Explorer? explorer = chosenCourse as Explorer;

                if (explorer == null)
                    explorer = _chosenCourse;

                Window window = new Window
                {
                    Icon = _icon,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Переименование курса",
                    Content = new RenameCourse()
                };

                await _messageBus.SendTo<RenameCourseVM>(new ExplorerWindowMessage(window, explorer));

                window.ShowDialog();
            }
            );
        }

        public ICommand RemoveCourse_Click
        {
            get => new Commands.DelegateCommand(async (chosenCourse) =>
            {
                Explorer? explorer = chosenCourse as Explorer;

                if (explorer == null)
                    explorer = _chosenCourse;

                Window window = new Window
                {
                    Icon = _icon,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Удаление курса",
                    Content = new RemoveCourse()
                };

                await _messageBus.SendTo<RemoveCourseVM>(new ExplorerWindowMessage(window, explorer));

                window.ShowDialog();
            }
            );
        }

        public abstract ICommand OpenCourse_Click { get; }
    }
}
