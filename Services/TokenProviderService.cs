namespace ProductApi.Services;

public class TokenProviderService : IUserTwoFactorTokenProvider<AppUser>
{

    public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<AppUser> manager, AppUser user)
    {
        return Task.FromResult(true);
    }

    public Task<string> GenerateAsync(string purpose, UserManager<AppUser> manager, AppUser user)
    {
        return Task.FromResult("'");
    }

    public Task<bool> ValidateAsync(string purpose, string token, UserManager<AppUser> manager, AppUser user)
    {
        throw new NotImplementedException();
    }
}
