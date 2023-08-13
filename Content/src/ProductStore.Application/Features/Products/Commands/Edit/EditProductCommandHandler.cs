using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductStore.Application.Common.Errors;
using ProductStore.Domain.Abstractions;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Application.Features.Products.Commands.Edit;

public class EditProductCommandHandler :
                    IRequestHandler<EditProductCommand,
                                    ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ICacheService _chacheService;


    public EditProductCommandHandler(IUnitOfWork unitOfWork,
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


    public async Task<ErrorOr<Product>> Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
            return Errors.User.UserNotExist;

        var product = await _productRepository.GetUserProductByIdAsync(request.UserId,
                                                                            request.Id);

        if (product is null)
            return Errors.Product.UserDosentHaveTheProduct;

        product =  Edit(request, product);

        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync();
        _chacheService.RemoveData("products");
        return product;

    }

    private static Product Edit(EditProductCommand request, Product product)
    {
        product =  product.UpdateQuantity(request.quantity);
        return product.UpdatePrice(request.price);
    }


}
