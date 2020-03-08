using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.AttributeExt.validate
{
    public class EmailAttribute : AbstractValidateAttribute
    {
        public override bool Validate(object o)
        {
            return o != null &&
                Regex.IsMatch(o.ToString(), @"^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$");
        }

    }
}
