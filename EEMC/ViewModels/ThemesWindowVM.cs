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
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;

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

        public ICommand Guid_Click
        {
            get => new Commands.DelegateCommand(async (obj) =>
            {
                var window = new GuidTheme();

                window.ShowDialog();
            }
            );
        }

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

        public ICommand AddTest_Click
        {
            get => new Commands.DelegateCommand(async (currentTheme) =>
            {
                Window window = new Window
                {
                    Icon = _icon,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Создание теста",
                    Content = new EnterTestName()
                };

                await _messageBus.SendTo<EnterTestNameVM>(new ThemeWindowMessage(window, currentTheme as Theme));

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

        public ICommand CreateTotalTest_Click
        {
            get => new Commands.DelegateCommand(async (currentTheme) =>
            {
                Theme currentThemeConverted = currentTheme as Theme;

                //Найдём тесты со всех предыдущих тем
                var lastThemes = CurrentCourse.Themes.Where(x => x.ThemeNumber <= currentThemeConverted.ThemeNumber);
                var groupedTests = lastThemes
                    .Where(x => x.Files != null)
                    .Select(
                        x => new ThemeToTests {
                            Theme = x,
                            Tests = x.Files
                                .Where(y => y.IsTest())
                                .Select(y => TestService.Load(Environment.CurrentDirectory + y.NameWithPath))
                                .ToArray() 
                        }
                    )
                    .Where(x => x.Tests.Any())
                    .ToArray();

                if (!groupedTests.Any())
                {
                    MessageBox.Show("В этой теме или более ранних отсутствуют тесты для создания итогового контроля");

                    return;
                }

                Window window = new Window
                {
                    Icon = _icon,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Создание итогового теста",
                    Content = new EnterTotalTestName()
                };

                await _messageBus.SendTo<EnterTotalTestNameVM>(new ThemeToTestsWindowThemeMessage(groupedTests, currentThemeConverted, window));

                window.ShowDialog();
            }
            );
        }

        public ICommand EditTotalTest_Click
        {
            get => new Commands.DelegateCommand(async (currentFile) =>
            {
                ThemeFile currentFileConverted = currentFile as ThemeFile;

                Theme theme = CurrentCourse.Themes.First(x => x.Files != null && x.Files.Contains(currentFileConverted));

                //Найдём тесты со всех предыдущих тем
                var lastThemes = CurrentCourse.Themes.Where(x => x.ThemeNumber <= theme.ThemeNumber);
                var groupedTests = lastThemes
                    .Where(x => x.Files != null)
                    .Select(
                        x => new ThemeToTests
                        {
                            Theme = x,
                            Tests = x.Files
                                .Where(y => y.IsTest())
                                .Select(y => TestService.Load(Environment.CurrentDirectory + y.NameWithPath))
                                .ToArray()
                        }
                    )
                    .Where(x => x.Tests.Any())
                    .ToArray();

                if (!groupedTests.Any())
                {
                    MessageBox.Show("Отсутствует банк заданий для изменения состава итогового теста");

                    return;
                }

                //Заполняем GroupedTests существующими параметрами
                string json = File.ReadAllText(Environment.CurrentDirectory + currentFileConverted.NameWithPath);

                var tests = JsonConvert.DeserializeObject<TotalTestItem[]>(json);

                foreach (var test in tests)
                {
                    var thisTest = groupedTests.FirstOrDefault(x => x.Theme.ThemeName == test.ThemeName);

                    if (thisTest == default)
                        continue;

                    thisTest.IsChosenTheme = true;
                    thisTest.CountString = test.QuestionsCount.ToString();
                }

                Window window = new EditTotalTest();

                await _messageBus.SendTo<EditTotalTestVM>(new ThemeToTestsWindowThemeAndFileMessage(groupedTests, theme, window, currentFileConverted));

                window.ShowDialog();
            }
            );
        }

        public ICommand ShowFile_Click
        {
            get => new Commands.DelegateCommand(async (currentFile) =>
            {
                ThemeFile currentFileConverted = currentFile as ThemeFile;

                Window window = default;

                if (currentFileConverted.IsVideoOrAudio())
                {
                    window = new VideoView();

                    await _messageBus.SendTo<VideoViewVM>(new ThemeFileMessage(currentFileConverted));
                }
                else
                {
                    if (currentFileConverted.IsImage())
                    {
                        window = new ImageView();

                        await _messageBus.SendTo<ImageViewVM>(new ThemeFileMessage(currentFileConverted));
                    }
                    else
                    {
                        if (currentFileConverted.IsPdf())
                        {
                            window = new PdfView();

                            await _messageBus.SendTo<PdfViewVM>(new ThemeFileMessage(currentFileConverted));
                        }
                        else 
                        {
                            if (currentFileConverted.IsTest())
                            {
                                Test test = TestService.Load(Environment.CurrentDirectory + currentFileConverted.NameWithPath);

                                window = new TestView(test);

                                //await _messageBus.SendTo<TestViewVM>(new TestMessage(test));
                            }
                            else
                            {
                                if (currentFileConverted.IsTotalTest())
                                {
                                    string json = File.ReadAllText(Environment.CurrentDirectory + currentFileConverted.NameWithPath);

                                    var tests = JsonConvert.DeserializeObject<TotalTestItem[]>(json);

                                    Test test = TestService.TestFromTotalTest(tests);

                                    window = new TestView(test);
                                }
                                else
                                {
                                    window = new DocumentView();

                                    await _messageBus.SendTo<DocumentViewVM>(new ThemeFileMessage(currentFileConverted));
                                }

                                
                            }
                        }
                    }
                }

                window?.ShowDialog();
            }
            );
        }

        public ICommand DownloadFile_Click
        {
            get => new Commands.DelegateCommand(async (currentFile) =>
            {
                ThemeFile currentFileConverted = currentFile as ThemeFile;

                var fileDialog = new SaveFileDialog();
                fileDialog.Filter = currentFileConverted.GetSaveFilter();
                
                if (fileDialog.ShowDialog() == true)
                {
                    var filePath = fileDialog.FileName;

                    currentFileConverted.SaveFile(filePath);
                }
            }
            );
        }

        public ICommand AddFile_Click
        {
            get => new Commands.DelegateCommand(async (currentTheme) =>
            {
                Theme currentThemeConverted = currentTheme as Theme;

                var fileDialog = new OpenFileDialog();

                if (fileDialog.ShowDialog() == true)
                {
                    var filePath = fileDialog.FileName;
                    try
                    {
                        currentThemeConverted.AddFile(filePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            );
        }

        public ICommand RemoveFile_Click
        {
            get => new Commands.DelegateCommand(async (currentFile) =>
            {
                ThemeFile currentFileConverted = currentFile as ThemeFile;

                currentFileConverted.RemoveFile();
            }
            );
        }

        public ICommand Up
        {
            get => new Commands.DelegateCommand((theme) =>
            {
                Theme themeConverted = theme as Theme;

                _currentCourse.Up(themeConverted);
            });
        }

        public ICommand Down
        {
            get => new Commands.DelegateCommand((theme) =>
            {
                Theme themeConverted = theme as Theme;

                _currentCourse.Down(themeConverted);
            });
        }
    }
}
