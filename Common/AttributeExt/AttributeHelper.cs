using Common.AttributeExt.validate;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Common.AttributeExt
{
    public static class AttributeHelper
    {
        public static string GetColName(this PropertyInfo property)
        {
            if (property.IsDefined(typeof(ColuAttribute), true))
            {
                ColuAttribute attr = (ColuAttribute)property.GetCustomAttribute(typeof(ColuAttribute),true);
                return attr.GetName();
            }
            return property.Name;
        }
        /// <summary>
        /// 后面使用表达式目录树进行优化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool Validate<T>(this T o) where T:BaseModel
        {
            Type type = o.GetType();
            foreach (var item in type.GetProperties())
            {
                object[] attr = item.GetCustomAttributes(typeof(AbstractValidateAttribute),true);
                foreach (AbstractValidateAttribute attribute in attr)
                {
                    if (!attribute.Validate(item.GetValue(o)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
