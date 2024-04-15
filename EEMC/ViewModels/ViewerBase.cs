using DevExpress.Mvvm;
using EEMC.Models;
using EEMC.ToXPSConverteres;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;

namespace EEMC.ViewModels
{
    public class ViewerBase : ViewModelBase
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

        protected XpsDocument _xpsDocument;

        static WordConverter _wordConverter = new WordConverter();
        protected static CancellationTokenSource? _currentCancellationSource = null;

        public Commands.IAsyncCommand ShowDocument
        {
            get => new Commands.AsyncCommand(async (ChosenFile) =>
            {
                if (ChosenFile != null)
                {
                    XpsDocument oldXpsPackage = _xpsDocument;

                    if (Path.GetExtension((ChosenFile as Explorer).NameWithPath) == ".docx")
                    {
                        IsEnabledTW = false;

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
                    }

                    //Сигнализирует о том, что был вход в одну из ветвей ифа
                    if (!IsEnabledTW)
                    {
                        //null может быть, когда слишком быстро переключаешь окна (одно ещё не загрузилось, а второе уже включаем)
                        if (_xpsDocument != null)
                            Document = _xpsDocument.GetFixedDocumentSequence();

                        oldXpsPackage?.Close();

                        _xpsDocument?.Close();
                        IsEnabledTW = true;
                    }
                }
            });
        }
    }
}
