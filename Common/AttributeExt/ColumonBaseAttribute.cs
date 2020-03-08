
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.AttributeExt
{
    public abstract class ColumonBaseAttribute:Attribute
    {
        public abstract bool Validate();
    }
}
