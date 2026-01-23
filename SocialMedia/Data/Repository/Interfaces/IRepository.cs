namespace SocialMedia.Data.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);

        Task UpdateAsync(T entity);

        Task<T?> GetByIdAsync(Guid id);
    }
}
