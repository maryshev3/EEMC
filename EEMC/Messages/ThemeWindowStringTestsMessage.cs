using EEMC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EEMC.Messages
{
    public class ThemeWindowStringTestsMessage : IMessage
    {
        public Window? Window { get; set; }
        public Theme? Theme { get; set; }
        public string String { get; set; }
        public ThemeToTests[]? ThemeToTests { get; set; }

        public ThemeWindowStringTestsMessage(Window? window, Theme? theme, string @string, ThemeToTests[]? themeToTests)
        {
            Window = window;
            Theme = theme;
            String = @string;
            ThemeToTests = themeToTests;
        }
    }
}
