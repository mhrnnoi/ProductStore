namespace ProductStore.Domain.Abstractions;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
    Task DisposeAsync();
}
