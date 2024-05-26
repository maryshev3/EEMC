using EEMC.Models;
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
    /// Interaction logic for CreateTest.xaml
    /// </summary>
    public partial class CreateTest : Window, ITextHover, IImageHover
    {
        private Button _oldPressedButton;
        public Button _oldHoveredButton { get; set; }

        public CreateTest()
        {
            InitializeComponent();
        }

        private void AddQuestion_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as ITextHover).ConfirmHoverEffect(sender, ButtonType.AddButton);

            Cursor = Cursors.Hand;
        }

        private void AddQuestion_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default)
            {
                (this as ITextHover).ResetButtonStyle(_oldHoveredButton, true);
            }

            Cursor = Cursors.Arrow;
        }

        private void ResetButtonStyle(Button button)
        {
            button.BorderThickness = new Thickness() { Left = 0 };
            button.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#efeff5"));

            button.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#efeff5"));

            var text = (button.Content as Label);
            text.Foreground = System.Windows.Media.Brushes.Black;
        }

        private void ConfirmSelectedStyle(Button button)
        {
            if (_oldPressedButton != default)
            {
                ResetButtonStyle(_oldPressedButton);
            }

            button.BorderThickness = new Thickness() { Left = 3 };
            button.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4b6cdf"));

            ColorSettings colorSettings = new ColorSettings(ButtonType.CourseButton);

            var text = button.Content as Label;
            text.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorSettings.Foreground));

            _oldPressedButton = button;
        }

        private void Question_Button_Click(object sender, RoutedEventArgs e)
        {
            var dc = this.DataContext as CreateTestVM;

            //Заполняем старый вопрос
            dc.SelectedQuestion.QuestionText = richTextBox.Document;

            dc.OpenQuestion.Execute((sender as Button).DataContext as Question);

            //Заполняем новый вопрос
            richTextBox.Document = dc.SelectedQuestion.QuestionText ?? new FlowDocument();

            //Применяем стиль выбранной кнопки
            _oldHoveredButton = default;
            ConfirmSelectedStyle(sender as Button);
        }

        private void Question_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender as Button == _oldPressedButton)
                return;

            (this as ITextHover).ConfirmHoverEffect(sender, ButtonType.CourseButton);

            Cursor = Cursors.Hand;
        }

        private void Question_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender as Button == _oldPressedButton)
                return;

            AddQuestion_Button_MouseLeave(sender, e);
        }

        private void AddQuestion_Button_Click(object sender, RoutedEventArgs e)
        {
            var dc = this.DataContext as CreateTestVM;

            //Заполняем старый вопрос
            if (dc.SelectedQuestion != default)
                dc.SelectedQuestion.QuestionText = richTextBox.Document;

            dc.AddQuestion.Execute(new object());

            //Заполняем новый
            richTextBox.Document = dc.SelectedQuestion.QuestionText ?? new FlowDocument();

            ConfirmSelectedStyle(FindButtonWithSelectedQuestion());
        }

        private void Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            var dc = this.DataContext as CreateTestVM;

            dc.RemoveQuestion.Execute(new object());

            //Заполняем новый
            richTextBox.Document = dc.SelectedQuestion?.QuestionText ?? new FlowDocument();

            ConfirmSelectedStyle(FindButtonWithSelectedQuestion());
        }

        private Button FindButtonWithSelectedQuestion()
        {
            var dc = this.DataContext as CreateTestVM;

            for (int i = 0; i < QuestionsList.Items.Count; i++)
            {
                ContentPresenter c = (ContentPresenter)QuestionsList.ItemContainerGenerator.ContainerFromItem(QuestionsList.Items[i]);

                c.ApplyTemplate();

                Button button = c.ContentTemplate.FindName("Question_Button", c) as Button;

                var buttonDc = (button.Content as Label).Content as string;

                if (buttonDc == dc.SelectedQuestion.DisplayedShortQuestionText)
                {
                    return button;
                }
            }

            return default;
        }

        private void Down_Button_Click(object sender, RoutedEventArgs e)
        {
            var dc = this.DataContext as CreateTestVM;

            dc.DownQuestion.Execute(new object());

            ConfirmSelectedStyle(FindButtonWithSelectedQuestion());
        }

        private void Up_Button_Click(object sender, RoutedEventArgs e)
        {
            var dc = this.DataContext as CreateTestVM;

            dc.UpQuestion.Execute(new object());

            ConfirmSelectedStyle(FindButtonWithSelectedQuestion());
        }

        private void SaveTest_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as ITextHover).ConfirmHoverEffect(sender, ButtonType.CourseButton);

            Cursor = Cursors.Hand;
        }

        private void SaveTest_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            AddQuestion_Button_MouseLeave(sender, e);
        }

        private void SaveTest_Button_Click(object sender, RoutedEventArgs e)
        {
            var dc = this.DataContext as CreateTestVM;

            //Сохраняем последний ричтекстбокс и вызываем команду
            if (dc.SelectedQuestion != default)
                dc.SelectedQuestion.QuestionText = richTextBox.Document;
            else
            {
                MessageBox.Show("Нельзя сохранять пустой тест");
            }

            dc.SaveTest.Execute(new object());
        }

        private void Down_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as IImageHover).ConfirmHoverEffect(sender);

            Cursor = Cursors.Hand;
        }

        private void Down_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default)
            {
                (this as IImageHover).ResetButtonStyle(_oldHoveredButton);
            }

            Cursor = Cursors.Arrow;
        }

        private void Up_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Down_Button_MouseEnter(sender, e);
        }

        private void Up_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Down_Button_MouseLeave(sender, e);
        }

        private void Remove_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Down_Button_MouseEnter(sender, e);
        }

        private void Remove_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Down_Button_MouseLeave(sender, e);
        }
    }
}
