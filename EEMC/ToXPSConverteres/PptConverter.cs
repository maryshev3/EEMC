using Microsoft.Office.Interop.PowerPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Xps.Packaging;

namespace EEMC.ToXPSConverteres
{
    public class PptConverter : IXPSConvert
    {
        public async Task<XpsDocument> ToXpsConvertAsync(string OriginFileName, string XPSFileName, CancellationToken cancellationToken)
        {
            return await Task<XpsDocument>.Run(() =>
            {
                Application pptApplication = new Microsoft.Office.Interop.PowerPoint.Application();

                Presentation pres = pptApplication.Presentations.Open(OriginFileName, ReadOnly: Microsoft.Office.Core.MsoTriState.msoTrue, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                
                pres.SaveAs(XPSFileName, PpSaveAsFileType.ppSaveAsXPS);

                pres.Close();
                pptApplication.Quit();

                XpsDocument xpsDoc = new XpsDocument(XPSFileName, System.IO.FileAccess.Read);

                return xpsDoc;
            }, cancellationToken
            );
        }
    }
}
