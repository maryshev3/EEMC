using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EEMC.ViewModels
{
    public class AddTotalTestVM : ViewModelBase
    {
        private readonly MessageBus _messageBus;

        private Window? _window;
        private Theme? _chosenTheme;
        private string? _testName;
        private ThemeToTests[]? _themeToTests;
        public ThemeToTests[]? ThemeToTests
        {
            get => _themeToTests;
            set
            {
                _themeToTests = value;
                RaisePropertyChanged(() => ThemeToTests);
            }
        }

        public AddTotalTestVM(MessageBus messageBus)
        {
            _messageBus = messageBus;

            _messageBus.Receive<ThemeWindowStringTestsMessage>(this, async (message) =>
            {
                _window = message.Window;
                _chosenTheme = message.Theme;
                _testName = message.String;
                ThemeToTests = message.ThemeToTests;
            }
            );
        }

        public ICommand Add_Click
        {
            get => new Commands.DelegateCommand((obj) =>
            {
                var response = TestService.ValidateForTotalTest(ThemeToTests);

                if (!response.IsValid)
                {
                    MessageBox.Show(response.ValidErrorText);

                    return;
                }

                string filePath = TestService.SaveTotalTest(ThemeToTests, _testName);

                _chosenTheme.AddFile(filePath);

                _window.Close();
            }
            );
        }

        public ICommand Cancel_Click
        {
            get => new Commands.DelegateCommand((obj) =>
            {
                _window?.Close();
            }
            );
        }
    }
}
