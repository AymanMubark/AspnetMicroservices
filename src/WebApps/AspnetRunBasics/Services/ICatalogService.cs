using AspnetRunBasics.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services  
{
    public interface ICatalogService
    {
        public Task<IEnumerable<CatalogModel>> GetCatalog();
        public Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category);
        public Task<CatalogModel> GetCatalog(string id);
        public Task<CatalogModel> CreateCatalog(CatalogModel model);
    }
}
