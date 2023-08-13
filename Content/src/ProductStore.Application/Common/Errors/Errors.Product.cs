using ErrorOr;

namespace ProductStore.Application.Common.Errors;

public static partial class Errors
{
    public static class Product
    {
        public static Error NameAlreadyExist => Error.Conflict(code: "Product.NameAlreadyExist",
                                                               description: "there is a product with this name ");
        public static Error UserDosentHaveTheProduct => Error.NotFound(code: "Product.UserDosentHaveTheProduct",
                                                               description: "product with this id is not exist in your product list..");
    }
}