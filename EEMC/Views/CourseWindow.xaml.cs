﻿using EEMC.Models;
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
    /// Логика взаимодействия для CourseWindow.xaml
    /// </summary>
    public partial class CourseWindow : Page, ITextHover, IInitWebView2
    {
        public CourseWindow()
        {
            InitializeComponent();
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape)
                return;

            var item = CourseTreeView.ItemContainerGenerator.ContainerFromItem(CourseTreeView.SelectedItem) as TreeViewItem;

            if (item != null)
                item.IsSelected = false;
        }

        private void CourseTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = CourseTreeView.SelectedItem as Explorer;

            if (item == default) 
            {
                AddFile_Button.IsEnabled = false;
                AddFolder_Button.IsEnabled = false;

                Rename_Button.IsEnabled = false;
                Remove_Button.IsEnabled = false;

                return; 
            }

            Remove_Button.IsEnabled= true;

            Boolean isFile = item.Type == ContentType.File;

            Rename_Button.IsEnabled = !isFile;
            AddFolder_Button.IsEnabled = !isFile;
            AddFile_Button.IsEnabled = !isFile;

            //Устанавливаем Source у WebView2
            var dc = this.DataContext as CourseWindowVM;

            dc.ShowFile_Click.Execute(item);

            webView.Source = dc.PdfPath;
        }

        public Button _oldHoveredButton { get; set; }

        private void Remove_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as ITextHover).ConfirmHoverEffect(sender, ButtonType.RemoveButton);

            Cursor = Cursors.Hand;
        }

        private void Remove_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default)
            {
                (this as ITextHover).ResetButtonStyle(_oldHoveredButton);
            }

            Cursor = Cursors.Arrow;
        }

        private void Rename_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as ITextHover).ConfirmHoverEffect(sender, ButtonType.ChangeButton);

            Cursor = Cursors.Hand;
        }

        private void Rename_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Remove_Button_MouseLeave(sender, e);
        }

        private void AddFile_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as ITextHover).ConfirmHoverEffect(sender, ButtonType.AddButton);

            Cursor = Cursors.Hand;
        }

        private void AddFile_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Remove_Button_MouseLeave(sender, e);
        }

        private void AddFolder_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as ITextHover).ConfirmHoverEffect(sender, ButtonType.AddButton);

            Cursor = Cursors.Hand;
        }

        private void AddFolder_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Remove_Button_MouseLeave(sender, e);
        }

        private void AddGroup_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as ITextHover).ConfirmHoverEffect(sender, ButtonType.AddButton);

            Cursor = Cursors.Hand;
        }

        private void AddGroup_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Remove_Button_MouseLeave(sender, e);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await (this as IInitWebView2).InitializeWebView2(webView);

            //Устанавливаем Source у WebView2
            webView.Source = (this.DataContext as CourseWindowVM).PdfPath;
        }
    }
}
