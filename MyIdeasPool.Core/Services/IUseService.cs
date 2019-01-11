using MyIdeasPool.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyIdeasPool.Core.Services
{
	public interface IUserService
	{
		User CurrentUser { get; }
	}
}
