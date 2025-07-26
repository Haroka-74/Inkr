namespace Inkr
{
    public static class AppSettings
    {
        public static LoggingConfig Logging { get; private set; } = null!;
        public static BackendConfig Backend { get; private set; } = null!;
        public static ConnectionStringsConfig ConnectionStrings { get; private set; } = null!;
        public static JwtConfig JWT { get; private set; } = null!;
        public static RefreshTokenConfig RefreshToken { get; private set; } = null!;
        public static EmailConfig Email { get; private set; } = null!;
        public static string AdminInviteKey { get; private set; } = null!;

        public static void Initialize(IConfiguration configuration)
        {
            Logging = new LoggingConfig(configuration);
            Backend = new BackendConfig(configuration);
            ConnectionStrings = new ConnectionStringsConfig(configuration);
            JWT = new JwtConfig(configuration);
            RefreshToken = new RefreshTokenConfig(configuration);
            Email = new EmailConfig(configuration);
            AdminInviteKey = configuration["AdminInviteKey"] ?? "ABC123";
        }

        public class LoggingConfig(IConfiguration configuration)
        {
            public string Default { get; } = configuration["Logging:LogLevel:Default"] ?? "Information";
            public string MicrosoftAspNetCore { get; } = configuration["Logging:LogLevel:Microsoft.AspNetCore"] ?? "Warning";
        }

        public class BackendConfig(IConfiguration configuration)
        {
            public string Protocol { get; } = configuration["Backend:Protocol"] ?? "https";
            public string Domain { get; } = configuration["Backend:Domain"] ?? "localhost";
            public int Port { get; } = int.Parse(configuration["Backend:Port"] ?? "7235");
            public string URL => $"{Protocol}://{Domain}:{Port}";
        }

        public class ConnectionStringsConfig(IConfiguration configuration)
        {
            public string Default { get; } = configuration["ConnectionStrings:cs"] ?? throw new InvalidOperationException("Connection string is required");
        }

        public class JwtConfig(IConfiguration configuration)
        {
            public string SecretKey { get; } = configuration["JWT:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is required");
            public string Issuer { get; } = configuration["JWT:Issuer"] ?? "https://localhost:7088/";
            public string Audience { get; } = configuration["JWT:Audience"] ?? "http://localhost:4200/";
            public int ExpiryMinutes { get; } = int.Parse(configuration["JWT:ExpiryMinutes"] ?? "1");
        }

        public class RefreshTokenConfig(IConfiguration configuration)
        {
            public int ExpiryDays { get; } = int.Parse(configuration["RefreshToken:ExpiryDays"] ?? "7");
        }

        public class EmailConfig(IConfiguration configuration)
        {
            public SmtpConfig Smtp { get; } = new SmtpConfig(configuration);

            public class SmtpConfig(IConfiguration configuration)
            {
                public string Host { get; } = configuration["Email:Smtp:Host"] ?? "smtp.gmail.com";
                public int Port { get; } = int.Parse(configuration["Email:Smtp:Port"] ?? "587");
                public string Username { get; } = configuration["Email:Smtp:Username"] ?? throw new InvalidOperationException("Email username is required");
                public string Password { get; } = configuration["Email:Smtp:Password"] ?? throw new InvalidOperationException("Email password is required");
                public string Sender { get; } = configuration["Email:Smtp:Sender"] ?? throw new InvalidOperationException("Email sender is required");
            }
        }
    }
}