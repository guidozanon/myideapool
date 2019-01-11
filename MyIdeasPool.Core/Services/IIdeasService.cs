using MyIdeasPool.Core.Models;
using System.Linq;

namespace MyIdeasPool.Core.Services
{
	public interface IIdeasService
	{
		IQueryable<Idea> List();
	}
}
