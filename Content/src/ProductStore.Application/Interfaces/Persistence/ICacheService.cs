namespace ProductStore.Application.Interfaces.Persistence;

    public interface ICacheService
    {
        T? GetData<T>(string key);
        bool SetData<T>(string key, T value, DateTimeOffset expirationTime);
        bool AddBlacklist(string token, DateTimeOffset expirationTime);
        bool IsBlacklist(string token);
        object RemoveData(string key);
    }
