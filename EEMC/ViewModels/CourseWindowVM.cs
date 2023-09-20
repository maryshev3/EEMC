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

        public ICommand ShowDocument 
        {
            get => new DelegateCommand((ChosenFile) =>
            {
                if (Path.GetExtension((ChosenFile as Explorer).NameWithPath) == ".docx") 
                {
                    //string newXPSDocumentName = String.Concat(System.IO.Path.GetDirectoryName(dlg.FileName), "\\",  System.IO.Path.GetFileNameWithoutExtension(dlg.FileName), ".xps");
                    string OrigibDocumentName = Environment.CurrentDirectory + "\\" + (ChosenFile as Explorer).NameWithPath;

                    string str = Path.Combine(Environment.CurrentDirectory, Path.GetFileNameWithoutExtension((ChosenFile as Explorer).Name) + ".xps");

                    Document = new WordConverter().ToXpsConvert(OrigibDocumentName, Path.Combine(Environment.CurrentDirectory, Path.GetFileNameWithoutExtension((ChosenFile as Explorer).Name) + ".xps")).GetFixedDocumentSequence();
                    //Document = new WordConverter().ToXpsConvert("D:\\netLessons\\EEMC\\EEMC\\bin\\Debug\\net6.0-windows\\Курсы\\Мат анализ\\Новая папка\\wr.docx", "D:\\netLessons\\EEMC\\EEMC\\bin\\Debug\\net6.0-windows\\Курсы\\Мат анализ\\Новая папка\\wr.xps");
                    
                    //MessageBox.Show(newXPSDocumentName);
                }
            });
        }
            
    }
}
