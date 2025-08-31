using exercise.wwwapi.Model;
using exercise.wwwapi.Model.DTOs;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace exercise.wwwapi.Endpoints
{
    public static class ProductEndpoints
    {
        public static void ConfigureProductEndpoints(this WebApplication app)
        {
            var products = app.MapGroup("products");
            products.MapPost("/", AddProduct);
            products.MapGet("/", GetProducts);
            products.MapGet("/{id}", GetProductById);
            products.MapDelete("/{id}", Delete);
            products.MapPut("/", Update);
        }


        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> AddProduct(IRepository repository, ProductPost model)
        {
            if (model.Price.GetType() != typeof(int)) return TypedResults.BadRequest($"Not an int");

            Product entity = new Product();
            entity.Name = model.Name;
            entity.Category = model.Category;
            entity.Price = model.Price;


            var list = await repository.GetAsync();
            if (list.Any(x => x.Name == model.Name)) return TypedResults.BadRequest($"same name exists");


            await repository.AddAsync(entity);
            return TypedResults.Created($"https://localhost:7188/products/{entity.Id}", entity);
        }

        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetProducts(IRepository repository, string? category)
        {
            var results = await repository.GetAsync();

            if (category is null) return TypedResults.Ok(results);

            var byCategory = results.Where(p => p.Category.ToLower() == category.ToLower());

            return (byCategory.Count() > 0) ? TypedResults.Ok(byCategory) : TypedResults.NotFound("No products of the provided category found");
            
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetProductById(IRepository repository, int id)
        {
            var result = await repository.GetByIdAsync(id);

            if (result is null)
            {
                return TypedResults.NotFound("Product not found");
            }
            return TypedResults.Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Delete(IRepository repository, int id)
        {
            var entity = await repository.DeleteAsync(id);
            return entity is not null ? TypedResults.Ok(entity) : TypedResults.NotFound("Product Not found");

        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Update(IRepository repository, int id, ProductPut model)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity is null) return TypedResults.NotFound("No product with that ID exists.");


            if (model.Price.GetType() != typeof(int)) return TypedResults.BadRequest($"ID must be an int");

            var list = await repository.GetAsync();
            if (list.Any(x => x.Name == model.Name)) return TypedResults.BadRequest($"same name exists");
            
            Product p = new Product();
            if (model.Name != null) p.Name = model.Name;
            if (model.Category != null) p.Category = model.Category;
            if (model.Price != null) p.Price = model.Price.Value;


            var res = await repository.UpdateAsync(entity, p);
            return TypedResults.Ok(res);
        }
    }
}
