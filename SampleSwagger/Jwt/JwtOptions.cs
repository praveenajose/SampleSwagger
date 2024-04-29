public class JwtOptions
{
    public string SigningKey { get; set; }
    public int ExpirationMinutes { get; set; }
}