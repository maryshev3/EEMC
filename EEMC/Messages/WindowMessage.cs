using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EEMC.Messages
{
    public class WindowMessage : IMessage
    {
        public Window? Window { get; set; }

        public WindowMessage(Window? window)
        {
            Window = window;
        }
    }
}
