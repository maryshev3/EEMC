using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public class ExplorerBuilder
    {
        public static Explorer Build(string ExplorerPath) 
        {
            string ExpName = ExplorerPath.Split(Path.DirectorySeparatorChar).Last();
            Explorer explorer = new Explorer
                (
                    ExplorerPath.Split(Path.DirectorySeparatorChar).Last(),
                    ExplorerPath.Remove(0, Environment.CurrentDirectory.Length),
                    ExpName.Contains('.') ? ContentType.File : ContentType.Folder,
                    new ObservableCollection<Explorer>()
                );

            //Заполняем папки
            string[] directories = Directory.GetDirectories(ExplorerPath);
            foreach (string directory in directories)
                explorer.Content.Add(Build(directory));

            //Заполняем файлы
            string[] content = Directory.GetFiles(ExplorerPath);
            foreach (string file in content)
                explorer.Content.Add
                    (
                        new Explorer
                        (
                            Path.GetFileName(file),
                            file.Remove(0, Environment.CurrentDirectory.Length),
                            ContentType.File,
                            null
                        )
                    );

            return explorer;
        }
    }
}
