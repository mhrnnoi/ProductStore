using ErrorOr;

namespace ProductStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class Product
    {
        public static Error NameAlreadyExist => Error.Conflict(code: "Product.NameAlreadyExist",
                                                               description: "there is a product with this name ");
        public static Error UserDosentHaveTheProduct => Error.NotFound(code: "Product.UserDosentHaveTheProduct",
                                                               description: "product with this id is not exist in your product list..");
        public static Error QuantityLowerThanZero => Error.Validation(code: "Product.QuantityLowerThanZero",
                                                               description: "quantity can't be below zero.");
        public static Error PriceLowerThanZero => Error.Validation(code: "Product.PriceLowerThanZero",
                                                               description: "Plz enter valid price");
        public static Error UserIdMinimumCharacter => Error.Validation(code: "Product.UserIdMinimumCharacter",
                                                               description: "something went wrong.. maybe you need to login again");
        public static Error NameMinimumCharacter => Error.Validation(code: "Product.NameMinimumCharacter",
                                                               description: "you should enter at least 2 character name");
    }
}