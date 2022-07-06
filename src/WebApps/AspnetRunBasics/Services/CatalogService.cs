using AspnetRunBasics.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services
{
    public class  CatalogService: ICatalogService
    {
        private readonly HttpClient _client;

        public CatalogService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }
        public async Task<IEnumerable<CatalogModel>> GetCatalog()
        {
            return await _client.GetFromJsonAsync<IEnumerable<CatalogModel>>("/Catalog");
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
        {
            return await _client.GetFromJsonAsync<IEnumerable<CatalogModel>>($"/Catalog/GetProductByCategory/{category}");
        }

        public async Task<CatalogModel> GetCatalog(string id)
        {
            return await _client.GetFromJsonAsync<CatalogModel>($"/Catalog/{id}");
        }
        public async Task<CatalogModel> CreateCatalog(CatalogModel model)
        {
            var response = await _client.PostAsJsonAsync<CatalogModel>("/Catalog",model);
            return await response.Content.ReadFromJsonAsync<CatalogModel>();
        }
    }
}
