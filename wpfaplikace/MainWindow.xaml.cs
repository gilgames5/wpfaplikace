using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace wpfaplikace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            Categories = new ObservableCollection<Category>();
            Categories.Add(new Category { name = "test", budget = 1000 });
            Categories.Add(new Category { name = "test2", budget = 1000 });
            listBoxCategories.ItemsSource = Categories;
        }

        public ObservableCollection<Category> Categories { get; set; }
        public class MainSpreadRow
        {
            public bool adding { get; set; }
            public decimal amount { get; set; }
            public DateTime date { get; set; }
            public string note { get; set; }
        }
        public class Category
        {
            public string name { get; set; }
            public decimal budget { get; set; }
            public ObservableCollection<MainSpreadRow> Spread { get; set; }

            public Category()
            {
                Spread = new ObservableCollection<MainSpreadRow>();
            }
        }
        private void add_row(object sender, RoutedEventArgs e)
        {
            if (listBoxCategories.SelectedItem is Category selectedCategory)
            {
                selectedCategory.Spread.Add(new MainSpreadRow());
            }
        }

        private void ListBoxCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxCategories.SelectedItem is Category selectedCategory)
            {
                spreadsheetDataGrid.ItemsSource = selectedCategory.Spread;
            }
        }


        private void Categories_(object sender, RoutedEventArgs e)
        {
            categories_popup.IsOpen = !categories_popup.IsOpen;
        }

        private void listBoxCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxCategories.SelectedItem is Category selectedCategory)
            {
                spreadsheetDataGrid.ItemsSource = selectedCategory.Spread;
            }
        }
    }
}
