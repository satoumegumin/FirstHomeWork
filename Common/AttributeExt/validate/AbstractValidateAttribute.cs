using System;
using System.Collections.Generic;
using System.Text;

namespace Common.AttributeExt.validate
{
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Field)]
    public abstract class AbstractValidateAttribute:Attribute
    {
        public abstract bool Validate(object o);
    }
}
