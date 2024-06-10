using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Services
{
    //Костыль для того, чтобы скролл не сбрасывался при любой модификации темы
    public static class ScrollSaver
    {
        public static string CurrentCourseName { get; set; }
        public static double ScrollPosition { get; set; }
    }
}
