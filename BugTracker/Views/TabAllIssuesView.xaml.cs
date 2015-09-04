using BugTracker.Common;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using BugTracker.ViewModels;
using BugTracker.Model;

namespace BugTracker.Views
{
    /// <summary>
    /// Interaction logic for TabAllIssuesView.xaml
    /// </summary>
    public partial class TabAllIssuesView : UserControl
    {
        private GridViewColumnHeader listviewSortCol = null;
        private SortAdorner listviewSortAdorner = null;

        public TabAllIssuesView()
        {
            InitializeComponent();
        }

        private void ListViewColumnHeader_Click(Object sender, RoutedEventArgs e)
        {
            using (listviewIssues.Items.DeferRefresh())
            {
                GridViewColumnHeader column = (GridViewColumnHeader)sender;
                string sortBy = column.Tag.ToString();

                if (listviewSortCol != null)
                {
                    AdornerLayer.GetAdornerLayer(listviewSortCol).Remove(listviewSortAdorner);
                    listviewIssues.Items.SortDescriptions.Clear();
                }

                ListSortDirection newDir = ListSortDirection.Ascending;
                if (column.Equals(listviewSortCol) && listviewSortAdorner.Direction == newDir)
                {
                    newDir = ListSortDirection.Descending;
                }

                listviewSortCol = column;
                listviewSortAdorner = new SortAdorner(listviewSortCol, newDir);
                AdornerLayer.GetAdornerLayer(listviewSortCol).Add(listviewSortAdorner);
                listviewIssues.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
            }
        }

        private void listview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as IssueViewModel;
            if (item != null)
            {
                ((TabAllIssuesViewModel)DataContext).OpenIssueInNewTab(item.IssueID);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbFilter.Focus();
            tbFilter.Select(tbFilter.Text.Length, 0);
        }
    }
}
