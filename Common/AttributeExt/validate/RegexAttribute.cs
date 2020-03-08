using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.AttributeExt.validate
{
    public class RegexAttribute : AbstractValidateAttribute
    {
        private string _RegexExpree = "";

        public RegexAttribute(string s)
        {
            this._RegexExpree = s;
        }
        public override bool Validate(object o)
        {
            return Regex.IsMatch(o.ToString(),this._RegexExpree);
        }
    }
}
