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
    /// Interaction logic for RemoveTheme.xaml
    /// </summary>
    public partial class RemoveTheme : UserControl, ITextHover
    {
        public Button _oldHoveredButton { get; set; }

        public RemoveTheme()
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
            Remove_Button_MouseLeave(sender, e);
        }

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
    }
}
