using DataSample.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSample.BusinessLayer.Services
{
    public interface IProductsService
    {
        Task<IEnumerable<Product>> GetProductsAsync(string searchTerm, int pageIndex, int itemsPerPage);
    }
}
