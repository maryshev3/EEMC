using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Button _oldPressedButton;
        private Button _oldHoveredButton;

        public void UpdateChosenCourse(string courseName)
        {
            if (_oldPressedButton != default)
            {
                ResetButtonStyle(_oldPressedButton);
            }

            for (int i = 0; i < CoursesList.Items.Count; i++)
            {
                ContentPresenter c = (ContentPresenter)CoursesList.ItemContainerGenerator.ContainerFromItem(CoursesList.Items[i]);
                Button button = c.ContentTemplate.FindName("CourseButton", c) as Button;

                var text = (button.Content as StackPanel).Children.OfType<Label>().First();
                
                if (text.Content as string == courseName)
                {
                    button.BorderThickness = new Thickness() { Left = 3 };
                    button.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4b6cdf"));

                    text.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4b6cdf"));

                    _oldPressedButton = button;

                    break;
                }
            }
        }

        private void ResetButtonStyle(Button button) 
        {
            if (button.BorderThickness.Bottom == 0)
            {
                button.BorderThickness = new Thickness() { Left = 0 };
                button.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#efeff5"));
            }

            if (button.Content is StackPanel)
            {
                var text = (button.Content as StackPanel).Children.OfType<Label>().First();
                text.Foreground = System.Windows.Media.Brushes.Black;
            }
            else
            {
                var text = (button.Content as Label);
                text.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void CourseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_oldPressedButton != default)
            {
                ResetButtonStyle(_oldPressedButton);
            }

            Button button = sender as Button;

            button.BorderThickness = new Thickness() { Left = 3 };
            button.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4b6cdf"));

            var text = (button.Content as StackPanel).Children.OfType<Label>().First();
            text.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4b6cdf"));

            _oldPressedButton = button;
        }

        private void CourseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default && _oldHoveredButton != _oldPressedButton)
            {
                ResetButtonStyle(_oldHoveredButton);
            }

            Button button = sender as Button;

            if (button.Content is StackPanel)
            {
                var text = (button.Content as StackPanel).Children.OfType<Label>().First();
                text.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4b6cdf"));
            }
            else
            {
                var text = (button.Content as Label);
                text.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4b6cdf"));
            }

            _oldHoveredButton = button;
        }

        private void CourseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default && _oldHoveredButton != _oldPressedButton)
            {
                ResetButtonStyle(_oldHoveredButton);
            }
        }

        private void ContextButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            Button button = sender as Button;

            button.ContextMenu.IsOpen = true;
        }

        private void ContextButton_MouseEnter(object sender, MouseEventArgs e)
        {
            e.Handled = true;

            if (_oldHoveredButton != default && _oldHoveredButton != _oldPressedButton)
            {
                ResetButtonStyle(_oldHoveredButton);
            }

            Cursor = Cursors.Hand;
        }

        private void ContextButton_MouseLeave(object sender, MouseEventArgs e)
        {
            e.Handled = true;

            Cursor = Cursors.Arrow;
        }

        private void AddMainButton_MouseEnter(object sender, MouseEventArgs e)
        {
            CourseButton_MouseEnter(sender, e);

            Cursor = Cursors.Hand;
        }

        private void AddMainButton_MouseLeave(object sender, MouseEventArgs e)
        {
            CourseButton_MouseLeave(sender, e);

            Cursor = Cursors.Arrow;
        }
    }
}