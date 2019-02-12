using DataSample.DataAccessLayer.EntityFramework;
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
using Microsoft.Extensions.DependencyInjection;
using DataSample.BusinessLayer;
using DataSample.BusinessLayer.Services;

namespace DataSample.Client.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IProductsService productsService;

        public MainWindow(IProductsService productsService)
        {
            InitializeComponent();

            this.productsService = productsService;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await SearchAsync();
        }

        private async void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            await SearchAsync();
        }

        private async Task SearchAsync()
        {
            try
            {
                var products = await productsService.GetProductsAsync(SearchTextBox.Text.Trim(), 0, 50);
                ProductsDataGrid.ItemsSource = products;
            }
            catch
            {
            }
        }
    }
}
