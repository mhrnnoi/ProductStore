using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductStore.Domain.Common.Errors;
using ProductStore.Domain.Abstractions;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Application.Features.Products.Commands.UpdateQuantity;

public class UpdateQuantityCommandHandler :
                    IRequestHandler<UpdateQuantityCommand,
                                    ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ICacheService _chacheService;


    public UpdateQuantityCommandHandler(IUnitOfWork unitOfWork,
                                    IProductRepository productRepository,
                                    IMapper mapper,
                                    UserManager<IdentityUser> userManager,
                                    ICacheService chacheService)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _mapper = mapper;
        _userManager = userManager;
        _chacheService = chacheService;
    }


    public async Task<ErrorOr<Product>> Handle(UpdateQuantityCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
            return Errors.User.UserNotExist;

        var product = await _productRepository.GetUserProductByIdAsync(request.UserId,
                                                                            request.Id);

        if (product is null)
            return Errors.Product.UserDosentHaveTheProduct;

        var updateQuantityRes = UpdateQuantity(request, product);

        if (updateQuantityRes.IsError)
            return updateQuantityRes.FirstError;

        _productRepository.Update(updateQuantityRes.Value);
        await _unitOfWork.SaveChangesAsync();
        _chacheService.RemoveData("products");
        return updateQuantityRes.Value;

    }

    private static ErrorOr<Product> UpdateQuantity(UpdateQuantityCommand request, Product product)
    {
        var updateQuantityRes = product.UpdateQuantity(request.quantity);

        if (updateQuantityRes.IsError)
            return Errors.Product.QuantityLowerThanZero;
        return updateQuantityRes.Value;
    }


}
