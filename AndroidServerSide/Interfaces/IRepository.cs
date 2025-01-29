namespace AndroidServerSide.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task AddAsync(T entity);
        Task UpdateAsync(T existingEntity, T entity);
        Task DeleteAsync(string id);
    }
}
