using System.Text;

namespace ProductApi.Services;

public class MailService : IMailService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public MailService(ApplicationDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<string> GetConfirmationKey(string address) =>
        await _userManager.GenerateEmailConfirmationTokenAsync(await _userManager.FindByEmailAsync(address));
        
        
        /*_context.MailConfirmations.Any(x => x.address == address)
            ? _context.MailConfirmations.First(x => x.address == address).key : "";
        */

    public async Task SendConfirmationMessage( string address)
    {
        SmtpClient smtpClient = new SmtpClient(String.Empty);
        MailAddress from = new MailAddress("qudratovsultonxon20011124@gmail.com",
            "Jwt Demo Project", Encoding.UTF8);
        MailAddress to = new MailAddress(address);
        MailMessage message = new MailMessage(from, to);

        var key = await GetConfirmationKey(address);
        Console.WriteLine("=========================>" + key);
        message.Body = $"Your email comfirmation key is: {key}";
        message.Subject = "Email COnfirmation";
        smtpClient.Send(message);
        _context.MailConfirmations.Add(new MailConfirmation { address = address, key = key });
        _context.SaveChanges();
    }

    

    public async Task<bool> Confirm(string address, string key)
    {
        var user = await _userManager.FindByEmailAsync(address);

        var result = await _userManager.ConfirmEmailAsync(user, key);

        return result.Succeeded;

    }
    
    


}
