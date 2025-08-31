using exercise.wwwapi.Data;
using exercise.wwwapi.Model;
using exercise.wwwapi.Model.DTOs;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository
{
    public class Repository : IRepository
    {
        private DataContext _db;

        public Repository(DataContext db)
        {
            _db = db;
        }
        
        
        public async Task<List<Product>> GetAsync()
        {
            return await _db.Products.ToListAsync();
        }


        public async Task<Product> AddAsync(Product product)
        {
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task<Product> DeleteAsync(int id)
        {
            var target = await _db.Products.FindAsync(id);
            if (target == null) return target;
            _db.Products.Remove(target);
            await _db.SaveChangesAsync();
            return target;
        }


        public async Task<Product> GetByIdAsync(int id)
        {
            return await _db.Products.FindAsync(id);
        }

        public async Task<Product> UpdateAsync(Product entity, Product model)
        {

           
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
