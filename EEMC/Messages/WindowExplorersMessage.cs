using EEMC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EEMC.Messages
{
    public class WindowExplorersMessage : IMessage
    {
        public Window? Window { get; set; }
        public List<Explorer>? ExplorerList { get; set; }

        public WindowExplorersMessage(Window? window, List<Explorer>? explorerList)
        {
            Window = window;
            ExplorerList = explorerList;
        }
    }
}
