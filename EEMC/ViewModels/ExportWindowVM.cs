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

        private readonly ConverterUtils _converterUtils;
        private List<Explorer> _courses;

        public ExportWindowVM(
            MessageBus messageBus,
            ConverterUtils converterUtils
        )
        {
            //_importExportService = new ImportExportService(converterUtils, Theme.ReadAllThemes(), courses);
            _converterUtils = converterUtils;

            _messageBus = messageBus;

            _messageBus.Receive<WindowExplorersMessage>(this, async (message) =>
            {
                _window = message.Window;
                _courses = message.ExplorerList;
            }
            );
        }

        public ICommand Export_Click
        {
            get => new Commands.DelegateCommand(async (description) =>
                {
                    IsEnabledWindow = false;

                    ImportExportService importExportService = new(
                        _converterUtils,
                        Theme.ReadAllThemes().Where(
                            x => 
                            _courses.Select(y => y.Name).ToHashSet().Contains(x.CourseName)
                            && !x.IsHiden
                        )
                        .ToArray(),
                        _courses
                    );

                    string descriptionConverted = description as string;

                    Guid id = String.IsNullOrWhiteSpace(descriptionConverted) 
                        ? await importExportService.Export()
                        : await importExportService.Export(descriptionConverted);

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
