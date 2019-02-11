using DataSample.BusinessLayer;
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
        public static IServiceProvider ServiceProvider { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("SqlConnection");

            ServiceProvider = new ServiceCollection()
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
                .BuildServiceProvider();
        }
    }
}
