using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EEMC.ViewBases
{
    public enum ButtonType
    {
        CancelButton,
        ConfirmButton
    }

    public interface IHover
    {
        public Button _oldHoveredButton { get; set; }

        public void ResetButtonStyle(Button button)
        {
            button.Background = System.Windows.Media.Brushes.White;

            var text = button.Content as Label;
            text.Foreground = System.Windows.Media.Brushes.Black;
        }

        public void ConfirmHoverEffect(object sender, ButtonType buttonType)
        {
            if (_oldHoveredButton != default)
            {
                ResetButtonStyle(_oldHoveredButton);
            }

            Button button = sender as Button;

            button.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonType == ButtonType.ConfirmButton ? "#e0e7fd" : "#fdf4ef"));

            var text = (button.Content as Label);
            text.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(buttonType == ButtonType.ConfirmButton ? "#4b6cdf" : "#fd8958"));

            _oldHoveredButton = button;
        }
    }
}
