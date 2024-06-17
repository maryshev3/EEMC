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
    public class EditTotalTestVM : ViewModelBase
    {
        private readonly MessageBus _messageBus;

        private Window? _window;
        private Theme? _chosenTheme;
        private ThemeFile? _themeFile;
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

        public EditTotalTestVM(MessageBus messageBus)
        {
            _messageBus = messageBus;

            _messageBus.Receive<ThemeToTestsWindowThemeAndFileMessage>(this, async (message) =>
            {
                _window = message.Window;
                _chosenTheme = message.Theme;
                _themeFile = message.ThemeFile;
                ThemeToTests = message.ThemeToTests;
            }
            );
        }

        public ICommand Edit_Click
        {
            get => new Commands.DelegateCommand((obj) =>
            {
                var response = TestService.ValidateForTotalTest(ThemeToTests);

                if (!response.IsValid)
                {
                    MessageBox.Show(response.ValidErrorText);

                    return;
                }

                _themeFile.RemoveFile();

                string filePath = TestService.SaveTotalTest(ThemeToTests, _themeFile.NameWithoutExtension);

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
