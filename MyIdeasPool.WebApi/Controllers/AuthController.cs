using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyIdeasPool.Core.Domain;
using MyIdeasPool.Core.Models;
using MyIdeasPool.Core.Services;
using MyIdeasPool.WebApi.Models;
using MyIdeasPool.WebApi.Security;

namespace MyIdeasPool.WebApi.Controllers
{
	[Route("access-tokens")]
	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class AuthController : ControllerBase
	{
		private readonly ITokenGenerator _tokenGenerator;
		private readonly SignInManager<UserEntity> _signinManager;
		private readonly UserManager<UserEntity> _userManager;
		private readonly IMapper _mapper;
		private readonly IUserService _userService;

		public AuthController(ITokenGenerator tokenGenerator, SignInManager<UserEntity> signinManager,
			UserManager<UserEntity> userManager, IMapper mapper, IUserService userService)
		{
			_tokenGenerator = tokenGenerator;
			_signinManager = signinManager;
			_userManager = userManager;
			_mapper = mapper;
			_userService = userService;
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login(LoginModel login)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(login.Email);

				if (user != null)
				{
					var result = await _signinManager.CheckPasswordSignInAsync(user, login.Password, false);

					if (result.Succeeded)
					{
						var token = _tokenGenerator.Generate(_mapper.Map<User>(user));

						_userService.SetCurrentUser(user.UserName);

						await _userService.AddToken(token.Jwt, TokenType.Token);
						await _userService.AddToken(token.RefreshToken, TokenType.RefreshToken);

						return Ok(token);
					}
				}
			}

			return BadRequest("Wrong username or password");
		}

		[HttpPost]
		[Route("refresh")]
		public async Task<IActionResult> Refresh(RefreshTokenModel model)
		{
			if (ModelState.IsValid && !string.IsNullOrEmpty(model.RefreshToken))
			{
				if (await _userService.IsValidToken(model.RefreshToken, TokenType.RefreshToken))
				{
					var user = await _userService.GetUser(model.RefreshToken);

					var newToken = _tokenGenerator.Generate(user);

					await _userService.RevokeToken(model.RefreshToken, TokenType.RefreshToken);
					await _userService.AddToken(newToken.Jwt, TokenType.Token);
					await _userService.AddToken(newToken.RefreshToken, TokenType.RefreshToken);

					return Ok(newToken);
				}
			}

			return BadRequest("Invalid refresh token");
		}


		[HttpDelete]
		public async Task<IActionResult> Logout()
		{
			try
			{
				var token = Request.Headers[CustomAuthMiddleware.DefaultHeader];

				await _userService.RevokeToken(token, TokenType.Token);

				return StatusCode(204);
			}
			catch (System.Exception ex)
			{
				return StatusCode(400, ex);
			}
		}
	}
}