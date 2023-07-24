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
    private readonly ICacheService _chacheService;


    public GetAllProductsQueryHandler(IProductRepository productRepository,
                                      IMapper mapper,
                                      ICacheService chacheService)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _chacheService = chacheService;
    }

    public async Task<ErrorOr<List<Product>>> Handle(GetAllProductsQuery request,
                                                     CancellationToken cancellationToken)
    {
        var productsInRedis = _chacheService.GetData<List<Product>>("products");
        if (productsInRedis is null || productsInRedis.Count < 1)
        {
            var expiryTime = DateTimeOffset.Now.AddSeconds(30);
            var products = await _productRepository.GetAllAsync();
            _chacheService.SetData<List<Product>>("products", products, expiryTime);
            return products;
        }
        return productsInRedis;
    }
}
