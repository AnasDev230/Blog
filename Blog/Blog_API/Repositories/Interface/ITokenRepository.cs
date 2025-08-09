using Microsoft.AspNetCore.Identity;

namespace Blog_API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user,List<string> roles);
    }
}
