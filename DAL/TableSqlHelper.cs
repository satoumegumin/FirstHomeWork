using Common;
using Common.AttributeExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class TableSqlHelper<T>where T:BaseModel
    {
        static TableSqlHelper()
        {
            Type type = typeof(T);
            string colString = string.Join(',', type.GetProperties().Select(t => t.GetColName()));
            string tableName = type.Name;
        }
        public static string FindSql { get; set; }

    }
}
