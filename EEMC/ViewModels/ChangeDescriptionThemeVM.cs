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
    public class ChangeDescriptionThemeVM
    {
        private readonly MessageBus _messageBus;
        private Window? _window;
        private Theme? _chosenTheme;

        public ChangeDescriptionThemeVM(MessageBus messageBus)
        {
            _messageBus = messageBus;

            _messageBus.Receive<ThemeWindowMessage>(this, async (message) =>
            {
                _window = message.Window;
                _chosenTheme = message.Theme;
            }
            );
        }

        public ICommand ChangeDescriptionTheme_Click
        {
            get => new Commands.DelegateCommand((newDescription) =>
            {
                try
                {
                    _chosenTheme.ChangeDescription(newDescription as string);

                    _window?.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            );
        }
    }
}
