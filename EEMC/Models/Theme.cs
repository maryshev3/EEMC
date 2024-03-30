using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public class Theme
    {
        public string ThemeName { get; set; }
        public string ThemeDescription { get; set; }
        //Воспринимать как внешний ключ к Course
        public string CourseName { get; set; }
    }
}
