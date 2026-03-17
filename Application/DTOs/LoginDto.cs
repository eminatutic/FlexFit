namespace FlexFit.Application.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;

        public string? Password { get; set; }

  
        public bool IsGoogle { get; set; } = false;

    }
}