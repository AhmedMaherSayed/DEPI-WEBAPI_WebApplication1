using WebApplication1.Entities;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class ProductService(IGenericRepository<Product> repository) : IProductService
    {
        public Task<Product> CreateAsync(Product entity)
            => repository.CreateAsync(entity);

        public async Task<bool> DeleteAsync(int id)
            => await repository.DeleteAsync(id);

        public async Task<IEnumerable<Product>> GetAllAsync()
            => await repository.GetAllAsync();

        public async Task<Product> GetByIdAsync(int id)
            => await repository.GetByIdAsync(id);

        public async Task<bool> UpdateAsync(Product entity)
            => await repository.UpdateAsync(entity);
    }
}
