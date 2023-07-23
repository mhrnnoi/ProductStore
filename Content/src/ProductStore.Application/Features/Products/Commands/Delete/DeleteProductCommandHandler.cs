using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductStore.Application.Interfaces.Persistence;

namespace ProductStore.Application.Features.Products.Commands.Delete;

public class DeleteProductCommandHandler :
                    IRequestHandler<DeleteProductCommand,
                                    ErrorOr<bool>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;


    public DeleteProductCommandHandler(IUnitOfWork unitOfWork,
                                    IProductRepository productRepository,
                                    IMapper mapper,
                                    UserManager<IdentityUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _mapper = mapper;
        _userManager = userManager;
    }


    public async Task<ErrorOr<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
            return Error.Failure("something went wrong..");

        var product = await _productRepository.GetUserProductByIdAsync(request.UserId,
                                                                       request.ProductId);
        if (product is null)
            return Error.NotFound("product with this id is not exist in your product list..");

        _productRepository.Remove(product);

        return true;

    }
}
