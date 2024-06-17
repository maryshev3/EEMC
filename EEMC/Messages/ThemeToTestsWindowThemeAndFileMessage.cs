using EEMC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EEMC.Messages
{
    public class ThemeToTestsWindowThemeAndFileMessage : IMessage
    {
        public ThemeToTests[]? ThemeToTests { get; set; }
        public Window? Window { get; set; }
        public Theme? Theme { get; set; }
        public ThemeFile? ThemeFile { get; set; }

        public ThemeToTestsWindowThemeAndFileMessage(ThemeToTests[]? themeToTests, Theme? theme, Window window, ThemeFile? themeFile)
        {
            ThemeToTests = themeToTests;
            Theme = theme;
            Window = window;
            ThemeFile = themeFile;
        }
    }
}
