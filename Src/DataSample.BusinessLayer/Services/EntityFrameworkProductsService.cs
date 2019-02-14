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
                    CategoryName = p.Category.CategoryName,
                    Discontinued = p.Discontinued,
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    QuantityPerUnit = p.QuantityPerUnit,
                    ReorderLevel = p.ReorderLevel,
                    SupplierName = p.Supplier.CompanyName,                    
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
