namespace ProductStore.Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
    Task DisposeAsync();
}
