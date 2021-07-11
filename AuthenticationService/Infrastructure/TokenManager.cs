using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationServiceWithCustomJwt.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Infrastructure
{
    public class TokenManager : ITokenManager
    {
        private const string Username = "admin";
        private const string Password = "password";

        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly AuthorizationOptions _options;
        private readonly byte[] _secretKey;

        public TokenManager(IOptions<AuthorizationOptions> options)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _options = options.Value;
            _secretKey = Encoding.ASCII.GetBytes(_options.SecretKey);
        }

        // We usually authenticate via database or some other data storage.
        public bool Authenticate(string user, string password)
        {
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            return string.Equals(user, Username) && string.Equals(password, Password);
        }

        public string CreateToken()
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // The user info. It gets or sets the claims. The claims can store the following information:
                // * user's name
                // * user's email
                // * user's age
                // * user's authorization for an action
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "Ilya Valchanka")
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);
            var jwtString = _tokenHandler.WriteToken(token);

            return jwtString;
        }

        // Note: The VerifyToken method is not necessarily have to be inside the token manager.
        // Here in AuthenticationService we only need the authentication endpoint to generate the token.
        // It allows us to implement Single Sign-On (SSO). As long as each Web API service shares the same secret key
        // then they can use the token handler to verify the token.
        public ClaimsPrincipal VerifyToken(string token)
        {
            var validationParameters = new TokenValidationParameters
            {
                // Validates a signature of the token.
                ValidateIssuerSigningKey = true,
                // The key which is used for signature validation.
                IssuerSigningKey = new SymmetricSecurityKey(_secretKey),
                ValidateLifetime = true,
                // The audience refer to the Resource Servers that should accept the token such as https://contoso.com.
                ValidateAudience = false,
                // The issuer is an application which generates the token. Basically it's string or URI.
                ValidateIssuer = false,
                ClockSkew = TimeSpan.Zero
            };
            
            // If the validation will fail it will actually throws an exception.
            var claims = _tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return claims;
        }
    }
}