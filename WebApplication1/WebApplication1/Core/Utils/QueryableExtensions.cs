using System.Linq.Expressions;

namespace WebApplication1.Core.Utils
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> source, string sortBy, string direction)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return source;

            // Kiểm tra xem thuộc tính có tồn tại trong lớp T
            var propertyInfo = typeof(T).GetProperty(sortBy);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"The property '{sortBy}' does not exist on type '{typeof(T).Name}'.");
            }

            // Tạo một biểu thức cho thuộc tính sắp xếp
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyInfo);
            var orderByExpression = Expression.Lambda(property, parameter);

            // Lấy kiểu của thuộc tính để sử dụng cho OrderBy
            var orderByMethod = direction?.Equals("asc", StringComparison.OrdinalIgnoreCase) == true
                ? "OrderBy"
                : "OrderByDescending";

            // Tạo phương thức sắp xếp động
            var resultExpr = Expression.Call(typeof(Queryable), orderByMethod,
                new Type[] { typeof(T), propertyInfo.PropertyType }, source.Expression, orderByExpression);

            return source.Provider.CreateQuery<T>(resultExpr);
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            var skipResults = (pageNumber - 1) * pageSize; // Tính số kết quả cần bỏ qua
            return query.Skip(skipResults).Take(pageSize); // Lấy kết quả
        }
    }
}
