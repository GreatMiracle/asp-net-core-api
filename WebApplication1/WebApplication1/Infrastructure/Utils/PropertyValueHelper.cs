using System.Reflection;

namespace WebApplication1.Infrastructure.Utils
{
    public class PropertyValueHelper
    {
        // Lấy giá trị kiểu string với xử lý null
        public static string GetStringValueOrDefault(object obj, string propertyName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Property name cannot be null or empty.", nameof(propertyName));
            }

            var property = obj.GetType().GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (property == null)
            {
                return string.Empty;
            }

            var value = property.GetValue(obj);
            return value?.ToString() ?? string.Empty;
        }

        // Lấy giá trị kiểu int với xử lý null
        public static int GetIntValueOrDefault(object r, string propertyName)
        {
            var property = r.GetType().GetProperty(propertyName);
            if (property != null)
            {
                var value = property.GetValue(r);
                return value is int intValue ? intValue : 0; // Trả về giá trị int hoặc 0 nếu không phải int
            }
            return 0; // Trả về 0 nếu thuộc tính không tồn tại
        }

        // Lấy giá trị kiểu bool với xử lý null
        public static bool GetBoolValueOrDefault(object r, string propertyName)
        {
            var property = r.GetType().GetProperty(propertyName);
            if (property != null)
            {
                var value = property.GetValue(r);
                return value is bool boolValue && boolValue; // Trả về giá trị bool hoặc false nếu không phải bool
            }
            return false; // Trả về false nếu thuộc tính không tồn tại
        }

        // Lấy giá trị kiểu Guid với xử lý null
        public static Guid GetGuidValueOrDefault(object r, string propertyName)
        {
            var property = r.GetType().GetProperty(propertyName);
            if (property != null)
            {
                var value = property.GetValue(r);
                return value is Guid guidValue ? guidValue : Guid.Empty; // Trả về giá trị Guid hoặc Guid.Empty nếu không phải Guid
            }
            return Guid.Empty; // Trả về Guid.Empty nếu thuộc tính không tồn tại
        }

        // Lấy giá trị kiểu DateTime với xử lý null
        public static DateTime GetDateTimeValueOrDefault(object r, string propertyName)
        {
            var property = r.GetType().GetProperty(propertyName);
            if (property != null)
            {
                var value = property.GetValue(r);
                return value is DateTime dateTimeValue ? dateTimeValue : DateTime.MinValue; // Trả về giá trị DateTime hoặc DateTime.MinValue nếu không phải DateTime
            }
            return DateTime.MinValue; // Trả về DateTime.MinValue nếu thuộc tính không tồn tại
        }
    }
}
