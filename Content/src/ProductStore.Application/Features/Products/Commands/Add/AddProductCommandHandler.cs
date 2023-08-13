using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductStore.Domain.Common.Errors;
using ProductStore.Domain.Abstractions;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Application.Features.Products.Commands.Add;

public class AddProductCommandHandler :
                    IRequestHandler<AddProductCommand,
                                    ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ICacheService _chacheService;

    public AddProductCommandHandler(IUnitOfWork unitOfWork,
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


    public async Task<ErrorOr<Product>> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {

        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
            return Errors.User.UserNotExist;

        var product = await _productRepository.GetProductByNameAsync(request.Name);
        if (product is not null)
            return Errors.Product.NameAlreadyExist;

        var productOrError = Product.Create(request.UserId,
                                            request.Name,
                                            request.quantity,
                                            request.price);
        if (productOrError.IsError)
            return productOrError.FirstError;

        _productRepository.Add(productOrError.Value);
        await _unitOfWork.SaveChangesAsync();
        _chacheService.RemoveData("products");

        return productOrError.Value;

    }

}
