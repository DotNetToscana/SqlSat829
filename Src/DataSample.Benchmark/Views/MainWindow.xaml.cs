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

namespace DataSample.Benchmark.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void ButtonClearAndInit_Click(object sender, RoutedEventArgs e)
        {
            LogArea.Text = string.Empty;
            needleDapper.Value = 0.0f;
            needleEntityFramework.Value = 0.0f;
            scaleDapper.Max = 2000;
            scaleEntityFramework.Max = 2000;

            var elapsedMilliseconds = await MeasureExecutionAsync(async () =>
            {
                await App.DapperProductsService.GetAsync(string.Empty, 0, 100);
                await App.EntityFrameworkProductsService.GetAsync(string.Empty, 0, 100);
            });
            LogArea.Text = "Ready";
        }


        protected async Task<long> MeasureExecutionAsync(Func<Task> action)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                Cursor = Cursors.Wait;
                await action.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
            watch.Stop();
            var elapsedMS = watch.ElapsedMilliseconds;

            return elapsedMS;
        }

        private async Task ExecuteTestAsync(float maxValue, int maxRecords)
        {
            needleDapper.Value = needleEntityFramework.Value = 0.0f;
            scaleDapper.Max = scaleEntityFramework.Max = maxValue;

            var elapsedMilliseconds = await MeasureExecutionAsync(async () =>
            {
                await App.DapperProductsService.GetAsync(string.Empty, 0, maxRecords);
            });
            needleDapper.Value = elapsedMilliseconds;
            LogArea.Text += $"\nDapper {maxRecords}: {elapsedMilliseconds}ms";

            elapsedMilliseconds = await MeasureExecutionAsync(async () =>
            {
                await App.EntityFrameworkProductsService.GetAsync(string.Empty, 0, maxRecords);
            });

            needleEntityFramework.Value = elapsedMilliseconds;
            LogArea.Text += $"\nEntity Framework Core {maxRecords}: {elapsedMilliseconds}ms";
        }

        private async void Button1000Records_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteTestAsync(2000, 1000);
        }

        private async void Button10000Records_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteTestAsync(2000, 10000);
        }
        private async void Button50000Records_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteTestAsync(2000, 50000);
        }
        private async void Button100000Records_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteTestAsync(2000, 100000);
        }
    }
}
