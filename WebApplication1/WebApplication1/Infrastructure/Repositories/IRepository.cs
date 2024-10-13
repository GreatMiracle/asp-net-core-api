using Azure.Core;

namespace WebApplication1.Infrastructure.Repositories
{
    public interface IRepository<TRequest, TResponse> where TRequest : class
    {
        Task<IEnumerable<TResponse>> GetAllAsync();
        Task<TResponse?> GetByIdAsync(Guid id);
        Task<TResponse> AddAsync(TResponse entity);
        Task<TResponse> UpdateAsync(Guid id , TResponse entity);
        Task<bool> DeleteAsync(Guid id);

        // Thêm phương thức cho truy vấn phức tạp
        Task<IEnumerable<TResponse>> GetByConditionAsync(Func<TRequest, bool> predicate);
    }
}
