using ErrorOr;

namespace ProductStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class Auth
    {
        public static Error InvalidCred => Error.Validation(code: "Auth.InvalidCred",
                                                           description: "Bad Credential");
    } 
}