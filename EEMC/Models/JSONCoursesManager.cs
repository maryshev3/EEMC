using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public class JSONCoursesManager
    {
        private string _JSONPath;

        public JSONCoursesManager(string JSONPath) 
        {
            _JSONPath = JSONPath;
        }

        public List<Course> Parse() 
        {
            string JSONContent = File.ReadAllText(_JSONPath);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Course>>(JSONContent);
        }

        public void Save(List<Course> Courses) 
        {
            string JSONContent = Newtonsoft.Json.JsonConvert.SerializeObject(Courses);

            File.WriteAllText(_JSONPath, JSONContent);
        }
    }
}
