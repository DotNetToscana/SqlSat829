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

namespace DataSample.Client.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IProductsService productsService;

        public MainWindow(IServiceProvider serviceProvider, IProductsService productsService)
        {
            InitializeComponent();

            this.productsService = productsService;
        }

        private async void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            await productsService.GetProductsAsync(null, 0, 30);

            Application.Current.Shutdown();
        }
    }
}
