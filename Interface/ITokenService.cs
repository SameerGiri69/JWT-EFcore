using JWT_EF_Core.Models;

namespace JWT_EF_Core.Interface
{
    public interface ITokenService 
    {
        string CreateToken(AppUser user);
    }
}
