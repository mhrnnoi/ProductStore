namespace ProductStore.Application.Interfaces.Persistence;

public interface ICacheService
{
    T? GetData<T>(string key);
    bool SetData<T>(string key,
                    T value,
                    DateTimeOffset expirationTime);
    bool BlacklistToken(string token);
    bool IsBlacklistToken(string token);
    object RemoveData(string key);
    bool UserActiveToken(string userId, string token);
    bool BlacklistUserAllTokens(string userId);
}
