using System;
using System.Collections.Generic;

namespace AuthenticationService.Infrastructure
{
    public class TokenManager : ITokenManager
    {
        private const string Username = "admin";
        private const string Password = "password";

        private readonly IDictionary<string, Token> _tokens;

        public TokenManager()
        {
            _tokens = new Dictionary<string, Token>();
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

        public Token CreateToken()
        {
            var token = new Token
            {
                Value = Guid.NewGuid().ToString(),
                ExpirationDate = DateTime.Now.AddMinutes(10)
            };

            _tokens.Add(token.Value, token);
            return token;
        }

        public bool VerifyToken(string token)
        {
            if (token == null)
            {
                return false;
            }

            _tokens.TryGetValue(token, out Token tokenFromStorage);

            if (tokenFromStorage == null)
            {
                return false;
            }

            return string.Equals(token, tokenFromStorage.Value) && tokenFromStorage.ExpirationDate > DateTime.Now;
        }
    }
}