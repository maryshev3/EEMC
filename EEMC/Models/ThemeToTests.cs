using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public class ThemeToTests
    {
        public Theme Theme { get; set; }
        public Test[] Tests { get; set; }
        public bool IsChosenTheme { get; set; }
        public string CountString { get; set; }
        public int QuestionsCount 
        {
            get => Tests.Sum(x => x.Questions.Count);
        }
    }
}
