using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using EEMC.ToXPSConverteres;
using EEMC.Views;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Xps.Packaging;

namespace EEMC.ViewModels
{
    public class CourseWindowVM : ViewModelBase
    {
        private bool _isEnabledTW = true;
        public bool IsEnabledTW 
        {
            get => _isEnabledTW;
            set
            {
                _isEnabledTW = value;
                RaisePropertyChanged(() => IsEnabledTW);
            }
        }

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
                        IsEnabledTW = false;

                        XpsDocument oldXpsPackage = _xpsDocument;

                        string OriginDocumentName = Environment.CurrentDirectory + "\\" + (ChosenFile as Explorer).NameWithPath;

                        if (_currentCancellationSource != null)
                        {
                            _currentCancellationSource?.Cancel();
                            _currentCancellationSource?.Token.WaitHandle.WaitOne();

                            _currentCancellationSource?.Dispose();

                            _currentCancellationSource = null;
                        }

                        _currentCancellationSource = new CancellationTokenSource();
                        try
                        {
                            _xpsDocument = await _wordConverter.ToXpsConvertAsync(
                                OriginDocumentName,
                                Path.Combine(Environment.CurrentDirectory, Path.GetFileNameWithoutExtension((ChosenFile as Explorer).Name) + ".xps"),
                                _currentCancellationSource.Token
                            );
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);

                            _xpsDocument = null;
                        }
                        _currentCancellationSource?.Dispose();
                        _currentCancellationSource = null;

                        //null может быть, когда слишком быстро переключаешь окна (одно ещё не загрузилось, а второе уже включаем)
                        if (_xpsDocument != null)
                            Document = _xpsDocument.GetFixedDocumentSequence();

                        if (oldXpsPackage != null)
                            oldXpsPackage.Close();

                        _xpsDocument.Close();
                        IsEnabledTW = true;
                    }
                }
            });
        }

        public ICommand Add
        {
            get => new Commands.DelegateCommand(async (ChosenFolder) =>
            {
                if (ChosenFolder == null)
                    ChosenFolder = _currentCourse;

                if (ChosenFolder is Explorer && (ChosenFolder as Explorer).Type == ContentType.File)
                    return;

                Window window = new Window
                {
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Добавление раздела",
                    Content = new AddFolder()
                };

                await _messageBus.SendTo<AddFolderVM>(new ExplorerWindowMessage(window, ChosenFolder as Explorer));

                window.ShowDialog();
            }
            );
        }

        public ICommand AddFile
        {
            get => new Commands.DelegateCommand(async (ChosenFolder) =>
            {
                if (ChosenFolder == null)
                    ChosenFolder = _currentCourse;

                if (ChosenFolder is Explorer && (ChosenFolder as Explorer).Type == ContentType.File)
                    return;

                var fileDialog = new OpenFileDialog();

                if (fileDialog.ShowDialog() == true)
                {
                    var filePath = fileDialog.FileName;

                    (ChosenFolder as Explorer).AddFile(filePath);
                }
            }
            );
        }

        public ICommand Rename
        {
            get => new Commands.DelegateCommand(async (chosenCourse) =>
            {
                if (chosenCourse == null)
                    return;

                if (chosenCourse is Explorer && (chosenCourse as Explorer).Type == ContentType.File)
                    return;

                Window window = new Window
                {
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Переименование раздела",
                    Content = new RenameCourse()
                };

                await _messageBus.SendTo<RenameCourseVM>(new ExplorerWindowMessage(window, chosenCourse as Explorer));

                window.ShowDialog();
            }
            );
        }

        public ICommand Remove
        {
            get => new Commands.DelegateCommand(async (chosenCourse) =>
            {
                IsEnabledTW = true;

                if (_currentCancellationSource != null)
                {
                    _currentCancellationSource?.Cancel();
                    _currentCancellationSource?.Token.WaitHandle.WaitOne();

                    _currentCancellationSource?.Dispose();

                    _currentCancellationSource = null;
                }

                _xpsDocument?.Close();

                if (chosenCourse == null)
                    return;

                Window window = new Window
                {
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    Title = "Удаление раздела",
                    Content = new RemoveCourse()
                };

                var converted = chosenCourse as Explorer;

                await _messageBus.SendTo<RemoveCourseVM>(new ExplorerWindowMessage(window, converted));

                window.ShowDialog();
            }
            );
        }
    }
}
