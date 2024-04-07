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
    /// Interaction logic for ChangeDescriptionTheme.xaml
    /// </summary>
    public partial class ChangeDescriptionTheme : UserControl, IHover
    {
        public Button _oldHoveredButton { get; set; }

        public ChangeDescriptionTheme()
        {
            InitializeComponent();
        }

        private void Cancel_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as IHover).ConfirmHoverEffect(sender, ButtonType.CancelButton);

            Cursor = Cursors.Hand;
        }

        private void Cancel_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Change_Button_MouseLeave(sender, e);
        }

        private void Change_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as IHover).ConfirmHoverEffect(sender, ButtonType.ConfirmButton);

            Cursor = Cursors.Hand;
        }

        private void Change_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default)
            {
                (this as IHover).ResetButtonStyle(_oldHoveredButton);
            }

            Cursor = Cursors.Arrow;
        }
    }
}
