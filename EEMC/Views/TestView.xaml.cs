using EEMC.Models;
using EEMC.ViewBases;
using EEMC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class TestView : Window
    {
        private Button _oldPressedButton;
        private Button _oldHoveredButton;

        public TestView()
        {
            InitializeComponent();
        }

        private void ResetButtonStyle(Button button)
        {
            if (button.BorderThickness.Bottom == 0)
            {
                button.BorderThickness = new Thickness() { Left = 0 };
                button.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#efeff5"));
            }

            button.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#efeff5"));

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

        private void Question_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            HoverEffect(sender, ButtonType.CourseButton);

            Cursor = Cursors.Hand;
        }

        private void Question_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default && _oldHoveredButton != _oldPressedButton)
            {
                ResetButtonStyle(_oldHoveredButton);

                Cursor = Cursors.Arrow;
            }
        }

        private void Question_Button_Click(object sender, RoutedEventArgs e)
        {
            //Актуализируем richTextBox и открываем вопрос
            var dc = this.DataContext as TestViewVM;

            dc.OpenQuestion.Execute((sender as Button).DataContext as Question);

            richTextBox.Document = dc.SelectedQuestion.QuestionText;

            //Применяем стиль
            if (_oldPressedButton != default)
            {
                ResetButtonStyle(_oldPressedButton);
            }

            Button button = sender as Button;

            button.BorderThickness = new Thickness() { Left = 3 };
            button.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4b6cdf"));

            ColorSettings colorSettings = new ColorSettings(ButtonType.CourseButton);

            var text = (button.Content as StackPanel).Children.OfType<Label>().First();
            text.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorSettings.Foreground));

            _oldPressedButton = button;
        }

        private void HoverEffect(object sender, ButtonType buttonType)
        {
            if (_oldHoveredButton != default && _oldHoveredButton != _oldPressedButton)
            {
                ResetButtonStyle(_oldHoveredButton);
            }

            Button button = sender as Button;

            ColorSettings colorSettings = new ColorSettings(buttonType);
            button.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorSettings.Background));

            if (button.Content is StackPanel)
            {
                var text = (button.Content as StackPanel).Children.OfType<Label>().First();
                text.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorSettings.Foreground));
            }
            else
            {
                var text = (button.Content as Label);
                text.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorSettings.Foreground));
            }

            _oldHoveredButton = button;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Актуализируем richTextBox
            var dc = this.DataContext as TestViewVM;

            Thread thread = new(() =>
            {
                while (dc.SelectedQuestion == null) ;

                Dispatcher.Invoke(() => richTextBox.Document = dc.SelectedQuestion.QuestionText);
            });
            
            thread.Start();
        }

        private void EndTest_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Question_Button_MouseEnter(sender, e);
        }

        private void EndTest_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Question_Button_MouseLeave(sender, e);
        }

        private void Conclusion_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Question_Button_MouseEnter(sender, e);
        }

        private void Conclusion_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Question_Button_MouseLeave(sender, e);
        }
    }
}
