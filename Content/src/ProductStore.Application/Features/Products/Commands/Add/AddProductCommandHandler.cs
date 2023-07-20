using ErrorOr;
using MapsterMapper;
using MediatR;
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

    public AddProductCommandHandler(IUnitOfWork unitOfWork,
                                    IProductRepository productRepository,
                                    IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _mapper = mapper;
    }


    public async Task<ErrorOr<Product>> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(request);
        _productRepository.Add(product);
        await _unitOfWork.SaveChangesAsync();

        return product;

    }
}
