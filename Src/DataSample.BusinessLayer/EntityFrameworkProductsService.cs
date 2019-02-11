using DataSample.DataAccessLayer.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalModels = DataSample.DataAccessLayer.EntityFramework.Models;
using Entities = DataSample.Common.Models;

namespace DataSample.BusinessLayer
{
    public class EntityFrameworkProductsService : IProductsService
    {
        private readonly IEntityFrameworkContext context;

        public EntityFrameworkProductsService(IEntityFrameworkContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Entities.Product>> GetProductsAsync(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var products = await context.GetData<DalModels.Product>().Include(p => p.Supplier).Include(p => p.Category)
                .Where(p => string.IsNullOrWhiteSpace(searchTerm) || p.ProductName.Contains(searchTerm))
                .OrderBy(p => p.ProductName)
                .Skip(pageIndex * itemsPerPage).Take(itemsPerPage).ToListAsync();

            return null;
        }
    }
}
