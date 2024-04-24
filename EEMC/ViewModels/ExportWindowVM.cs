using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using EEMC.Services;
using EEMC.Messages;
using EEMC.Models;
using EEMC.ToXPSConverteres;

namespace EEMC.ViewModels
{
    public class ExportWindowVM : ViewModelBase
    {
        private readonly MessageBus _messageBus;
        private Window? _window;

        private bool _isEnabledWindow = true;
        public bool IsEnabledWindow 
        {
            get => _isEnabledWindow;
            set
            {
                _isEnabledWindow = value;
                RaisePropertyChanged(() => IsEnabledWindow);
            }
        }

        private readonly ImportExportService _importExportService;

        public ExportWindowVM(
            MessageBus messageBus,
            Course courses,
            ConverterUtils converterUtils
        )
        {
            _importExportService = new ImportExportService(converterUtils, Theme.ReadAllThemes(), courses);

            _messageBus = messageBus;

            _messageBus.Receive<WindowMessage>(this, async (message) =>
            {
                _window = message.Window;
            }
            );
        }

        public ICommand Export_Click
        {
            get => new Commands.DelegateCommand(async (description) =>
                {
                    IsEnabledWindow = false;

                    string descriptionConverted = description as string;

                    Guid id = String.IsNullOrWhiteSpace(descriptionConverted) 
                        ? await _importExportService.Export()
                        : await _importExportService.Export(descriptionConverted);

                    MessageBox.Show($"Текущая коллекция курсов была успешно экспортирована. ID версии: {id}");

                    IsEnabledWindow = true;

                    _window?.Close();
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
