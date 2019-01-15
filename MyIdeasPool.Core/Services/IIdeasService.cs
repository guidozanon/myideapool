using MyIdeasPool.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdeasPool.Core.Services
{
	public interface IIdeasService
	{
		IQueryable<Idea> List();
		Task<Idea> Save(Idea idea);
		Task<Idea> Get(Guid id);
		Task<Idea> Update(Idea idea);
		Task Delete(Guid id);
	}
}
