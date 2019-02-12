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

        public async Task<IEnumerable<Product>> GetProductsAsync(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var sql = @"SELECT p.*, s.*, c.*
                        FROM Products p
                        LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID
                        LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                        WHERE p.ProductName LIKE @searchTerm
                        ORDER BY p.ProductName";

            AddPagination(ref sql, pageIndex, itemsPerPage);

            var products = await context.GetDataAsync<Product, Supplier, Category, Product>(sql,
                map: (product, supplier, category) =>
                {
                    product.Supplier = supplier;
                    product.Category = category;

                    return product;
                }, 
                splitOn: "SupplierId, CategoryId",
                param: new { SearchTerm = $"%{searchTerm}%" });

            return products;
        }

        private void AddPagination(ref string query, int pageIndex, int itemsPerPage)
        {
            query += $" OFFSET {pageIndex * itemsPerPage} ROWS FETCH NEXT {itemsPerPage} ROWS ONLY";
        }
    }
}
