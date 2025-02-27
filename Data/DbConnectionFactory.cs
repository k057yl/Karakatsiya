using Karakatsiya.Interfaces;

namespace Karakatsiya.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbConnectionFactory(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetConnectionString(string role)
        {
            if (role == "Gala")
            {
                return _configuration.GetConnectionString("GalaConnection");
            }
            else
            {
                return _configuration.GetConnectionString("RegularUserConnection");
            }
        }
    }
}