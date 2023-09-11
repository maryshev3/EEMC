using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    internal class CourseController
    {

        private IManager _manager;

        private List<Course> _courses;

        public List<Course> Courses => _courses;

        public CourseController(IManager Manager) 
        {
            _manager = Manager;

            try
            {
                _courses = _manager.Parse();
            }
            catch (Exception) 
            {
                _courses = new List<Course>();
            }
        }

        private Course? FindCourse(string CourseName) 
        {
            return _courses.Find((x) =>
            {
                return x.Name == CourseName;
            });
        }

        public void AddCourse(string CourseName) 
        {
            if (FindCourse(CourseName) != null)
                throw (new Exception("The course already exists"));

            _courses.Add(new Course() { Name = CourseName });

            _manager.Save(_courses);
        }

        public void RemoveCourse(string CourseName)
        {
            Course? FindedCourse = FindCourse(CourseName);

            if (FindedCourse == null)
                throw (new Exception("The course is not exist"));

            _courses.Remove(FindedCourse);

            _manager.Save(_courses);
        }
    }
}
