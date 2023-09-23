using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using EEMC.ToXPSConverteres;
using EEMC.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Xps.Packaging;

namespace EEMC.ViewModels
{
    public class CourseWindowVM : ViewModelBase
    {
        private FixedDocumentSequence _document;

        public FixedDocumentSequence Document 
        {
            get => _document;
            set 
            {
                _document = value;
                RaisePropertyChanged(() => Document);
            }
        }

        private XpsDocument _xpsDocument;

        private string _chosenFileWithoutExtension = "";

        private Explorer _currentCourse;

        public Explorer CurrentCourse
        {
            get => _currentCourse;
            set
            {
                _currentCourse = value;
                RaisePropertyChanged(() => CurrentCourse);
            }
        }

        private readonly MessageBus _messageBus;

        public CourseWindowVM(MessageBus messageBus)
        {
            _messageBus = messageBus;

            _messageBus.Receive<CourseMessage>(this, async (message) =>
                {
                    CurrentCourse = message.Course;
                });
        }

        static WordConverter _wordConverter = new WordConverter();

        public ICommand ShowDocument 
        {
            get => new DelegateCommand((ChosenFile) =>
            {
                if (Path.GetExtension((ChosenFile as Explorer).NameWithPath) == ".docx" && Path.GetFileNameWithoutExtension((ChosenFile as Explorer).Name) != _chosenFileWithoutExtension) 
                {
                    XpsDocument oldXpsPackage = _xpsDocument;
                    string OrigibDocumentName = Environment.CurrentDirectory + "\\" + (ChosenFile as Explorer).NameWithPath;
                    _chosenFileWithoutExtension = Path.GetFileNameWithoutExtension(OrigibDocumentName);
                    string str = Path.Combine(Environment.CurrentDirectory, Path.GetFileNameWithoutExtension((ChosenFile as Explorer).Name) + ".xps");

                    _xpsDocument = _wordConverter.ToXpsConvert(OrigibDocumentName, Path.Combine(Environment.CurrentDirectory, Path.GetFileNameWithoutExtension((ChosenFile as Explorer).Name) + ".xps"));
                    Document = _xpsDocument.GetFixedDocumentSequence();

                    if (oldXpsPackage != null)
                        oldXpsPackage.Close();
                }
            });
        }
            
    }
}
