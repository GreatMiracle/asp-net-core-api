namespace WebApplication1.Infrastructure.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);

        // Thêm phương thức cho truy vấn phức tạp
        Task<IEnumerable<T>> GetByConditionAsync(Func<T, bool> predicate);
    }
}
