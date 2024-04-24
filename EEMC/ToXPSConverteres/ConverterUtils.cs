using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.ToXPSConverteres
{
    public class ConverterUtils
    {
        private readonly WordConverter _wordConverter;
        private readonly TxtConverter _txtConverter;
        private readonly PptConverter _pptConverter;

        public ConverterUtils(WordConverter wordConverter, TxtConverter txtConverter, PptConverter pptConverter)
        {
            _wordConverter = wordConverter;
            _txtConverter = txtConverter;
            _pptConverter = pptConverter;
        }

        public IXPSConvert GetInstanceByFileExtension(string fileExt)
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
                case ".ppt":
                case ".pptx":
                    xpsConverter = _pptConverter;
                    break;
            }

            return xpsConverter;
        }
    }
}
