
using System.Text.Json;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Application.Interfaces.Services;
using StackExchange.Redis;

namespace ProductStore.Infrastructure.Persistence;

public class CacheService : ICacheService
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private IDatabase _cacheDb;

    public CacheService(IDateTimeProvider dateTimeProvider)
    {
        var redis = ConnectionMultiplexer.Connect("redis:6379");
        _cacheDb = redis.GetDatabase();
        _dateTimeProvider = dateTimeProvider;
    }

    public bool AddBlacklist(string token, DateTimeOffset expirationTime)
    {
        var expiryTime = expirationTime.DateTime.Subtract(_dateTimeProvider.UtcNow);
        return _cacheDb.StringSet(token, JsonSerializer.Serialize(true), expiryTime);
    }

    public T? GetData<T>(string key)
    {

        var value = _cacheDb.StringGet(key);
        if (!value.IsNullOrEmpty)
        {
            return JsonSerializer.Deserialize<T>(value);

        }
        return default;
    }

    public bool IsBlacklist(string token)
    {
        var value = _cacheDb.StringGet(token);
        if (!value.IsNullOrEmpty)
        {
            return true;

        }
        return false;
    }

    public object RemoveData(string key)
    {
        var _exist = _cacheDb.KeyExists(key);
        if (_exist)
        {
            return _cacheDb.KeyDelete(key);
        }
        return false;
    }

    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var expiryTime = expirationTime.DateTime.Subtract(_dateTimeProvider.UtcNow);
        return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
    }
}