namespace Leaguerly.Repositories.DataModels
{
    public interface IDbEntity<TKey>
    {
        TKey Id { get; set; }
    }
}