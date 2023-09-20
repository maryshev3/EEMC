using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps.Packaging;

namespace EEMC.ToXPSConverteres
{
    public class WordConverter : IXPSConvert
    {
        public XpsDocument ToXpsConvert(string OriginFileName, string XPSFileName)
        {
            Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();

            wordApplication.Documents.Add(OriginFileName);

            Document doc = wordApplication.ActiveDocument;

            try
            {
                doc.SaveAs(XPSFileName, WdSaveFormat.wdFormatXPS);

                wordApplication.Quit();

                return new XpsDocument(XPSFileName, System.IO.FileAccess.Read);
            }
            catch (Exception exp)
            {
                string str = exp.Message;
            }

            return null;
        }
    }
}
