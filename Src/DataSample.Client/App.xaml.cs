using DataSample.BusinessLayer;
using DataSample.BusinessLayer.Services;
using DataSample.Client.Services;
using DataSample.Client.Views;
using DataSample.DataAccessLayer.Dapper;
using DataSample.DataAccessLayer.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DataSample.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //var launch = Environment.GetEnvironmentVariable("LAUNCH_PROFILE");

            if (string.IsNullOrWhiteSpace(env))
            {
                env = "Development";
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            // Create a service collection and configure our depdencies
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Build the our IServiceProvider and set our static reference to it
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("SqlConnection");

            services
                .AddSingleton<IEntityFrameworkContext, EntityFrameworkContext>((_) =>
                {
                    var options = new DbContextOptionsBuilder<EntityFrameworkContext>()
                        .UseSqlServer(connectionString)
                        .Options;

                    return new EntityFrameworkContext(options);
                })
                .AddSingleton<IDapperContext, DapperContext>((_) => new DapperContext(connectionString))
                .AddSingleton<IProductsService, EntityFrameworkProductsService>()
                //.AddSingleton<IProductsService, DapperProductsService>()
                //.AddSingleton<IProductsService, RemoteProductsService>()
                ;

            // Add AutoMapper
            //var config = new AutoMapper.MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile(new AutoMapperProfile());
            //});
            //
            //var mapper = config.CreateMapper();
            //services.AddSingleton(mapper);

            services.AddTransient(typeof(MainWindow));
            services.AddTransient(typeof(EditProductWindow));
        }
    }
}
