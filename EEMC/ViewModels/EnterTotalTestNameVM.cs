using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using EEMC.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EEMC.ViewModels
{
    public class EnterTotalTestNameVM
    {
        private readonly MessageBus _messageBus;
        private Window? _window;
        private ThemeToTests[]? _themeToTests;
        private Theme? _chosenTheme;

        public EnterTotalTestNameVM(MessageBus messageBus)
        {
            _messageBus = messageBus;

            _messageBus.Receive<ThemeToTestsWindowThemeMessage>(this, async (message) =>
            {
                _window = message.Window;
                _themeToTests = message.ThemeToTests;
                _chosenTheme = message.Theme;
            }
            );
        }

        public ICommand EnterTestName_Click
        {
            get => new Commands.DelegateCommand(async (testName) =>
            {
                try
                {
                    string testNameConverted = testName as string;

                    if (String.IsNullOrWhiteSpace(testNameConverted))
                    {
                        MessageBox.Show("Не было введено название теста");
                        return;
                    }

                    if (testNameConverted is "Курсы" or "runtimes" or "Файлы тем" or "EEMC.exe.WebView2")
                    {
                        MessageBox.Show("Название теста не может совпадать с одним из следующих:\nКурсы\nruntimes\nФайлы тем\nEEMC.exe.WebView2\n");
                        return;
                    }

                    if (_chosenTheme.Files != null && _chosenTheme.Files.Where(x => x.IsTotalTest()).Select(x => x.Name).Contains(testNameConverted))
                    {
                        MessageBox.Show("Тест с таким названием уже есть в теме");
                        return;
                    }

                    //Открываем формы создания теста и отправляем введенное название теста
                    Window window = new AddTotalTest();

                    await _messageBus.SendTo<AddTotalTestVM>(new ThemeWindowStringTestsMessage(window, _chosenTheme, testNameConverted, _themeToTests));

                    window.ShowDialog();

                    _window?.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            );
        }

        public ICommand Cancel_Click
        {
            get => new Commands.DelegateCommand((courseName) =>
            {
                _window?.Close();
            }
            );
        }
    }
}
