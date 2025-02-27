namespace Karakatsiya.Interfaces
{
    public interface IDbConnectionFactory
    {
        string GetConnectionString(string role);
    }
}
