using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using EEMC.ToXPSConverteres;
using EEMC.Views;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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

        private readonly ImportExportService _importExportService;

        public CoursesListVM(
            Course courses,
            ConverterUtils converterUtils,
            MessageBus messageBus,
            IServiceProvider serviceProvider
        )
        {
            _courses = courses;
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;

            _importExportService = new ImportExportService(converterUtils, Theme.ReadAllThemes(), courses);

            _courses.AddWatcherHandler(OnDirectoryChanged);
        }

        public override ICommand OpenCourse_Click
        {
            get => new Commands.DelegateCommand(async (ChosenCourse) =>
                {
                    //_chosenCourse = ChosenCourse as Explorer;
                    _mainWindowVM = _serviceProvider.GetService(typeof(MainWindowVM)) as MainWindowVM;

                    _mainWindowVM.VisibilityHomeButton = Visibility.Visible;

                    var shell = System.Windows.Application.Current.MainWindow as MainWindow;
                    shell.UpdateChosenCourse((ChosenCourse as Explorer).Name);

                    await _mainWindowVM.ChangeCurrentCourse(ChosenCourse as Explorer);
                }
            );
        }

        private readonly BitmapImage _icon = new BitmapImage(new Uri("pack://application:,,,/Resources/app_icon.png", UriKind.RelativeOrAbsolute));

        public ICommand Export_Click
        {
            get => new Commands.DelegateCommand(async (obj) =>
            {
                Window window = new Window
                {
                    Icon = _icon,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Создание новой версии",
                    Content = new ExportWindow()
                };

                await _messageBus.SendTo<ExportWindowVM>(new WindowMessage(window));

                window.ShowDialog();
            }
            );
        }

        public ICommand Import_Click
        {
            get => new Commands.DelegateCommand(async (obj) =>
            {
                var fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Courses exported file | *.ce";

                if (fileDialog.ShowDialog() == true)
                {
                    var filePath = fileDialog.FileName;
                    try
                    {
                        _importExportService.Import(filePath);

                        MessageBox.Show("Курсы были успешно импортированы");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            );
        }

        public ICommand Versions_Click
        {
            get => new Commands.DelegateCommand(async (obj) =>
            {
                Window window = new Window
                {
                    Icon = _icon,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Просмотр версий",
                    Content = new VersionsView()
                };

                window.ShowDialog();
            }
            );
        }
    }
}