﻿using EEMC.Models;
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
        public TestView()
        {
            InitializeComponent();
        }

        private void Question_Button_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void Question_Button_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void Question_Button_Click(object sender, RoutedEventArgs e)
        {
            //Актуализируем richTextBox и открываем вопрос
            var dc = this.DataContext as TestViewVM;

            dc.OpenQuestion.Execute((sender as Button).DataContext as Question);

            richTextBox.Document = dc.SelectedQuestion.QuestionText;
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

        }

        private void EndTest_Button_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void Conclusion_Button_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void Conclusion_Button_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
