namespace ProductApi.Models.DTO
{
    public class UserLoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public bool IsAuthSuccessful { get; set; } = true;
        public List<string> ErrorMessage { get; set; } = new List<string>();
    }
}
