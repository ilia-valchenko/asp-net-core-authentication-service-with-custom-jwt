using System.Security.Claims;

namespace AuthenticationService.Infrastructure
{
    public interface ITokenManager
    {
        bool Authenticate(string user, string password);

        string CreateToken();

        ClaimsPrincipal VerifyToken(string token);
    }
}