using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public class Course
    {
        [Newtonsoft.Json.JsonProperty("name")]
        public string Name = "";
    }
}
