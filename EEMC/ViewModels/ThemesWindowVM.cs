using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using EEMC.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media.Imaging;

namespace EEMC.ViewModels
{
    public class ThemesWindowVM : ViewModelBase
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

        public ThemesWindowVM(MessageBus messageBus)
        {
            _messageBus = messageBus;

            _messageBus.Receive<CourseMessage>(this, async (message) =>
            {
                CurrentCourse = message.Course;
            });
        }

        private readonly BitmapImage _icon = new BitmapImage(new Uri("pack://application:,,,/Resources/app_icon.png", UriKind.RelativeOrAbsolute));

        public ICommand AddTheme_Click
        {
            get => new Commands.DelegateCommand(async (obj) =>
            {
                Window window = new Window
                {
                    Icon = _icon,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Добавление темы",
                    Content = new AddTheme()
                };

                await _messageBus.SendTo<AddThemeVM>(new ExplorerWindowMessage(window, CurrentCourse));

                window.ShowDialog();
            }
            );
        }

        public ICommand RenameTheme_Click
        {
            get => new Commands.DelegateCommand(async (currentTheme) =>
            {
                Window window = new Window
                {
                    Icon = _icon,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Переименование темы",
                    Content = new RenameTheme()
                };

                await _messageBus.SendTo<RenameThemeVM>(new ThemeWindowMessage(window, currentTheme as Theme));

                window.ShowDialog();
            }
            );
        }

        public ICommand ChangeDescriptionTheme_Click
        {
            get => new Commands.DelegateCommand(async (currentTheme) =>
            {
                Window window = new Window
                {
                    Icon = _icon,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Изменение описания темы",
                    Content = new ChangeDescriptionTheme()
                };

                await _messageBus.SendTo<ChangeDescriptionThemeVM>(new ThemeWindowMessage(window, currentTheme as Theme));

                window.ShowDialog();
            }
            );
        }

        public ICommand RemoveTheme_Click
        {
            get => new Commands.DelegateCommand(async (currentTheme) =>
            {
                Window window = new Window
                {
                    Icon = _icon,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Удаление темы",
                    Content = new RemoveTheme()
                };

                await _messageBus.SendTo<RemoveThemeVM>(new ThemeWindowMessage(window, currentTheme as Theme));

                window.ShowDialog();
            }
            );
        }

        public ICommand ChangeHidenModeTheme_Click
        {
            get => new Commands.DelegateCommand(async (currentTheme) =>
            {
                Theme currentThemeConverted = currentTheme as Theme;

                if (currentThemeConverted != default)
                    currentThemeConverted.ChangeHidenMode();
            }
            );
        }

        public ICommand ShowFile_Click
        {
            get => new Commands.DelegateCommand(async (currentFile) =>
            {
                ThemeFile currentFileConverted = currentFile as ThemeFile;

                ;
            }
            );
        }

        public ICommand DownloadFile_Click
        {
            get => new Commands.DelegateCommand(async (currentFile) =>
            {
                ThemeFile currentFileConverted = currentFile as ThemeFile;

                ;
            }
            );
        }
    }
}
