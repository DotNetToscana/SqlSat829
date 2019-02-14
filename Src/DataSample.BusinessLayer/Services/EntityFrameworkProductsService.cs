using DataSample.DataAccessLayer.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalModels = DataSample.DataAccessLayer.EntityFramework.Models;
using Entities = DataSample.Common.Models;

namespace DataSample.BusinessLayer.Services
{
    public class EntityFrameworkProductsService : IProductsService
    {
        private readonly IEntityFrameworkContext context;

        public EntityFrameworkProductsService(IEntityFrameworkContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Entities.Product>> GetAsync(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var products = await context.GetData<DalModels.Product>().Include(p => p.Supplier).Include(p => p.Category)
                .Where(p => string.IsNullOrWhiteSpace(searchTerm) || p.ProductName.Contains(searchTerm))
                .OrderBy(p => p.ProductName)
                .Skip(pageIndex * itemsPerPage).Take(itemsPerPage).Select(p => new Entities.Product
                {
                    Category = new Entities.Category
                    {
                        CategoryId = p.Category.CategoryId,
                        CategoryName = p.Category.CategoryName,
                        Description = p.Category.Description
                    },
                    CategoryId = p.CategoryId,
                    Discontinued = p.Discontinued,
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    QuantityPerUnit = p.QuantityPerUnit,
                    ReorderLevel = p.ReorderLevel,
                    Supplier = new Entities.Supplier
                    {
                        Address = p.Supplier.Address,
                        City = p.Supplier.City,
                        CompanyName = p.Supplier.CompanyName,
                        ContactName = p.Supplier.ContactName,
                        ContactTitle = p.Supplier.ContactTitle,
                        Country = p.Supplier.Country,
                        Fax = p.Supplier.Fax,
                        HomePage = p.Supplier.HomePage,
                        Phone = p.Supplier.Phone,
                        PostalCode = p.Supplier.PostalCode,
                        Region = p.Supplier.Region,
                        SupplierId = p.Supplier.SupplierId
                    },
                    SupplierId = p.SupplierId,
                    UnitPrice = p.UnitPrice,
                    UnitsInStock = p.UnitsInStock,
                    UnitsOnOrder = p.UnitsOnOrder
                }).ToListAsync();

            return products;
        }

        public async Task SaveAsync(Entities.Product product)
        {
            var dbProduct = await context.GetData<DalModels.Product>(trackingChanges: true).FirstOrDefaultAsync(p => p.ProductId == product.ProductId);
            if (dbProduct != null)
            {
                dbProduct.ProductName = product.ProductName;
                await context.SaveAsync();
            }
        }
    }
}
