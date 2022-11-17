namespace ProductApi.Services.ServiceInterfaces;

public interface IMailService
{
    Task SendConfirmationMessage( string address);

    Task<string> GetConfirmationKey(string address);

    Task<bool> Confirm(string address, string key);
}
