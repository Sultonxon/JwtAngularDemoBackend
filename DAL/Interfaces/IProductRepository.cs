namespace ProductApi.DAL.Interfaces;

public interface IProductRepository
{
    IEnumerable<Product> GetProducts();

    Product Get(int id);

    Task UpdateProduct(Product product);

    Task CreateProduct(Product product);
    
    Task DeleteProduct(int id);


    IEnumerable<Audit> ProductAudits { get; }


    Task SaveChangesAsync(string userId);
}
