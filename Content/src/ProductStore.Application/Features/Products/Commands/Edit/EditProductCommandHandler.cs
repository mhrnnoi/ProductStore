using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductStore.Application.Interfaces.Persistence;
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
            return Error.NotFound(description: "something went wrong.. maybe you need to login again");

        var product = await _productRepository.GetUserProductByIdAsync(request.UserId,
                                                                            request.Id);

        if (product is null)
            return Error.NotFound(description: "product with this id is not exist in your product list..");

        var isUniqueByEmailAndDate = await _productRepository.IsEmailAndDateUniqueAsync(request.ManufactureEmail, request.ProduceDate);
        if (!isUniqueByEmailAndDate)
        {
            if (request.ManufactureEmail != product.ManufactureEmail && request.ProduceDate != product.ProduceDate)
            {
                return Error.Failure(description: "there is a product with this email and produce date ");
            }
        }

        Map(request, product);

        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync();
        _chacheService.RemoveData("products");
        return product;

    }

    private static void Map(EditProductCommand request, Product product)
    {
        product.IsAvailable = request.IsAvailable;
        product.ProduceDate = request.ProduceDate;
        product.Name = request.Name;
        product.ManufactureEmail = request.ManufactureEmail;
        product.ManufacturePhone = request.ManufacturePhone;
    }
}
