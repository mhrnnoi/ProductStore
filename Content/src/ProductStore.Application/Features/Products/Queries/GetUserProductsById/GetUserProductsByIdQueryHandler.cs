using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Application.Features.Products.Queries.GetUserProductsById;

public class GetUserProductsByIdQueryHandler : IRequestHandler<GetUserProductsByIdQuery, ErrorOr<List<Product>>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public GetUserProductsByIdQueryHandler(IProductRepository productRepository,
                                           IMapper mapper,
                                           UserManager<IdentityUser> userManager)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _userManager = userManager;
    }
    public async Task<ErrorOr<List<Product>>> Handle(GetUserProductsByIdQuery request,
                                                     CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
            return Error.NotFound("something went wrong..");

        return await _productRepository.GetUserProductsAsync(request.UserId);

    }
}

