using System.Threading.Tasks;
using System.Windows.Input;

namespace EEMC.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
