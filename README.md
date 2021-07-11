### The content of the decoded JWT

```json
{
    "header":
    {
        "alg":"HS256",
        "typ":"JWT"
    }, 
    "payload":
    {
        "unique_name":"Ilya Valchanka",
        "nbf":1625949225,
        "exp":1625949285,
        "iat":1625949225
    }
}
```

A person who stole and modified the token will not be able to use it because the modified token will not pass a signature validation.