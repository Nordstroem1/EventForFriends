namespace Application.Interfaces
{
    public interface IMySqlContext
    {
        Task LoadCollectionAsync<TEntity>(TEntity entity, string collectionName) where TEntity : class;
    }
}

