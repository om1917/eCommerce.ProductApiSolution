using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                //Check if product already exist
                var getProduct = await GetByAsync(_ => _.Name!.Equals(entity.Name));
                if (getProduct is not null && !string.IsNullOrEmpty(getProduct.Name))
                    return new Response(false, $"{entity.Name} already added");

                var currentEntity = context.Products.Add(entity).Entity;
                await context.SaveChangesAsync();
                if (currentEntity is not null && currentEntity.Id > 0)
                    return new Response(true, $"{entity.Name} added to Database successfully");
                else
                    return new Response(false, $"Error occured while adding {entity.Name}");
            }
            catch (Exception ex)
            {
                //Log Original Exception
                LogException.LogExceptions(ex);

                //Display scary free message to client
                return new Response(false, "Adding new Product");
            }
        }

        public async Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                //Check if product already exist
                var product = await FindByIdAsync(entity.Id);
                if(product is null)
                    return new Response(false,$"{entity.Name} not found");

                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return new Response(true,$"{entity.Name} deleted successfully !");
               
            }
            catch (Exception ex)
            {
                //Log Original Exception
                LogException.LogExceptions(ex);

                //Display scary free message to client
                return new Response(false, "Deleting new Product");
            }
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
                //Check if product already exist
                var product = await context.Products.FindAsync(id);
                return product is not null? product : null!;               

            }
            catch (Exception ex)
            {
                //Log Original Exception
                LogException.LogExceptions(ex);

                throw new Exception("Error occured while retriving product");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                //Check if product already exist
                var productList = await context.Products.AsNoTracking().ToListAsync();
                return productList is not null ? productList : null!;

            }
            catch (Exception ex)
            {
                //Log Original Exception
                LogException.LogExceptions(ex);

                throw new Exception("Error occured while retriving product");
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                //Check if product already exist
                var productQuery = await context.Products.Where(predicate).FirstOrDefaultAsync();
                return productQuery is not null ? productQuery : null!;
            }
            catch (Exception ex)
            {
                //Log Original Exception
                LogException.LogExceptions(ex);

                throw new Exception("Error occured while retriving product");
            }
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                //Check if product already exist
                var product = await FindByIdAsync(entity.Id);
                if(product is null)
                return new Response(false,$"{entity.Name} not found.");

                context.Entry(product).State = EntityState.Detached;
                context.Products.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true,$"{entity.Name} updated successfully !");

            }
            catch (Exception ex)
            {
                //Log Original Exception
                LogException.LogExceptions(ex);

                throw new Exception("Error occured while retriving product");
            }
        }
    }
}
