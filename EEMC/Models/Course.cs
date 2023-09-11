using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public class Course
    {
        public string Name = "";

        private List<string> _fileContent;

        public List<string> FileContent 
        {
            get
            {
                List<string> FileContentFullPath = new List<string>(_fileContent);

                for (int i = 0; i < _fileContent.Count; i++)
                    FileContentFullPath[i] = Path.Combine(Environment.CurrentDirectory, _fileContent[i]);

                return FileContentFullPath;
            }
        }

        public void AddFile(string FileName) 
        {
            _fileContent.Add(FileName);
        }

        public List<Course> SubCourses;
    }
}
