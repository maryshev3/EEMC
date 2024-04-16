using EEMC.Models;
using EEMC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EEMC.Views
{
    /// <summary>
    /// Interaction logic for DocumentView.xaml
    /// </summary>
    public partial class DocumentView : Window
    {
        public DocumentView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = DocView.DataContext as DocumentViewVM;

            vm.ShowDocument.Execute(
                new Explorer(
                    vm.themeFile.Name,
                    vm.themeFile.NameWithPath.Substring(1),
                    ContentType.File,
                    null
                )
            );
        }
    }
}
