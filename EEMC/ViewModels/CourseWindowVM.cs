using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using EEMC.ToXPSConverteres;
using System;
using System.IO;
using System.Threading;
using System.Windows.Documents;
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

        private Explorer? _currentCourse;

        public Explorer? CurrentCourse
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
        static CancellationTokenSource? _currentCancellationSource = null;

        public Commands.IAsyncCommand ShowDocument
        {
            get => new Commands.AsyncCommand(async (ChosenFile) =>
            {
                if (ChosenFile != null)
                {
                    if (Path.GetExtension((ChosenFile as Explorer).NameWithPath) == ".docx")
                    {
                        XpsDocument oldXpsPackage = _xpsDocument;

                        string OriginDocumentName = Environment.CurrentDirectory + "\\" + (ChosenFile as Explorer).NameWithPath;

                        if (_currentCancellationSource != null)
                        {
                            _currentCancellationSource.Cancel();
                            _currentCancellationSource.Token.WaitHandle.WaitOne();
                        }

                        _currentCancellationSource = new CancellationTokenSource();
                        _xpsDocument = await _wordConverter.ToXpsConvertAsync(
                            OriginDocumentName,
                            Path.Combine(Environment.CurrentDirectory, Path.GetFileNameWithoutExtension((ChosenFile as Explorer).Name) + ".xps"),
                            _currentCancellationSource.Token
                        );
                        _currentCancellationSource = null;

                        //null может быть, когда слишком быстро переключаешь окна (одно ещё не загрузилось, а второе уже включаем)
                        if (_xpsDocument != null)
                            Document = _xpsDocument.GetFixedDocumentSequence();

                        if (oldXpsPackage != null)
                            oldXpsPackage.Close();
                    }
                }
            });
        }

    }
}
