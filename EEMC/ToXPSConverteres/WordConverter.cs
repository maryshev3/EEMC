using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
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

                wordApplication.Documents.Add(OriginFileName);

                Document doc = wordApplication.ActiveDocument;

                try
                {
                    doc.SaveAs(XPSFileName, WdSaveFormat.wdFormatXPS);

                    wordApplication.Quit();

                    XpsDocument xpsDoc = new XpsDocument(XPSFileName, System.IO.FileAccess.Read);

                    return xpsDoc;
                }
                catch (Exception exp)
                {
                    string str = exp.Message;
                }

                return null;
            }, cancellationToken
            );
        }
    }
}
