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
        public async Task<XpsDocument> ToXpsConvertAsync(string OriginFileName, string XPSFileName, CancellationToken? cancellationToken = null)
        {
            return await Task<XpsDocument>.Run(() =>
            {
                Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();

                Document doc = wordApplication.Documents.Open(OriginFileName, ReadOnly: true, Visible: false, Revert: false);
                doc.Activate();

                if (!wordApplication.Options.AlertIfNotDefault)
                {
                    throw new Exception("Для работы необходимо установить Word в качестве приложения по умолчанию");
                }

                
                doc.SaveAs(XPSFileName, WdSaveFormat.wdFormatXPS);


                doc.Close();
                wordApplication.Quit();

                XpsDocument xpsDoc = new XpsDocument(XPSFileName, System.IO.FileAccess.Read);

                return xpsDoc;
            }, cancellationToken ?? new CancellationToken()
            );
        }
    }
}
