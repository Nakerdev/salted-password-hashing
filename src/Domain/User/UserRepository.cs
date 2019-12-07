using SaltedPasswordHashing.Src.Domain.Types;

namespace SaltedPasswordHashing.Src.Domain.User
{
    public interface UserRepository
    {
        User Create(User user);
        bool Exist(Email email);
    }
}