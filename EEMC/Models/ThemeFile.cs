﻿using Newtonsoft.Json;
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

        [JsonIgnore]
        private static Dictionary<string, string> _filtersMap = new()
        {
            {".docx", "Word 2007+ file | *.docx"}
        };

        public string Name { get; set; }
        public string NameWithPath { get; set; }

        public bool IsSupportedExtension()
        {
            string extension = Path.GetExtension(Name);

            return _supportedExtensions.Contains(extension);
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
    }
}
