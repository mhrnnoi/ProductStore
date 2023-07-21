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


    public EditProductCommandHandler(IUnitOfWork unitOfWork,
                                    IProductRepository productRepository,
                                    IMapper mapper,
                                    UserManager<IdentityUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _mapper = mapper;
        _userManager = userManager;
    }


    public async Task<ErrorOr<Product>> Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return Error.NotFound();
        }
        var product = await _productRepository.GetUserProductByIdAsync(request.UserId,
                                                                            request.id);
        if (product is null)
        {
            return Error.NotFound();
        }
        var isUniqueByEmailAndDate =  await _productRepository.IsEmailAndDateUniqueAsync(request.ManufactureEmail, request.ProduceDate)
        if (!isUniqueByEmailAndDate)
        {
            return Error.Failure();
        }
        var editedProduct = _mapper.Map<Product>(request);
        _productRepository.Update(editedProduct);
        await _unitOfWork.SaveChangesAsync();
        return editedProduct;

    }
}
