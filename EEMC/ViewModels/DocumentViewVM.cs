using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using EEMC.ToXPSConverteres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.ViewModels
{
    public class DocumentViewVM : ViewerBase
    {
        private readonly MessageBus _messageBus;

        public ThemeFile themeFile;

        public DocumentViewVM(
            MessageBus messageBus,
            ConverterUtils converterUtils
        ) : base(converterUtils)
        {
            _messageBus = messageBus;

            _messageBus.Receive<ThemeFileMessage>(this, async (message) =>
            {
                themeFile = message.ThemeFile;
            }
            );
        }
    }
}
