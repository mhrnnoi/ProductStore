using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductStore.Application.Interfaces.Persistence;
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
 private readonly ICacheService _cacheService;


    public AddProductCommandHandler(IUnitOfWork unitOfWork,
                                    IProductRepository productRepository,
                                    IMapper mapper,
                                    UserManager<IdentityUser> userManager,
                                    ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _mapper = mapper;
        _userManager = userManager;
        _cacheService = cacheService;
    }


    public async Task<ErrorOr<Product>> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
            return Error.Failure(description :"something went wrong.. maybe you need to login again");
        
        var product = _mapper.Map<Product>(request);
        var isUniqueByEmailAndDate =  await _productRepository.IsEmailAndDateUniqueAsync(request.ManufactureEmail, 
                                                                                        request.ProduceDate);
        if (!isUniqueByEmailAndDate)
            return Error.Failure(description :"there is a product with this email and produce date ");

        _productRepository.Add(product);
        await _unitOfWork.SaveChangesAsync();

        return product;

    }
}
