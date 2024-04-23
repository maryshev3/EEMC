using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.ViewModels
{
    public class VideoViewVM : ViewModelBase
    {
        private readonly MessageBus _messageBus;

        private ThemeFile _themeFile;

        public ThemeFile ThemeFile
        {
            get => _themeFile;
            set
            {
                _themeFile = value;
                RaisePropertyChanged(() => ThemeFile);
            }
        }

        public VideoViewVM(MessageBus messageBus)
        {
            _messageBus = messageBus;

            _messageBus.Receive<ThemeFileMessage>(this, async (message) =>
            {
                _themeFile = message.ThemeFile;
            }
            );
        }
    }
}
