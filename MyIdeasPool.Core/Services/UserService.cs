using System.Linq;
using AutoMapper;
using MyIdeasPool.Core.DAL;
using MyIdeasPool.Core.Domain;
using MyIdeasPool.Core.Models;

namespace MyIdeasPool.Core.Services
{
	class UserService : IUserService
	{
		private readonly IdeasContext _context;
		private readonly IMapper _mapper;

		public User CurrentUser { get; private set; }

		internal UserEntity CurrentUserInternal { get; private set; }

		public UserService(IdeasContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}


		public void SetCurrentUser(string username)
		{
			if (CurrentUser == null)
			{
				CurrentUserInternal = _context.Users.Where(u => u.UserName == username).FirstOrDefault();

				CurrentUser = _mapper.Map<User>(CurrentUserInternal);
			}
		}
	}
}
