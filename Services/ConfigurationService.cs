

namespace ProductApi.Services;

public class ConfigurationService: IConfigurationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IConfiguration _configuration;

    public ConfigurationService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public string GetCurrentUserId() =>
        _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

    public decimal GetDiscount() => Convert.ToDecimal(_configuration["discount"]);


}
