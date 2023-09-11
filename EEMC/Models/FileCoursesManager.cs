using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public class FileCoursesManager : IManager
    {
        public List<Course> Parse()
        {
            string FolderCourses = Path.Combine(Environment.CurrentDirectory, "Курсы");

            string? FoundedFolderCourses = Array.Find(Directory.GetDirectories(Environment.CurrentDirectory), (x) =>
                {
                    return x == FolderCourses;
                });

            if (FoundedFolderCourses == null)
            {
                Directory.CreateDirectory(FolderCourses);

                return new List<Course>();
            }

            return new List<Course>();
        }

        public void Save(List<Course> Courses)
        {
            throw new NotImplementedException();
        }
    }
}
