using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Xps.Packaging;

namespace EEMC.ToXPSConverteres
{
    public class WordConverter : IXPSConvert
    {
        public async Task<XpsDocument> ToXpsConvertAsync(string OriginFileName, string XPSFileName, CancellationToken cancellationToken)
        {
            return await Task<XpsDocument>.Run(() =>
            {
                Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();

                wordApplication.Documents.Open(OriginFileName, ReadOnly: true, Visible: false, Revert: false);

                if (wordApplication.Options.AlertIfNotDefault)
                {
                    throw new Exception("Для работы необходимо установить Word в качестве приложения по умолчанию");
                }

                Document doc = wordApplication.ActiveDocument;

                doc.SaveAs(XPSFileName, WdSaveFormat.wdFormatXPS);

                Marshal.ReleaseComObject(doc);
                Marshal.ReleaseComObject(wordApplication);

                wordApplication.Documents.Close(SaveChanges: false);
                wordApplication.Quit();

                XpsDocument xpsDoc = new XpsDocument(XPSFileName, System.IO.FileAccess.Read);

                return xpsDoc;
            }, cancellationToken
            );
        }
    }
}
