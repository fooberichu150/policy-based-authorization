using System.Collections.Generic;

namespace PolicyBasedAuthorization.Models
{
	public class BasicUser
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public IReadOnlyList<string> Roles { get; set; }
	}
}
