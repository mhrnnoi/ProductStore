
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

    public bool BlacklistToken(string token)
    {
        if (_jwtOptions.ExpiryMinutes < 1)
            _jwtOptions.ExpiryMinutes = 1;
        var expiryTime = _dateTimeProvider.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes)
                                                 .Subtract(_dateTimeProvider.UtcNow);

        return _cacheDb.StringSet(token, "BlackList", expiryTime);

    }


    public T? GetData<T>(string key)
    {

        var value = _cacheDb.StringGet(key);
        if (!value.IsNullOrEmpty)
            return JsonSerializer.Deserialize<T>(value);
        return default;
    }

    public bool IsBlacklistToken(string token)
    {
        var value = _cacheDb.StringGet(token);
        if (!value.IsNullOrEmpty)
            return true;
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
            expiryTime = new TimeSpan(0, 1, 0);
        return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
    }
    public bool IsTokenInSet(string userId, string token)
    {
        return _cacheDb.SetContains(userId, token);
    }

    public bool UserActiveToken(string userId, string token)
    {

        if (_jwtOptions.ExpiryMinutes <= 1)
            _jwtOptions.ExpiryMinutes = 1;
        var expiryTime = _dateTimeProvider.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes).Subtract(_dateTimeProvider.UtcNow);
        if (!IsTokenInSet(userId, token))
        {
            var addResult = _cacheDb.SetAdd(userId, token);
            var setExpireResult = _cacheDb.KeyExpire(userId, expiryTime);
            if (addResult && setExpireResult)
            {
                return true;
            }
            return false;
        }

        return true;


    }
    public bool BlacklistUserAllTokens(string userId)
    {

        if (_jwtOptions.ExpiryMinutes < 1)
            _jwtOptions.ExpiryMinutes = 1;
        var expiryTime = _dateTimeProvider.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes)
                                                 .Subtract(_dateTimeProvider.UtcNow);
        var tokens = _cacheDb.SetMembers(userId);
        bool result = true;
        if (tokens.Count() > 0)
        {
            RedisKey key;
            foreach (var item in tokens)
            {
                key = item.ToString();
                result = _cacheDb.StringSet(key, "BlackList", expiryTime);
                if (result is false)
                    return false;
            }

        }
        return result;

    }





}
