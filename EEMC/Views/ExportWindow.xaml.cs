﻿using EEMC.ViewBases;
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
    /// Interaction logic for ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow : UserControl, ITextHover
    {
        public Button _oldHoveredButton { get; set; }

        public ExportWindow()
        {
            InitializeComponent();
        }

        private void Cancel_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as ITextHover).ConfirmHoverEffect(sender, ButtonType.CancelButton);

            Cursor = Cursors.Hand;
        }

        private void Cancel_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Add_Button_MouseLeave(sender, e);
        }

        private void Add_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as ITextHover).ConfirmHoverEffect(sender, ButtonType.AddButton);

            Cursor = Cursors.Hand;
        }

        private void Add_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default)
            {
                (this as ITextHover).ResetButtonStyle(_oldHoveredButton);
            }

            Cursor = Cursors.Arrow;
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            var dc = (sender as Button).DataContext as ExportWindowVM;

            dc.Export_Click.Execute(inputBox.Text);

            Mouse.OverrideCursor = null;
        }
    }
}
