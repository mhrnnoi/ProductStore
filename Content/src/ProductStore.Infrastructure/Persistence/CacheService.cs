
using System.Text.Json;
using Microsoft.Extensions.Options;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Application.Interfaces.Services;
using ProductStore.Infrastructure.Services;
using StackExchange.Redis;

namespace ProductStore.Infrastructure.Persistence;

public class CacheService : ICacheService
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private IDatabase _cacheDb;
    private readonly JwtSettings _jwtOptions;


    public CacheService(IDateTimeProvider dateTimeProvider,
    IOptions<JwtSettings> jwtOptions)
    {
        var redis = ConnectionMultiplexer.Connect("redis:6379");
        _cacheDb = redis.GetDatabase();
        _dateTimeProvider = dateTimeProvider;
        _jwtOptions = jwtOptions.Value;
    }

    public bool AddToBlacklist(string token)
    {

        var expiryTime = _dateTimeProvider.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes)
                                                 .Subtract(_dateTimeProvider.UtcNow);
        if (_jwtOptions.ExpiryMinutes < 1)
            return false;

        return _cacheDb.StringSet(token,
                                  JsonSerializer.Serialize(true),
                                  expiryTime);
    }

    public T? GetData<T>(string key)
    {

        var value = _cacheDb.StringGet(key);
        if (!value.IsNullOrEmpty)
            return JsonSerializer.Deserialize<T>(value);
        return default;
    }

    public bool IsInBlacklist(string token)
    {
        var value = _cacheDb.StringGet(token);
        if (!value.IsNullOrEmpty)
            return JsonSerializer.Deserialize<bool>(value);
        return false;
    }

    public object RemoveData(string key)
    {
        var isExist = _cacheDb.KeyExists(key);
        if (isExist)
            return _cacheDb.KeyDelete(key);
        return false;
    }

    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var expiryTime = expirationTime.DateTime.Subtract(_dateTimeProvider.UtcNow);
        if (expirationTime <= _dateTimeProvider.UtcNow)
            return false;
        return _cacheDb.StringSet(key,
                                 JsonSerializer.Serialize(value),
                                 expiryTime);
    }
}
