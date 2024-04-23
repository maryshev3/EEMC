using EEMC.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EEMC.ViewBases
{
    public interface IImageHover
    {
        public Button _oldHoveredButton { get; set; }

        private string GetHoveredUri(string uri)
        {
            StringBuilder result = new StringBuilder();

            result.Append(uri.Substring(0, uri.LastIndexOf('.')));
            result.Append("_hovered");
            result.Append(uri.Substring(uri.LastIndexOf('.')));

            return result.ToString();
        }

        private string GetOriginUri(string hoveredUri)
        {
            StringBuilder result = new StringBuilder();

            result.Append(hoveredUri.Substring(0, hoveredUri.LastIndexOf("_hovered")));
            result.Append(hoveredUri.Substring(hoveredUri.LastIndexOf('.')));

            return result.ToString();
        }

        private BitmapImage GetBitmap(string uri)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();

            return bitmap;
        }

        public void ResetButtonStyle(Button button)
        {
            (button.Content as Image).Source = GetBitmap(GetOriginUri((button.Content as Image).Source.ToString()));

            _oldHoveredButton = default;
        }

        public void ConfirmHoverEffect(object sender)
        {
            if (_oldHoveredButton != default)
            {
                ResetButtonStyle(_oldHoveredButton);
            }

            Button button = sender as Button;

            (button.Content as Image).Source = GetBitmap(GetHoveredUri((button.Content as Image).Source.ToString()));

            _oldHoveredButton = button;
        }
    }
}
