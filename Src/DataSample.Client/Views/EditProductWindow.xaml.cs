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
using DataSample.Common.Models;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace DataSample.Client.Views
{
    /// <summary>
    /// Interaction logic for EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        private readonly IProductsService productsService;

        private Product product;
        public Product Product
        {
            get => product;
            set
            {
                product = value;
                IdTextBlock.Text = product.ProductId.ToString();
                NameTextBox.Text = product.ProductName;
            }
        }

        public EditProductWindow(IProductsService productsService)
        {
            InitializeComponent();

            this.productsService = productsService;
        }

        private async void SaveProductButton_Click(object sender, RoutedEventArgs e)
        {
            product.ProductName = NameTextBox.Text;
            await productsService.SaveAsync(product);

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
