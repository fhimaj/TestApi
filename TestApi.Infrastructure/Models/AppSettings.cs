namespace TestApi.Infrastructure.Models
{
    public class AppSettings
    {
        public JwtConfig JwtConfig { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class JwtConfig
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }

    public class ConnectionStrings
    {
        public string DbConnectionString { get; set; }
    }
}
