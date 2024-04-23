using EEMC.ViewBases;
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
    /// Interaction logic for VideoView.xaml
    /// </summary>
    public partial class VideoView : Window, IImageHover
    {
        public Button _oldHoveredButton { get; set; }

        public VideoView()
        {
            InitializeComponent();

            _playIcon = new BitmapImage();
            _playIcon.BeginInit();
            _playIcon.UriSource = new Uri("pack://application:,,,/Resources/play_icon.png", UriKind.RelativeOrAbsolute);
            _playIcon.EndInit();

            _pauseIcon = new BitmapImage();
            _pauseIcon.BeginInit();
            _pauseIcon.UriSource = new Uri("pack://application:,,,/Resources/pause_icon.png", UriKind.RelativeOrAbsolute);
            _pauseIcon.EndInit();

            _playHoveredIcon = new BitmapImage();
            _playHoveredIcon.BeginInit();
            _playHoveredIcon.UriSource = new Uri("pack://application:,,,/Resources/play_icon_hovered.png", UriKind.RelativeOrAbsolute);
            _playHoveredIcon.EndInit();

            _pauseHoveredIcon = new BitmapImage();
            _pauseHoveredIcon.BeginInit();
            _pauseHoveredIcon.UriSource = new Uri("pack://application:,,,/Resources/pause_icon_hovered.png", UriKind.RelativeOrAbsolute);
            _pauseHoveredIcon.EndInit();
        }

        private BitmapImage _playIcon;
        private BitmapImage _pauseIcon;
        private BitmapImage _playHoveredIcon;
        private BitmapImage _pauseHoveredIcon;

        private bool _isPlaying;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VideoViewVM thisDc = VideoWindow.DataContext as VideoViewVM;

            VideoElement.Source = new Uri(Environment.CurrentDirectory + thisDc.ThemeFile.NameWithPath);

            Image image = ManipulateButton.Content as Image;
            image.Source = _pauseIcon;


            VideoElement.Play();
            _isPlaying = true;
        }

        private void ManipulateButton_Click(object sender, RoutedEventArgs e)
        {
            Image image = ManipulateButton.Content as Image;

            if (_isPlaying)
            {
                image.Source = _playHoveredIcon;

                VideoElement.Pause();

                _isPlaying = false;
            }
            else
            {
                image.Source = _pauseHoveredIcon;

                VideoElement.Play();

                _isPlaying = true;
            }
        }

        private void ManipulateButton_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as IImageHover).ConfirmHoverEffect(sender);

            Cursor = Cursors.Hand;
        }

        private void ManipulateButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default)
            {
                (this as IImageHover).ResetButtonStyle(_oldHoveredButton);
            }

            Cursor = Cursors.Arrow;
        }
    }
}
