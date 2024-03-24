using EEMC.Models;
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
    public partial class CourseWindow : Page
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
                AddFile_Button.IsEnabled = true;
                AddFolder_Button.IsEnabled = true;

                Rename_Button.IsEnabled = false;
                Remove_Button.IsEnabled = false;

                return; 
            }

            Remove_Button.IsEnabled= true;

            Boolean isFile = item.Type == ContentType.File;

            Rename_Button.IsEnabled = !isFile;
            AddFolder_Button.IsEnabled = !isFile;
            AddFile_Button.IsEnabled = !isFile;
        }
    }
}
