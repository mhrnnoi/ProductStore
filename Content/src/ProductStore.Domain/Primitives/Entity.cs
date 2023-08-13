namespace ProductStore.Domain.Primitives;
public abstract class Entity : IEquatable<Entity>
{
    public string Id { get; private init; }
    public static bool operator ==(Entity? first, Entity? second)
    {
        return first is not null && second is not null && first.Equals(second);
    }
    public static bool operator !=(Entity? first, Entity? second)
    {
        return !(first == second);
    }

    protected Entity()
    {
        Id = Guid.NewGuid().ToString();
    }
    public override bool Equals(object? obj)
    {
        if (obj is not null)
        {
            if (obj.GetType() != GetType())
            {
                if (obj is Entity entity)
                    return Id == entity.Id;
            }
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public bool Equals(Entity? other)
    {
        if (other is not null)
        {
            if (other.GetType() == GetType())
            {
                return Id == other.Id;
            }
        }

        return false;
    }
}