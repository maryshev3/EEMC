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
        AddButton,
        ChangeButton,
        RemoveButton,
        CourseButton
    }

    public class ColorSettings
    {
        public string Background { get; set; }
        public string Foreground { get; set; }

        public ColorSettings(ButtonType buttonType) 
        {
            switch(buttonType)
            {
                case ButtonType.CancelButton:
                    Background = "#f3f3f6";
                    Foreground = "#9a9eb1";
                    break;
                case ButtonType.AddButton:
                    Background = "#e9f5ef";
                    Foreground = "#5ac18f";
                    break;
                case ButtonType.ChangeButton:
                    Background = "#fdf4ef";
                    Foreground = "#fd8958";
                    break;
                case ButtonType.RemoveButton:
                    Background = "#fcdcdc";
                    Foreground = "#f55b5b";
                    break;
                case ButtonType.CourseButton:
                    Background = "#e0e7fd";
                    Foreground = "#4b6cdf";
                    break;
            }
        }
    }

    public interface ITextHover
    {
        public Button _oldHoveredButton { get; set; }

        public void ResetButtonStyle(Button button)
        {
            button.Background = System.Windows.Media.Brushes.White;

            if (button.Content is Label)
            {
                var text = button.Content as Label;
                text.Foreground = System.Windows.Media.Brushes.Black;
            }
            else
            {
                var stackPanel = button.Content as StackPanel;

                foreach (var child in stackPanel.Children)
                {
                    if (child is Label)
                    {
                        Label el = child as Label;
                        el.Background = System.Windows.Media.Brushes.White;
                        el.Foreground = System.Windows.Media.Brushes.Black;
                    }
                }
            }

            _oldHoveredButton = default;
        }

        public void ConfirmHoverEffect(object sender, ButtonType buttonType)
        {
            if (_oldHoveredButton != default)
            {
                ResetButtonStyle(_oldHoveredButton);
            }

            Button button = sender as Button;
            ColorSettings colorSettings = new ColorSettings(buttonType);

            button.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorSettings.Background));

            var text = (button.Content as Label);
            text.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorSettings.Foreground));

            _oldHoveredButton = button;
        }
    }
}
