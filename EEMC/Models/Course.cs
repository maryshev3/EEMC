using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public class Course
    {
        private ObservableCollection<Explorer>? _courses;

        public Course(ObservableCollection<Explorer>? Courses) 
        {
            _courses = Courses;
        }

        public IEnumerable<Explorer> Courses 
        {
            get => _courses;
        }
    }
}
