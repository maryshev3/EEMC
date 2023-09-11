using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public static class ExplorerBuilder
    {
        public static Explorer Build(string ExplorerPath) 
        {
            Explorer explorer = new Explorer();

            explorer.Name = ExplorerPath.Split(Path.DirectorySeparatorChar).Last();
            explorer.Type = explorer.Name.Contains('.') ? ContentType.File : ContentType.Folder;

            explorer.Content = new ObservableCollection<Explorer>();

            //Заполняем папки
            string[] directories = Directory.GetDirectories(ExplorerPath);
            foreach (string directory in directories)
                explorer.Content.Add(Build(directory));

            //Заполняем файлы
            string[] content = Directory.GetFiles(ExplorerPath);
            foreach (string file in content) 
                explorer.Content.Add(new Explorer() { Name = Path.GetFileName(file) });

            return explorer;
        }
    }
}
