using System.Threading;
using System.Threading.Tasks;
using System.Windows.Xps.Packaging;

namespace EEMC.ToXPSConverteres
{
    public interface IXPSConvert
    {
        Task<XpsDocument> ToXpsConvertAsync(string originFileName, string xpsFileName, CancellationToken? cancellationToken = null);
    }
}
