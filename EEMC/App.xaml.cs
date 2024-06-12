using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EEMC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.Sleep(1000);

            try
            {
                string currentPath = Environment.CurrentDirectory;
                string coursePath = Path.Combine(currentPath, "Курсы");

                if (!Directory.Exists(coursePath))
                    Directory.CreateDirectory(coursePath);
            }
            catch
            {
                MessageBox.Show("Вы установили программу в папку, где требуются права администраторы для различных действий. В этом случае необходимо запустить программу от имени администратора");
                return;
            }

            ViewModelLocator.Init();

            base.OnStartup(e);
        }
    }
}
