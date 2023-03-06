using Microsoft.Data.SqlClient;
using WebApp.Core.Helpers;

namespace WebApp.Core.Extentions
{
    public static class SqlParameterExtensions
    {
        public static List<SqlParameter> ToSqlParamsList(this object obj)
        {
            var props = (from p in obj.GetType().GetProperties()
                         select new { p.Name, Property = p }).ToList();
            var result = new List<SqlParameter>();
            props.ForEach(p =>
            {
                var sqlParam = new SqlParameter(p.Name.Replace("@", ""), SqlTypeConvertor.ToSqlDbType(p.Property.PropertyType))
                {
                    Value = p.Property.GetValue(obj) ?? DBNull.Value
                };

                result.Add(sqlParam);
            });

            return result;

        }
    }
}
