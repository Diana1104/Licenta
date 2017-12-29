using System.Collections.Generic;
using System.Linq;

namespace Licenta.ORM
{
    public class Sql
    {
        public static string GetSelectStatement(string tableName, List<string> columns)
        {
            string queryTemplate = "SELECT {0} FROM [dbo].[{1}]";
            string columnNames = string.Join(",", columns.Select(column => string.Format("[{0}]", column)));
            return string.Format(queryTemplate, columnNames, tableName);
        }

        public static string GetInsertStatement(string tableName, List<string> columns)
        {
            string queryTemplate = "INSERT INTO [dbo].[{0}] ({1}) VALUES ({2})";
            string columnNames = string.Join(",", columns.Select(column => string.Format("[{0}]", column)));
            string parameterNames = string.Join(",", columns.Select(column => string.Format("@{0}", column)));
            return string.Format(queryTemplate, tableName, columnNames, parameterNames);
        }

        public static string GetDeleteStatement(string tableName, List<string> columns)
        {
            string queryTemplate = "DELETE FROM [dbo].[{0}] WHERE {1}";
            string searchCondition = string.Join(" AND ", columns.Select(column => string.Format("[{0}] = @{0}", column)));
            return string.Format(queryTemplate, tableName, searchCondition);
        }
    }
}
