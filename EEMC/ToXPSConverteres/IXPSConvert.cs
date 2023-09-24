using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps.Packaging;

namespace EEMC.ToXPSConverteres
{
    interface IXPSConvert
    {
        Task<XpsDocument> ToXpsConvert(string OriginFileName, string XPSFileName);
    }
}
