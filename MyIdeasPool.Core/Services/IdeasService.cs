using AutoMapper.QueryableExtensions;
using MyIdeasPool.Core.DAL;
using MyIdeasPool.Core.Models;
using System.Linq;

namespace MyIdeasPool.Core.Services
{
	class IdeasService : IIdeasService
	{
		private readonly IdeasContext _context;
		private readonly IUserService _userService;
		public IdeasService(IdeasContext context, IUserService userService)
		{
			_context = context;
			_userService = userService;
		}

		public IQueryable<Idea> List()
		{
			return _context.Ideas
				.Where(i=>i.Owner.Id == _userService.CurrentUser.Id)
				.ProjectTo<Idea>();
		}
	}
}
