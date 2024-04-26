﻿using EEMC.ViewBases;
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
    /// Interaction logic for CoursesList.xaml
    /// </summary>
    public partial class CoursesList : Page, ITextHover
    {
        public Button _oldHoveredButton { get; set; }

        public CoursesList()
        {
            InitializeComponent();
        }

        private void Export_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as ITextHover).ConfirmHoverEffect(sender, ButtonType.CourseButton);

            Cursor = Cursors.Hand;
        }

        private void Export_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default)
            {
                (this as ITextHover).ResetButtonStyle(_oldHoveredButton);
            }

            Cursor = Cursors.Arrow;
        }

        private void Versions_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Export_Button_MouseEnter(sender, e);
        }

        private void Versions_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Export_Button_MouseLeave(sender, e);
        }

        private void Import_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Export_Button_MouseEnter(sender, e);
        }

        private void Import_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Export_Button_MouseLeave(sender, e);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            int i = 0;

            foreach (var item in CourseList.Items)
            {
                ContentPresenter c = (ContentPresenter)CourseList.ItemContainerGenerator.ContainerFromItem(item);
                Button button = c.ContentTemplate.FindName("CourseButton", c) as Button;

                Grid buttonGrid = button.Content as Grid;
                foreach (var child in buttonGrid.Children) 
                {
                    if (child is Border)
                    {
                        Border childBorder = child as Border;

                        childBorder.Background = new ImageBrush(
                            new BitmapImage(
                                new Uri($"pack://application:,,,/Resources/course_img{i % 5}.png", UriKind.RelativeOrAbsolute)
                            )
                        );

                        break;
                    }
                }

                i++;
            }
        }
    }
}
