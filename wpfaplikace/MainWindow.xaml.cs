using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

using LiveCharts;
using LiveCharts.Wpf;

namespace wpfaplikace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public SeriesCollection ChartSeries { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            Categories = new ObservableCollection<Category>();
            Categories.Add(new Category { name = "test", budget = 1000 });
            Categories.Add(new Category { name = "test2", budget = 1000 });
            listBoxCategories.ItemsSource = Categories;

            ChartSeries = new SeriesCollection();
            DataContext = this;
        }

        private void UpdateChartForCategory(Category category)
        {
            // Calculate used budget
            var usedBudget = category.Spread.Where(s => !s.adding).Sum(s => s.amount);
            // Assume the entire budget is the sum of the used budget plus what's left
            var unusedBudget = category.budget - usedBudget;

            ChartSeries.Clear();
            ChartSeries.Add(new PieSeries
            {
                Title = "Used",
                Values = new ChartValues<decimal> { usedBudget },
                DataLabels = true
            });
            ChartSeries.Add(new PieSeries
            {
                Title = "Unused",
                Values = new ChartValues<decimal> { unusedBudget },
                DataLabels = true
            });

            // Optionally, you can add more properties to PieSeries to customize appearance
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
        private void AddCategory_click(object sender, RoutedEventArgs e)
        {
            Categories.Add(new Category { name = "New Category", budget = 0 });
        }

        private void Categories_(object sender, RoutedEventArgs e)
        {
            categories_popup.IsOpen = !categories_popup.IsOpen;
        }

        private void listBoxCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedCategory = listBoxCategories.SelectedItem as Category;
            if (listBoxCategories.SelectedItem is Category selectedCategory)
            {
                spreadsheetDataGrid.ItemsSource = selectedCategory.Spread;
            }
            if (SelectedCategory != null)
            {
                UpdateChartForCategory(SelectedCategory);
            }
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                    UpdateBudgetUsedText(); // Call method to update the budget text
                }
            }
        }

        private void UpdateBudgetUsedText()
        {
            if (SelectedCategory != null)
            {
                var totalUsed = SelectedCategory.Spread.Where(s => !s.adding).Sum(s => s.amount);
                budget_viz.Text = $"Budget used: {totalUsed} out of {SelectedCategory.budget}";
            }
        }
    }
}