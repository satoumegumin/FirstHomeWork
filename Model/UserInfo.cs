using Common;
using Common.AttributeExt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model
{
    public class UserInfo:BaseModel
    {
		public string Name { get; set; }
		public string Account { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public int? CompanyId { get; set; }
		public string CompanyName { get; set; }
		//public int? State { get; set; }tim
		[Colu("state")]
		public int? Status { get; set; }
		public DateTime? CreateTime { get; set; }
		public DateTime? LastLoginTime { get; set; }
		public int? CreatorId { get; set; }
		public int? LastModifierID { get; set; }
		public DateTime? LastModifyTime { get; set; }

	}
}
