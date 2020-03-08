using System;
using System.Collections.Generic;
using System.Text;

namespace Common.AttributeExt
{
    [AttributeUsage(AttributeTargets.Property)]//只能修饰属性
    public class ColuAttribute:Attribute
    {
        public ColuAttribute(string name)
        {
            this._Name = name;
        }

        private string _Name = null;

        public string GetName()
        {
            return this._Name;
        }
    }
}
