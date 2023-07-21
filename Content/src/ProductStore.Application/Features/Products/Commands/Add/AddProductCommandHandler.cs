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


    public AddProductCommandHandler(IUnitOfWork unitOfWork,
                                    IProductRepository productRepository,
                                    IMapper mapper,
                                    UserManager<IdentityUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _mapper = mapper;
        _userManager = userManager;
    }


    public async Task<ErrorOr<Product>> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return Error.Failure();
        }
        var product = _mapper.Map<Product>(request);
        var isUniqueByEmailAndDate =  await _productRepository.IsEmailAndDateUniqueAsync(request.ManufactureEmail, 
                                                        request.ProduceDate);
        
        if (!isUniqueByEmailAndDate)
        {
            return Error.Failure();
        }
        _productRepository.Add(product);
        await _unitOfWork.SaveChangesAsync();

        return product;

    }
}
