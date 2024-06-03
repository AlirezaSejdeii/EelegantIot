namespace EelegantIot.Api.Models;

public class JwtConfigDto
{
    public string EncryptionKey { get; set; }
    public string Secret { get; set; }
    public int ValidDays { get; set; }
    
}