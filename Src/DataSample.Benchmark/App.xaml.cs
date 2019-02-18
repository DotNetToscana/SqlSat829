using DataSample.BusinessLayer.Services;
using DataSample.DataAccessLayer.Dapper;
using DataSample.DataAccessLayer.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DataSample.Benchmark
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static DapperProductsService DapperProductsService { get; private set; }

        public static EntityFrameworkProductsService EntityFrameworkProductsService { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("SqlConnection");
            var options = new DbContextOptionsBuilder<EntityFrameworkContext>()
                       .UseSqlServer(connectionString)
                       .Options;

            EntityFrameworkProductsService = new EntityFrameworkProductsService(new EntityFrameworkContext(options));
            DapperProductsService = new DapperProductsService(new DapperContext(connectionString));
        }
    }
}
