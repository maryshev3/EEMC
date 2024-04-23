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
            ".docx",
            ".doc",
            ".txt",
            ".cpp",
            ".h",
            ".py",
            ".cs",
            ".json",
            ".xml",
            ".html",
            ".css",
            ".ppt",
            ".pptx",
            ".mp4"
        };

        [JsonIgnore]
        private static Dictionary<string, string> _filtersMap = new()
        {
            { ".docx", "Word 2007+ file | *.docx" },
            { ".doc", "Word 2007- file | *.doc" },
            { ".txt", "Text file | *.txt" },
            { ".cpp", "CPP source file | *.cpp" },
            { ".h", "CPP header file | *.h" },
            { ".py", "Python source file | *.py" },
            { ".cs", "C# source file | *.cs" },
            { ".json", "JSON file | *.json" },
            { ".xml", "XML file | *.xml" },
            { ".html", "HTML file | *.html" },
            { ".css", "CSS file | *.css" },
            { ".ppt", "PowerPoint 2007- file | *.ppt" },
            { ".pptx", "PowerPoint 2007+ file | *.pptx" },
            { ".mp4", "Video mp4 file | *.mp4" }
        };

        public string Name { get; set; }
        public string NameWithPath { get; set; }

        public bool IsSupportedExtension()
        {
            string extension = Path.GetExtension(Name);

            return _supportedExtensions.Contains(extension);
        }

        public bool IsVideoOrAudio()
        {
            string extension = Path.GetExtension(Name);

            return extension is ".mp4" or ".jpg" or ".jpeg" or ".png";
        }

        public string GetSaveFilter()
        {
            string extension = Path.GetExtension(Name);

            return _filtersMap.ContainsKey(extension) ? _filtersMap[extension] : " | *" + extension;
        }

        public void SaveFile(string savePath)
        {
            if (savePath.Contains(Environment.CurrentDirectory))
                throw new Exception("Не допускается сохранение в дирректорию программы");

            string filePath = Environment.CurrentDirectory + NameWithPath;

            byte[] file = File.ReadAllBytes(filePath);

            File.WriteAllBytes(savePath, file);
        }

        public void RemoveFile()
        {
            var allThemes = Theme.ReadAllThemes();

            var thisTheme = allThemes.First(
                x => 
                    x.Files != default
                    && x
                        .Files
                        .Select(x => x.NameWithPath)
                        .ToHashSet()
                        .Contains(NameWithPath)
            );

            thisTheme.Files = new(thisTheme.Files.Where(x => x.NameWithPath != NameWithPath));

            Theme.RewriteAllThemes(allThemes);

            File.Delete(Environment.CurrentDirectory + NameWithPath);
        }
    }
}
