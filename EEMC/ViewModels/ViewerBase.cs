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
        static TxtConverter _txtConverter = new TxtConverter();
        
        protected static CancellationTokenSource? _currentCancellationSource = null;

        public Commands.IAsyncCommand ShowDocument
        {
            get => new Commands.AsyncCommand(async (ChosenFile) =>
            {
                if (ChosenFile != null)
                {
                    XpsDocument oldXpsPackage = _xpsDocument;

                    string fileExt = Path.GetExtension((ChosenFile as Explorer).NameWithPath);

                    if (
                        fileExt 
                            is ".docx"
                            or ".doc"
                            or ".txt"
                            or ".cpp"
                            or ".h"
                            or ".py" 
                            or ".cs"
                            or ".json"
                            or ".xml"
                            or ".html"
                            or ".css"
                    )
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
                            IXPSConvert xpsConverter = default;

                            switch (fileExt)
                            {
                                case ".doc":
                                case ".docx":
                                    xpsConverter = _wordConverter;
                                    break;
                                case ".cpp":
                                case ".h":
                                case ".py":
                                case ".cs":
                                case ".json":
                                case ".xml":
                                case ".html":
                                case ".css":
                                case ".txt":
                                    xpsConverter = _txtConverter;
                                    break;
                            }

                            _xpsDocument = await xpsConverter?.ToXpsConvertAsync(
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

                        //Сигнализирует о том, что было выполнено преобразование в xps
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
                }
            });
        }
    }
}
