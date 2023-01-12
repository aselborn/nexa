using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Nexa.ViewModels
{
    public class MultipleDatagrid : DataGrid
    {
        public MultipleDatagrid()
        {
            this.SelectionChanged += MultipleDatagrid_SelectionChanged;
        }

        private void MultipleDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItemsList = this.SelectedItems;
        }


        public IList SelectedItemsList
        {
            get => (IList)GetValue(SelectedItemsListProperty);
            set
            {
                SetValue(SelectedItemsListProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedItemsListProperty =
            DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(MultipleDatagrid), new PropertyMetadata(null));
    }
}
