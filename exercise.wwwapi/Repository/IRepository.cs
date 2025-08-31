using exercise.wwwapi.Model;
using exercise.wwwapi.Model.DTOs;

namespace exercise.wwwapi.Repository
{
    public interface IRepository
    {
        Task<Product> AddAsync(Product model);
        Task<List<Product>> GetAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> UpdateAsync(Product entity, Product model);
        Task<Product> DeleteAsync(int id);
    }
}
