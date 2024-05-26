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
using System.Windows;
using System.Windows.Input;

namespace EEMC.ViewModels
{
    public class EnterTestNameVM : ViewModelBase
    {
        private readonly MessageBus _messageBus;
        private Window? _window;
        private Theme? _chosenTheme;

        public EnterTestNameVM(MessageBus messageBus)
        {
            _messageBus = messageBus;

            _messageBus.Receive<ThemeWindowMessage>(this, async (message) =>
            {
                _window = message.Window;
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

                    //Открываем формы создания теста и отправляем введенное название теста
                    Window window = new CreateTest();

                    await _messageBus.SendTo<CreateTestVM>(new ThemeWindowStringMessage(window, _chosenTheme, testNameConverted));

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
