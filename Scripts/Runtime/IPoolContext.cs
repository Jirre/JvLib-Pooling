namespace JvLib.Pooling
{
    public interface IPoolContext
    {
        string Id { get; }

        string GroupId { get; }
        float GroupWeight { get; }
    }
}
