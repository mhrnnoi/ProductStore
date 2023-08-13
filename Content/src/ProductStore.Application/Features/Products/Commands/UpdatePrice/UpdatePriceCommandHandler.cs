using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductStore.Domain.Common.Errors;
using ProductStore.Domain.Abstractions;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Application.Features.Products.Commands.UpdatePrice;

public class UpdatePriceCommandHandler :
                    IRequestHandler<UpdatePriceCommand,
                                    ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ICacheService _chacheService;


    public UpdatePriceCommandHandler(IUnitOfWork unitOfWork,
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


    public async Task<ErrorOr<Product>> Handle(UpdatePriceCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
            return Errors.User.UserNotExist;

        var product = await _productRepository.GetUserProductByIdAsync(request.UserId,
                                                                            request.Id);

        if (product is null)
            return Errors.Product.UserDosentHaveTheProduct;

        var UpdateRes = UpdatePrice(request, product);

        if (UpdateRes.IsError)
            return UpdateRes.FirstError;

        _productRepository.Update(UpdateRes.Value);
        await _unitOfWork.SaveChangesAsync();
        _chacheService.RemoveData("products");
        return UpdateRes.Value;

    }

    private static ErrorOr<Product> UpdatePrice(UpdatePriceCommand request, Product product)
    {
        var updatePriceRes = product.UpdatePrice(request.price);

        if (updatePriceRes.IsError)
            return Errors.Product.PriceLowerThanZero;
        return updatePriceRes.Value;
    }


}
