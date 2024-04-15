using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public class ThemeFile
    {
        [JsonIgnore]
        private static HashSet<string> _supportedExtensions =  new() 
        {
            ".docx"
        };

        public string Name { get; set; }
        public string NameWithPath { get; set; }

        public bool IsSupportedExtension()
        {
            string extension = Path.GetExtension(Name);

            return _supportedExtensions.Contains(extension);
        }
    }
}
