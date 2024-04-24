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
        public async Task<XpsDocument> ToXpsConvertAsync(string originFileName, string xpsFileName, CancellationToken? cancellationToken = null)
        {
            return await Task<XpsDocument>.Run(() =>
            {
                Application pptApplication = new Microsoft.Office.Interop.PowerPoint.Application();

                Presentation pres = pptApplication.Presentations.Open(originFileName, ReadOnly: Microsoft.Office.Core.MsoTriState.msoTrue, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
                
                pres.SaveAs(xpsFileName, PpSaveAsFileType.ppSaveAsXPS);

                pres.Close();
                pptApplication.Quit();

                XpsDocument xpsDoc = new XpsDocument(xpsFileName, System.IO.FileAccess.Read);

                return xpsDoc;
            }, cancellationToken ?? new CancellationToken()
            );
        }
    }
}
