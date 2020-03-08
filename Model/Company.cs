using Common;
using Common.AttributeExt.validate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Company:BaseModel
    {
		[Regex(@"/^[\s\S]*.*[^\s][\s\S]*$/")]
		public string Name { get; set; }
		public DateTime? CreateTime { get; set; }
		public int? CreatorId { get; set; }
		public int? LastModifierId { get; set; }
		public DateTime? LastModifyTime { get; set; }
	}
}
