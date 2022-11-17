namespace ProductApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IConfigurationService _configurationService;

    public ProductService(IProductRepository productRepository, IConfigurationService configurationService)
    {
        _productRepository = productRepository;
        _configurationService = configurationService;
    }

    public async Task CreateProduct(Product product)
    {
        await _productRepository.CreateProduct(product);
        await _productRepository.SaveChangesAsync(_configurationService.GetCurrentUserId());
    }

    public async Task CreateProductWithoutAudit(Product product)
    {
        await _productRepository.CreateProduct(product);
        await _productRepository.SaveChangesAsync("Application");
    }

    public async Task DeleteProduct(int id)
    {
        await _productRepository.DeleteProduct(id);
        await _productRepository.SaveChangesAsync(_configurationService.GetCurrentUserId());
    }

    public Product GetProduct(int id) => _productRepository.Get(id);

    public IEnumerable<Product> GetProducts() => _productRepository.GetProducts();

    public async Task UpdateProduct(Product product)
    {
        await _productRepository.UpdateProduct(product);
        await _productRepository.SaveChangesAsync(_configurationService.GetCurrentUserId());
    }

    public IEnumerable<Audit> GetAuditLogs() => _productRepository.ProductAudits;

}
