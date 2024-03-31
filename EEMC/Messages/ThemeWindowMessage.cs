using EEMC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EEMC.Messages
{
    public class ThemeWindowMessage : IMessage
    {
        public Window? Window { get; set; }
        public Theme? Theme { get; set; }

        public ThemeWindowMessage(Window? window, Theme? theme)
        {
            Window = window;
            Theme = theme;
        }
    }
}
