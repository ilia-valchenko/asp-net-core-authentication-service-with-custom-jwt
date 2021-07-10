namespace AuthenticationService.Infrastructure
{
    public interface ITokenManager
    {
        bool Authenticate(string user, string password);

        Token CreateToken();

        bool VerifyToken(string token);
    }
}