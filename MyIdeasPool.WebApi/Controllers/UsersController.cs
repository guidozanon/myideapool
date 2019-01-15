using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyIdeasPool.Core.Domain;
using MyIdeasPool.Core.Models;
using MyIdeasPool.Core.Services;
using MyIdeasPool.WebApi.Configuration;
using MyIdeasPool.WebApi.Helpers;
using MyIdeasPool.WebApi.Models;
using MyIdeasPool.WebApi.Security;

namespace MyIdeasPool.WebApi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IOptions<GlobalConfiguration> _config;
		private readonly IMapper _mapper;
		private readonly UserManager<UserEntity> _userManager;
		private readonly JwtTokenGenerator _jwtTokenGenerator;

		public UsersController(IUserService userService, IOptions<GlobalConfiguration> config, IMapper mapper
			, UserManager<UserEntity> userManager, JwtTokenGenerator jwtTokenGenerator)
		{
			_config = config;
			_mapper = mapper;
			_userService = userService;
			_userManager = userManager;
			_jwtTokenGenerator = jwtTokenGenerator;
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult<TokenModel>> Signup(SignupModel signup)
		{
			if (ModelState.IsValid)
			{
				var newUser = _mapper.Map<UserEntity>(signup);

				newUser.AvatarUrl = signup.Email.GenerateImageUrl();

				var result = await _userManager.CreateAsync(newUser, signup.Password);
				if (result.Succeeded)
				{
					var token = _jwtTokenGenerator.Generate(_mapper.Map<User>(newUser));

					_userService.SetCurrentUser(newUser.UserName);

					await _userService.AddToken(token.Jwt, TokenType.Token);
					await _userService.AddToken(token.RefreshToken, TokenType.RefreshToken);

					return Ok(token);
				}
				else
				{
					return StatusCode(400, result.ToString());
				}
			}

			return base.StatusCode(400, ModelState);
		}

		
	}
}