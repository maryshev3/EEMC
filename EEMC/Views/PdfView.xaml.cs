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
    /// Interaction logic for PdfView.xaml
    /// </summary>
    public partial class PdfView : Window
    {
        public PdfView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dc = this.DataContext as PdfViewVM;

            Uri uri = new Uri(Environment.CurrentDirectory + dc.ThemeFile.NameWithPath);

            webView.Source = uri;
        }
    }
}
