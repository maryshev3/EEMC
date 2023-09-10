using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public class JSONCoursesParser
    {
        private List<Course> _courses = new List<Course>();

        public List<Course> Courses 
        {
            get => _courses;
        }

        public void Parse(string JSONPath) 
        {
            string JSONContent = File.ReadAllText(JSONPath);

            _courses = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Course>>(JSONContent);
        }
    }
}
