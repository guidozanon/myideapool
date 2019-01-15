using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

		public async Task<bool> IsValidToken(string token, TokenType type)
		{
			if (CurrentUser == null || string.IsNullOrEmpty(token))
				return false;

			return await _context.UserJwtTokens
				.Where(t =>
					t.UserId == CurrentUser.Id &&
					t.Token == token &&
					t.Type == type)
				.CountAsync() == 1;
		}


		public async Task AddToken(string token, TokenType type)
		{
			if (string.IsNullOrEmpty(token))
				throw new ArgumentException("token cannot be null or empty");

			if (CurrentUser == null)
				throw new InvalidOperationException("Cannot save a token without logged user");

			_context.UserJwtTokens.Add(new UserTokenEntity
			{
				Token = token,
				Type = type,
				UserId = CurrentUser.Id
			});

			await _context.SaveChangesAsync();
		}



		public async Task RevokeToken(string token, TokenType type)
		{
			if (string.IsNullOrEmpty(token))
				throw new ArgumentException("token cannot be null or empty");

			if (CurrentUser == null)
				throw new InvalidOperationException("Cannot revoke a token without logged user");

			var remove = await _context.UserJwtTokens
									.Where(t =>
										t.UserId == CurrentUser.Id &&
										t.Token == token &&
										t.Type == type)
									.FirstOrDefaultAsync();

			if (remove != null)
			{
				_context.UserJwtTokens.Remove(remove);

				await _context.SaveChangesAsync();
			}

		}

		public async Task<User> GetUser(string refreshToken)
		{
			if (!string.IsNullOrEmpty(refreshToken))
			{
				var token = await _context.UserJwtTokens
					.Where(t => t.Token == refreshToken && t.Type == TokenType.RefreshToken)
					.FirstOrDefaultAsync();

				if (token != null)
				{
					return _mapper.Map<User>(token.User);
				}
			}

			return null;
		}
	}
}
