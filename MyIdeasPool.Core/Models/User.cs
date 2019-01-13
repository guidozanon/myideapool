using System;
using System.Collections.Generic;
using System.Text;

namespace MyIdeasPool.Core.Models
{
	public class User
	{
		public string Id { get; internal set; }
		public string Name { get; set; }
		public string Email { get; set; }
	}
}
