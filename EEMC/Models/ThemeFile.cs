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
            ".mp4",
            ".mp3",
            ".bmp",
            ".jpg",
            ".jpeg",
            ".png",
            ".tiff",
            ".gif",
            ".icon",
            ".pdf",
            ".ctt",
            ".ttt"
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
            { ".mp4", "Video mp4 file | *.mp4" },
            { ".mp3", "Audio mp3 file | *.mp3" },
            { ".bmp", "Image BMP file | *.bmp" },
            { ".jpeg", "Image JPEG file | *.jpeg" },
            { ".jpg", "Image JPG file | *.jpg" },
            { ".png", "Image PNG file | *.png" },
            { ".tiff", "Image TIFF file | *.tiff" },
            { ".gif", "GIF file | *.gif" },
            { ".icon", "Image ICON file | *.icon" },
            { ".pdf", "PDF file | *.pdf" },
            { ".ctt", "Course theme test file | *.ctt" },
            { ".ttt", "Total theme test file | *.ttt" }
        };

        public string Name { get; set; }
        public string NameWithoutExtension 
        {
            get => Path.GetFileNameWithoutExtension(Name);
        }
        public string NameWithPath { get; set; }
        public string ImagePath
        {
            get
            {
                if (IsTest())
                    return "/Resources/test_icon.png";

                if (IsTotalTest())
                    return "/Resources/total_test_icon.png";

                return "/Resources/document_icon.png";
            }
        }

        public bool IsSupportedExtension()
        {
            string extension = Path.GetExtension(Name).ToLower();

            return _supportedExtensions.Contains(extension);
        }

        public bool IsVideoOrAudio()
        {
            string extension = Path.GetExtension(Name).ToLower();

            return extension is ".mp4" or ".mp3";
        }

        public bool IsText()
        {
            string extension = Path.GetExtension(Name).ToLower();

            return extension is ".docx" or ".doc" or ".txt" or ".cpp" or ".h" or ".py"
                or ".cs" or ".json" or ".xml" or ".html" or ".css" or ".ppt" or ".pptx";
        }

        public bool IsPdf()
        {
            string extension = Path.GetExtension(Name).ToLower();

            return extension == ".pdf";
        }

        public bool IsImage()
        {
            string extension = Path.GetExtension(Name).ToLower();

            return extension is ".bmp" or ".jpeg" or ".jpg" or ".png" or ".tiff" or ".gif" or ".icon";
        }

        public bool IsTotalTest()
        {
            string extension = Path.GetExtension(Name).ToLower();

            return extension == ".ttt";
        }

        public bool IsTest()
        {
            string extension = Path.GetExtension(Name).ToLower();

            return extension == ".ctt";
        }

        public string GetSaveFilter()
        {
            string extension = Path.GetExtension(Name).ToLower();

            return _filtersMap.ContainsKey(extension) ? _filtersMap[extension] : " | *" + extension;
        }

        public void SaveFile(string savePath, bool isServiceMode = false)
        {
            if (savePath.Contains(Environment.CurrentDirectory) && !isServiceMode)
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
