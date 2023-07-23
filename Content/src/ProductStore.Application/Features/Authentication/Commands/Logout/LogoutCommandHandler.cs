using ErrorOr;
using MediatR;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Application.Interfaces.Services;

namespace ProductStore.Application.Features.Authentication.Commands.Logout;

public class LogoutCommandHandler :
                    IRequestHandler<LogoutCommand,
                                    ErrorOr<bool>>
{

 private readonly ICacheService _cacheService;
 private readonly IDateTimeProvider _dateTimeProvider;

    public LogoutCommandHandler(ICacheService cacheService, IDateTimeProvider dateTimeProvider)
    {
        _cacheService = cacheService;
        _dateTimeProvider = dateTimeProvider;
    }


    public async Task<ErrorOr<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        _cacheService.AddBlacklist(request.Token, _dateTimeProvider.UtcNow.AddMinutes(5));
        return true;
    }
}
