using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;

        public CatalogService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalog()
        {
            return await _client.GetFromJsonAsync<IEnumerable<CatalogModel>>(_client.BaseAddress + "api/v1/Catalog");
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
        {
            return await _client.GetFromJsonAsync<IEnumerable<CatalogModel>>($"api/v1/Catalog/GetProductByCategory/{category}");
        }

        public async Task<CatalogModel> GetCatalog(string id)
        {
            return await _client.GetFromJsonAsync<CatalogModel>($"api/v1/Catalog/{id}");
        }
    }
}
