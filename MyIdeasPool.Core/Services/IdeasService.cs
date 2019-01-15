using AutoMapper;
using AutoMapper.QueryableExtensions;
using MyIdeasPool.Core.DAL;
using MyIdeasPool.Core.Domain;
using MyIdeasPool.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdeasPool.Core.Services
{
	class IdeasService : IIdeasService
	{
		private readonly IdeasContext _context;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public IdeasService(IdeasContext context, IUserService userService, IMapper mapper)
		{
			_context = context;
			_userService = userService;
			_mapper = mapper;
		}

		public async Task Delete(Guid id)
		{
			var idea = await _context.Ideas.FindAsync(id);

			if (idea != null && idea.OwnerId == _userService.CurrentUser.Id)
			{
				_context.Ideas.Remove(idea);

				await _context.SaveChangesAsync();
			}
		}

		public async Task<Idea> Get(Guid id)
		{
			var idea = await _context.Ideas.FindAsync(id);

			if (idea != null && idea.OwnerId == _userService.CurrentUser.Id)
				return _mapper.Map<Idea>(idea);

			return null;
		}

		public IQueryable<Idea> List()
		{
			return _context.Ideas
				.Where(i => i.Owner.Id == _userService.CurrentUser.Id)
				.ProjectTo<Idea>(_mapper.ConfigurationProvider);
		}

		public async Task<Idea> Save(Idea idea)
		{
			var newIdea = _mapper.Map<IdeaEntity>(idea);

			newIdea.CreatedAt = System.DateTime.Now;
			newIdea.OwnerId = _userService.CurrentUser.Id;

			_context.Ideas.Add(newIdea);

			await _context.SaveChangesAsync();

			return _mapper.Map<Idea>(newIdea);
		}

		public async Task<Idea> Update(Idea idea)
		{
			var updated = await _context.Ideas.FindAsync(idea.Id);

			if (updated == null || updated.OwnerId != _userService.CurrentUser.Id)
				throw new InvalidOperationException("Idea not found");

			updated.Impact = idea.Impact;
			updated.Ease = idea.Ease;
			updated.Confidence = idea.Confidence;
			updated.Content = idea.Content;

			await _context.SaveChangesAsync();

			return _mapper.Map<Idea>(updated);
		}
	}
}
