using System.Text.Json;
using ProductStore.Application.Interfaces.Services;
using ProductStore.Domain.Abstractions;
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

    public T? GetData<T>(string key)
    {

        var value = _cacheDb.StringGet(key);
        if (!value.IsNullOrEmpty)
            return JsonSerializer.Deserialize<T>(value);
        return default;
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
            expiryTime = new TimeSpan(0, 1, 0);
        return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
    }


}
