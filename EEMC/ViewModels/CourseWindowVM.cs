using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using EEMC.ToXPSConverteres;
using EEMC.Views;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Packaging;

namespace EEMC.ViewModels
{
    public class CourseWindowVM : ViewerBase
    {
        private Explorer? _currentCourse;

        public Explorer? CurrentCourse
        {
            get => _currentCourse;
            set
            {
                _currentCourse = value;
                RaisePropertyChanged(() => CurrentCourse);
            }
        }

        private readonly MessageBus _messageBus;

        public CourseWindowVM(
            MessageBus messageBus,
            ConverterUtils converterUtils
        ) : base(converterUtils)
        {
            _messageBus = messageBus;

            _messageBus.Receive<CourseMessage>(this, async (message) =>
                {
                    CurrentCourse = message.Course;
                });
        }

        private readonly BitmapImage _icon = new BitmapImage(new Uri("pack://application:,,,/Resources/app_icon.png", UriKind.RelativeOrAbsolute));

        public ICommand AddGroup
        {
            get => new Commands.DelegateCommand(async (obj) =>
            {
                Window window = new Window
                {
                    Icon = _icon,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Добавление группы",
                    Content = new AddFolder()
                };

                await _messageBus.SendTo<AddFolderVM>(new ExplorerWindowMessage(window, _currentCourse));

                window.ShowDialog();
            }
            );
        }

        public ICommand AddFolder
        {
            get => new Commands.DelegateCommand(async (ChosenFolder) =>
            {
                if (ChosenFolder == null)
                    ChosenFolder = _currentCourse;

                if (ChosenFolder is Explorer && (ChosenFolder as Explorer).Type == ContentType.File)
                    return;

                Window window = new Window
                {
                    Icon = _icon,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Добавление раздела",
                    Content = new AddFolder()
                };

                await _messageBus.SendTo<AddFolderVM>(new ExplorerWindowMessage(window, ChosenFolder as Explorer));

                window.ShowDialog();
            }
            );
        }

        public ICommand AddFile
        {
            get => new Commands.DelegateCommand(async (ChosenFolder) =>
            {
                if (ChosenFolder == null)
                    ChosenFolder = _currentCourse;

                if (ChosenFolder is Explorer && (ChosenFolder as Explorer).Type == ContentType.File)
                    return;

                var fileDialog = new OpenFileDialog();

                if (fileDialog.ShowDialog() == true)
                {
                    var filePath = fileDialog.FileName;

                    (ChosenFolder as Explorer).AddFile(filePath);
                }
            }
            );
        }

        public ICommand Rename
        {
            get => new Commands.DelegateCommand(async (chosenCourse) =>
            {
                if (chosenCourse == null)
                    return;

                if (chosenCourse is Explorer && (chosenCourse as Explorer).Type == ContentType.File)
                    return;

                Window window = new Window
                {
                    Icon = _icon,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Переименование раздела",
                    Content = new RenameCourse()
                };

                await _messageBus.SendTo<RenameCourseVM>(new ExplorerWindowMessage(window, chosenCourse as Explorer));

                window.ShowDialog();
            }
            );
        }

        public ICommand Remove
        {
            get => new Commands.DelegateCommand(async (chosenCourse) =>
            {
                IsEnabledTW = true;

                if (_currentCancellationSource != null)
                {
                    _currentCancellationSource?.Cancel();
                    _currentCancellationSource?.Token.WaitHandle.WaitOne();

                    _currentCancellationSource?.Dispose();

                    _currentCancellationSource = null;
                }

                _xpsDocument?.Close();

                if (chosenCourse == null)
                    return;

                Window window = new Window
                {
                    Icon = _icon,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Удаление раздела",
                    Content = new RemoveCourse()
                };

                var converted = chosenCourse as Explorer;

                await _messageBus.SendTo<RemoveCourseVM>(new ExplorerWindowMessage(window, converted));

                window.ShowDialog();
            }
            );
        }

        private static readonly string _defaultUriString = "about:blank";
        private Uri _pdfPath = new Uri(_defaultUriString);
        public Uri PdfPath 
        {
            get => _pdfPath;
            set
            {
                _pdfPath = value;
                RaisePropertyChanged(() => PdfPath);
                RaisePropertyChanged(() => PdfViewVisibility);
                RaisePropertyChanged(() => XpsViewVisibility);
            }
        }

        public Visibility PdfViewVisibility
        {
            get => _pdfPath.OriginalString != _defaultUriString ? Visibility.Visible : Visibility.Collapsed;
        }

        public Visibility XpsViewVisibility
        {
            get => _pdfPath.OriginalString == _defaultUriString ? Visibility.Visible : Visibility.Collapsed;
        }

        public ICommand ShowFile_Click
        {
            get => new Commands.DelegateCommand(async (chosenFile) =>
            {
                Explorer chosenFileConverted = chosenFile as Explorer;

                if (chosenFileConverted.IsText())
                {
                    PdfPath = new Uri(_defaultUriString);
                    await this.ShowDocument.ExecuteAsync(chosenFile);
                }

                if (chosenFileConverted.IsPdf())
                {
                    PdfPath = new Uri(Environment.CurrentDirectory + chosenFileConverted.NameWithPath);
                }
            }
            );
        }
    }
}
