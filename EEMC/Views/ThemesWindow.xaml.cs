using EEMC.ViewBases;
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
    public partial class ThemesWindow : Page, IHover
    {
        public ThemesWindow()
        {
            InitializeComponent();
        }

        public Button _oldHoveredButton { get; set; }

        private void AddTheme_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as IHover).ConfirmHoverEffect(sender, ButtonType.AddButton);

            Cursor = Cursors.Hand;
        }

        private void AddTheme_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default)
            {
                (this as IHover).ResetButtonStyle(_oldHoveredButton);
            }

            Cursor = Cursors.Arrow;
        }

        private void RenameTheme_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void RenameTheme_Button_MouseLeave(object sender, MouseEventArgs e)
        {
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
    }
}
