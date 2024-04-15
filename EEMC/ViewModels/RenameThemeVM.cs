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
    public class RenameThemeVM
    {
        private readonly MessageBus _messageBus;
        private Window? _window;
        private Theme? _chosenTheme;

        public RenameThemeVM(MessageBus messageBus)
        {
            _messageBus = messageBus;

            _messageBus.Receive<ThemeWindowMessage>(this, async (message) =>
            {
                _window = message.Window;
                _chosenTheme = message.Theme;
            }
            );
        }

        public ICommand RenameTheme_Click
        {
            get => new Commands.DelegateCommand((themeName) =>
            {
                try
                {
                    _chosenTheme.RenameTheme(themeName as string);

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
