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
using System.Windows.Xps.Packaging;

namespace EEMC.Views
{
    /// <summary>
    /// Interaction logic for GuidTheme.xaml
    /// </summary>
    public partial class GuidTheme : Window
    {
        public GuidTheme()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            XpsDocument xpsDocument = new XpsDocument("./Guids/guid_for_theme.xps", System.IO.FileAccess.Read);
            
            DocViewer.Document = xpsDocument.GetFixedDocumentSequence();

            xpsDocument.Close();
        }
    }
}
