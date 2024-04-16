using EEMC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Messages
{
    public class ThemeFileMessage : IMessage
    {
        public ThemeFile? ThemeFile { get; set; }

        public ThemeFileMessage(ThemeFile? themeFile)
        {
            ThemeFile = themeFile;
        }
    }
}
