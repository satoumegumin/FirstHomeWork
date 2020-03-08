using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class GetTable
    {
        public static string TableName { get; set; }

        public static string TableColString { get; set; }

        //static GetTable()
        //{
        //    Type type = typeof(T);
        //    TableName = type.Name;
        //    TableColString =string.Join(',', type.GetProperties().Select(p => p.Name));
        //}

    }
}
