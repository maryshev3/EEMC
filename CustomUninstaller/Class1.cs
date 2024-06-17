using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomUninstaller
{
    [RunInstaller(true)]
    public class MyUninstaller : System.Configuration.Install.Installer
    {
        public override void Uninstall(IDictionary savedState)
        {
            if (savedState != null)
                base.Uninstall(savedState);

            string currentPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string programPath = Path.GetFullPath(Path.Combine(currentPath, @"..\..\"));

            string[] files = Directory.GetFiles(programPath);

            // Сейчас находимся в директории установленной программы. Необходимо удалить все файлы, созданные не инсталлером.
            // В их число входят xps файлы в каталоге с программой. Также всё содержимое папки "Курсы". Саму папку "Курсы" удалять не надо.
            var filesToDelete = files.Where(
                x => Path.GetExtension(x) == ".xps"
                    || Path.GetExtension(x) == ".ctt"
                    || Path.GetExtension(x) == ".ttt"
            );

            var coursesPath = Path.Combine(programPath, "Курсы");

            string[] courses = Directory.GetDirectories(coursesPath);

            foreach (var course in courses)
                Directory.Delete(course, true);

            foreach (var file in filesToDelete)
                File.Delete(file);

            //Удаляем файлы тем, курсов и конвертированные
            var themesPath = Path.Combine(programPath, "Файлы тем");
            if (Directory.Exists(themesPath))
                Directory.Delete(themesPath, true);

            var themesConvertedPath = Path.Combine(programPath, "Файлы тем конвертированные");
            if (Directory.Exists(themesConvertedPath))
                Directory.Delete(themesConvertedPath, true);

            var coursesConvertedPath = Path.Combine(programPath, "Курсы конвертированные");
            if (Directory.Exists(coursesConvertedPath))
                Directory.Delete(coursesConvertedPath, true);
        }
    }
}
