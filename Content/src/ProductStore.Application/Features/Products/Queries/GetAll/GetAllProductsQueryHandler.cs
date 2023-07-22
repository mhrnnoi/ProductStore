using ErrorOr;
using MapsterMapper;
using MediatR;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Application.Features.Products.Queries.GetAll;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, ErrorOr<List<Product>>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;


    public GetAllProductsQueryHandler(IProductRepository productRepository,
                                      IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<List<Product>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        return await _productRepository.GetAllAsync();
    }
}
