using DataSample.Common.Models;
using DataSample.DataAccessLayer.Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataSample.BusinessLayer.Services
{
    public class DapperProductsService : IProductsService
    {
        private readonly IDapperContext context;

        public DapperProductsService(IDapperContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Product>> GetAsync(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var sql = @"SELECT p.*, s.CompanyName AS SupplierName, c.CategoryName
                        FROM Products p
                        LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID
                        LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                        WHERE p.ProductName LIKE @searchTerm
                        ORDER BY p.ProductName";

            AddPagination(ref sql, pageIndex, itemsPerPage);

            var products = await context.GetDataAsync<Product>(sql,                
                param: new { SearchTerm = $"%{searchTerm}%" });

            return products;
        }

        public Task SaveAsync(Product product)
        {
            var sql = "UPDATE Products SET ProductName = @productName WHERE ProductId = @productId";
            return context.ExecuteAsync(sql,
                new
                {
                    product.ProductName,
                    product.ProductId
                });
        }

        private void AddPagination(ref string sql, int pageIndex, int itemsPerPage)
        {
            sql += $" OFFSET {pageIndex * itemsPerPage} ROWS FETCH NEXT {itemsPerPage} ROWS ONLY";
        }
    }
}
