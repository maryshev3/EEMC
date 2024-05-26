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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EEMC.Views
{
    /// <summary>
    /// Interaction logic for SwitcherCourseView.xaml
    /// </summary>
    public partial class SwitcherCourseView : Page
    {
        public SwitcherCourseView()
        {
            InitializeComponent();
        }

        public enum SwitcherTypes
        {
            Materials,
            Lessons
        }

        public void UpdateChosenSwitcher(SwitcherTypes type)
        {
            if (_oldPressedButton != default)
            {
                ResetButtonStyle(_oldPressedButton);
            }

            Button button = SwitcherButtons.Children[(type == SwitcherTypes.Materials) ? 1 : 0] as Button;

            var text = button.Content as Label;

            button.BorderThickness = new Thickness() { Bottom = 3 };
            button.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4b6cdf"));

            text.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4b6cdf"));

            _oldPressedButton = button;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = SwitcherPage.DataContext as SwitcherCourseViewVM;

            if (vm != null)
            {
                if (vm.CurrentPage == null || vm.CurrentPage is ThemesWindow)
                {
                    vm.OpenThemesWindow().Wait();
                    UpdateChosenSwitcher(SwitcherTypes.Lessons);
                }
                else
                {
                    vm.OpenCourseWindow().Wait();
                    UpdateChosenSwitcher(SwitcherTypes.Materials);
                }
            }
        }

        private Button _oldPressedButton;
        private Button _oldHoveredButton;

        private void ResetButtonStyle(Button button)
        {
            button.BorderThickness = new Thickness() { Bottom = 0 };
            button.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#efeff5"));

            var text = button.Content as Label;
            text.Foreground = System.Windows.Media.Brushes.Black;
        }

        private void LessonsButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default && _oldHoveredButton != _oldPressedButton)
            {
                ResetButtonStyle(_oldHoveredButton);
            }

            Button button = sender as Button;

            var text = (button.Content as Label);
            text.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4b6cdf"));

            _oldHoveredButton = button;
        }

        private void LessonsButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default && _oldHoveredButton != _oldPressedButton)
            {
                ResetButtonStyle(_oldHoveredButton);
            }
        }

        private void MaterialButton_MouseEnter(object sender, MouseEventArgs e)
        {
            LessonsButton_MouseEnter(sender, e);
        }

        private void MaterialButton_MouseLeave(object sender, MouseEventArgs e)
        {
            LessonsButton_MouseLeave(sender, e);
        }

        private void MaterialButton_Click(object sender, RoutedEventArgs e)
        {
            LessonsButton_Click(sender, e);
        }

        private void LessonsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_oldPressedButton != default)
            {
                ResetButtonStyle(_oldPressedButton);
            }

            Button button = sender as Button;

            button.BorderThickness = new Thickness() { Bottom = 3 };
            button.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4b6cdf"));

            var text = button.Content as Label;
            text.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4b6cdf"));

            _oldPressedButton = button;
        }
    }
}
