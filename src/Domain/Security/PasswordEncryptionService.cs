using SaltedPasswordHashing.Src.Domain.Types;

namespace SaltedPasswordHashing.Src.Domain.Security
{
    public interface PasswordEncryptionService
    {
        string Encrypt(string password);
        bool Verify(string hashedPassword, string passwordIntent);
    }
}