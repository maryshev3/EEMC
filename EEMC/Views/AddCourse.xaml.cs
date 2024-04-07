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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EEMC.Views
{
    /// <summary>
    /// Interaction logic for AddCourse.xaml
    /// </summary>
    public partial class AddCourse : UserControl
    {
        public AddCourse()
        {
            InitializeComponent();
        }

        private Button _oldHoveredButton;

        private void ResetButtonStyle(Button button)
        {
            button.Background = System.Windows.Media.Brushes.White;

            var text = button.Content as Label;
            text.Foreground = System.Windows.Media.Brushes.Black;
        }

        private enum ButtonType
        {
            CancelButton,
            ConfirmButton
        }

        private void ConfirmHoverEffect(object sender, ButtonType buttonType)
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

            Cursor = Cursors.Hand;
        }

        private void Cancel_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            ConfirmHoverEffect(sender, ButtonType.CancelButton);
        }

        private void Cancel_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Add_Button_MouseLeave(sender, e);
        }

        private void Add_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            ConfirmHoverEffect(sender, ButtonType.ConfirmButton);
        }

        private void Add_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default)
            {
                ResetButtonStyle(_oldHoveredButton);
            }

            Cursor = Cursors.Arrow;
        }
    }
}
