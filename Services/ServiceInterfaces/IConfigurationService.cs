namespace ProductApi.Services.ServiceInterfaces
{
    public interface IConfigurationService
    {
        string GetCurrentUserId();

        decimal GetDiscount();
    }
}
