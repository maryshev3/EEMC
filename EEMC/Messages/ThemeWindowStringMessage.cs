using EEMC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EEMC.Messages
{
    class ThemeWindowStringMessage : IMessage
    {
        public Window? Window { get; set; }
        public Theme? Theme { get; set; }
        public string String { get; set; }

        public ThemeWindowStringMessage(Window? window, Theme? theme, string @string)
        {
            Window = window;
            Theme = theme;
            String = @string;
        }
    }
}
