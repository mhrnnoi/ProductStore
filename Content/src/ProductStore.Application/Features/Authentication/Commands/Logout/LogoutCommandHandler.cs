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
        var result = _cacheService.BlacklistToken(request.Token);
        if (result is false)
            return Error.Failure("Something Went Wrong..");
        return true;

    }
}
