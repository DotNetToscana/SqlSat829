using DataSample.BusinessLayer.Services;
using DataSample.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataSample.Client.Services
{
    public class RemoteProductsService : IProductsService
    {
        private const string BASE_URL = "https://localhost:44330/api/";
        private readonly HttpClient client;

        public RemoteProductsService()
        {
            client = new HttpClient { BaseAddress = new Uri(BASE_URL) };
        }

        public async Task<IEnumerable<Product>> GetAsync(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var resource = $"Products?searchTerm={searchTerm}&pageIndex={pageIndex}&itemsPerPage={itemsPerPage}";
            var response = await client.GetStringAsync(resource);
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(response);
        }

        public async Task<Product> SaveAsync(Product product)
        {
            var json = JsonConvert.SerializeObject(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PostAsync("Products", content);

            return product;
        }
    }
}
