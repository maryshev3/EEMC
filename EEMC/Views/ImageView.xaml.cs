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
    /// Interaction logic for ImageView.xaml
    /// </summary>
    public partial class ImageView : Window
    {
        public ImageView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dc = ImgWindow.DataContext as ImageViewVM;

            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(Environment.CurrentDirectory + dc.ThemeFile.NameWithPath);
            image.EndInit();

            ChosenImage.Source = image;
        }
    }
}
