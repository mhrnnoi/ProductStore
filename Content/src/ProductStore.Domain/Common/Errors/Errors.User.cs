using ErrorOr;

namespace ProductStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error UserNotExist => Error.NotFound(code: "User.UserNotExist",
                                                           description: "something went wrong.. maybe you need to login again");
    } 
}