namespace ProductStore.Application.Interfaces.Persistence;

    public interface ICacheService 
    {
        T? GetData<T>(string key);
        bool SetData<T>(string key,
                        T value,
                        DateTimeOffset expirationTime);
        bool AddToBlacklist(string token);
        bool IsInBlacklist(string token);
        object RemoveData(string key);
    }
