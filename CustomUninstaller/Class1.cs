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
            // Upd: необходимо удалять ещё папку EEMC.exe.WebView2
            var filesToDelete = files.Where(x => Path.GetExtension(x) == ".xps");

            var coursesPath = Path.Combine(programPath, "Курсы");

            string[] courses = Directory.GetDirectories(coursesPath);

            foreach (var course in courses)
                Directory.Delete(course, true);

            foreach (var file in filesToDelete)
                File.Delete(file);

            var webViewPath = Path.Combine(programPath, "EEMC.exe.WebView2");
            Directory.Delete(webViewPath, true);
        }
    }
}
