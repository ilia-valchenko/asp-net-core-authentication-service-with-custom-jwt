using System;

namespace AuthenticationService.Infrastructure
{
    public class Token
    {
        public string Value { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}