using DataSample.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSample.BusinessLayer.Services
{
    public interface IProductsService
    {
        Task<IEnumerable<Product>> GetAsync(string searchTerm, int pageIndex, int itemsPerPage);

        Task<Product> SaveAsync(Product product);
    }
}
