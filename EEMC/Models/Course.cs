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

        public Course() 
        {
            _courses = CourseBuilder.Build(new ExplorerBuilder())._courses;

        }

        public Course(ObservableCollection<Explorer>? Courses) 
        {
            _courses = Courses;
        }

        public IEnumerable<Explorer> Courses 
        {
            get => _courses;
        }

        public ObservableCollection<Explorer>? GetCourseContent(string CourseName) 
        {
            return Courses.First(x => x.Name == CourseName).Content;
        }
    }
}
