﻿using EEMC.Models;
using EEMC.Services;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EEMC.Views
{
    /// <summary>
    /// Interaction logic for ThemesWindow.xaml
    /// </summary>
    public partial class ThemesWindow : Page, ITextHover, IImageHover
    {
        public ThemesWindow()
        {
            InitializeComponent();
        }

        public Button _oldHoveredButton { get; set; }

        private void AddTheme_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as ITextHover).ConfirmHoverEffect(sender, ButtonType.AddButton);

            Cursor = Cursors.Hand;
        }

        private void AddTheme_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default)
            {
                (this as ITextHover).ResetButtonStyle(_oldHoveredButton);
            }

            Cursor = Cursors.Arrow;
        }

        private void RenameTheme_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as IImageHover).ConfirmHoverEffect(sender);

            Cursor = Cursors.Hand;
        }

        private void RenameTheme_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default)
            {
                (this as IImageHover).ResetButtonStyle(_oldHoveredButton);
            }

            Cursor = Cursors.Arrow;
        }

        private void ChangeDescription_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            RenameTheme_Button_MouseEnter(sender, e);
        }

        private void ChangeDescription_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            RenameTheme_Button_MouseLeave(sender, e);
        }

        private void RemoveTheme_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            RenameTheme_Button_MouseEnter(sender, e);
        }

        private void RemoveTheme_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            RenameTheme_Button_MouseLeave(sender, e);
        }

        private void File_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default)
            {
                (this as ITextHover).ResetButtonStyle(_oldHoveredButton);
            }

            Button button = sender as Button;
            ColorSettings colorSettings = new ColorSettings(ButtonType.CourseButton);

            button.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorSettings.Background));

            var stackPanel = (button.Content as StackPanel);

            foreach (var child in stackPanel.Children)
            {
                if (child is Label)
                {
                    Label el = child as Label;
                    el.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorSettings.Background));
                    el.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorSettings.Foreground));
                }
            }

            _oldHoveredButton = button;

            Cursor = Cursors.Hand;
        }

        private void File_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;

            if (_oldHoveredButton != default)
            {
                (this as ITextHover).ResetButtonStyle(_oldHoveredButton);
            }

            Cursor = Cursors.Arrow;
        }

        private void File_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            ThemeFile file = button.DataContext as ThemeFile;

            ThemesWindowVM dc = ThemeWindow.DataContext as ThemesWindowVM;

            if (file.IsTest())
            {
                dc.ShowFile_Click.Execute(file);

                return;
            }

            if (file.IsTotalTest())
            {
                //Формируем контекстное меню для итогового теста
                ContextMenu cm = new();

                MenuItem openItem = new();
                openItem.Header = "Пройти тест";
                openItem.Command = dc.ShowFile_Click;
                openItem.CommandParameter = file;

                MenuItem editItem = new();
                editItem.Header = "Изменить тест";
                editItem.Command = dc.EditTotalTest_Click;
                editItem.CommandParameter = file;

                cm.Items.Add(openItem);
                cm.Items.Add(editItem);

                cm.IsOpen = true;

                return;
            }

            if (file.IsSupportedExtension())
            {
                //Формируем контекстное меню для файла
                ContextMenu cm = new();

                MenuItem openItem = new();
                openItem.Header = "Просмотреть файл";
                openItem.Command = dc.ShowFile_Click;
                openItem.CommandParameter = file;

                MenuItem downloadItem = new();
                downloadItem.Header = "Скачать файл";
                downloadItem.Command = dc.DownloadFile_Click;
                downloadItem.CommandParameter = file;

                cm.Items.Add(openItem);
                cm.Items.Add(downloadItem);

                cm.IsOpen = true;

                return;
            }

            dc.DownloadFile_Click.Execute(file);
        }

        private void AddThemeFile_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            AddTheme_Button_MouseEnter(sender, e);
        }

        private void AddThemeFile_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            AddTheme_Button_MouseLeave(sender, e);
        }

        private void RemoveFile_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            RenameTheme_Button_MouseEnter(sender, e);
        }

        private void RemoveFile_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            RenameTheme_Button_MouseLeave(sender, e);
        }

        private void AddTest_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            AddTheme_Button_MouseEnter(sender, e);
        }

        private void AddTest_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            AddTheme_Button_MouseLeave(sender, e);
        }

        private void Down_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            RenameTheme_Button_MouseEnter(sender, e);
        }

        private void Down_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            RenameTheme_Button_MouseLeave(sender, e);
        }

        private void Up_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            RenameTheme_Button_MouseEnter(sender, e);
        }

        private void Up_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            RenameTheme_Button_MouseLeave(sender, e);
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var dc = this.DataContext as ThemesWindowVM;
            var scroll = sender as ScrollViewer;

            if (scroll.VerticalOffset != 0)
                ScrollSaver.ScrollPosition = scroll.VerticalOffset;

            ScrollSaver.CurrentCourseName = dc.CurrentCourse.Name;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var dc = this.DataContext as ThemesWindowVM;

            if (dc.CurrentCourse.Name == ScrollSaver.CurrentCourseName)
            {
                thisScroll.ScrollToVerticalOffset(ScrollSaver.ScrollPosition);
                thisScroll.UpdateLayout();
            }
        }

        private void Guid_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as ITextHover).ConfirmHoverEffect(sender, ButtonType.CourseButton);

            Cursor = Cursors.Hand;
        }

        private void Guid_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default)
            {
                (this as ITextHover).ResetButtonStyle(_oldHoveredButton);
            }

            Cursor = Cursors.Arrow;
        }
    }
}
