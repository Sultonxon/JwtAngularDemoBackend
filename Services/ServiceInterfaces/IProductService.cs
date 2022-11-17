namespace ProductApi.Services.ServiceInterfaces;

public interface IProductService
{
    Product GetProduct(int id);

    Task CreateProductWithoutAudit(Product product);


    IEnumerable<Product> GetProducts();

    Task CreateProduct(Product product);

    Task UpdateProduct(Product product);

    Task DeleteProduct(int id);

    IEnumerable<Audit> GetAuditLogs();

}
