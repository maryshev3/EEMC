using EEMC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EEMC.Messages
{
    public class ThemeToTestsWindowThemeMessage : IMessage
    {
        public ThemeToTests[]? ThemeToTests { get; set; }
        public Window? Window { get; set; }
        public Theme? Theme { get; set; }

        public ThemeToTestsWindowThemeMessage(ThemeToTests[]? themeToTests, Theme? theme, Window window) 
        {
            ThemeToTests = themeToTests;
            Theme = theme;
            Window = window;
        }
    }
}
