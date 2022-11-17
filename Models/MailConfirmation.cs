namespace ProductApi.Models;

public class MailConfirmation
{
    public int Id { get; set; }
    public string address { get; set; }
    public string key { get; set; }
}
