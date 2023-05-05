namespace PasswordHash.Interfaces
{
    public interface IPasswordGenerator
    {
        public string GeneratePasswordHashUsingSalt(string password, byte[] salt);
    }
}
