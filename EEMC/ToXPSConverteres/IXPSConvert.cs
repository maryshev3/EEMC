using System.Threading.Tasks;
using System.Windows.Xps.Packaging;

namespace EEMC.ToXPSConverteres
{
    interface IXPSConvert
    {
        Task<XpsDocument> ToXpsConvertAsync(string OriginFileName, string XPSFileName);
    }
}
