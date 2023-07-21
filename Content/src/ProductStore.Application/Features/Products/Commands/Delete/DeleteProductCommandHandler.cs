using ErrorOr;
using MapsterMapper;
using MediatR;
using ProductStore.Application.Interfaces.Persistence;

namespace ProductStore.Application.Features.Products.Commands.Delete;

public class DeleteProductCommandHandler :
                    IRequestHandler<DeleteProductCommand,
                                    ErrorOr<bool>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork,
                                    IProductRepository productRepository,
                                    IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _mapper = mapper;
    }


    public async Task<ErrorOr<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product =  await _productRepository.GetUserProductByIdAsync(request.UserId,
                                                                            request.ProductId);
        if (product is null)
        {
            return Error.NotFound();
        }
        _productRepository.Remove(product);
        
        return true;

    }
}
