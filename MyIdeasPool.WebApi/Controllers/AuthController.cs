using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyIdeasPool.Core.Domain;
using MyIdeasPool.Core.Models;
using MyIdeasPool.WebApi.Models;
using MyIdeasPool.WebApi.Security;

namespace MyIdeasPool.WebApi.Controllers
{
	[Route("access-token")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly JwtTokenGenerator _tokenGenerator;
		private readonly SignInManager<UserEntity> _signinManager;
		private readonly UserManager<UserEntity> _userManager;
		private readonly IMapper _mapper;
		public AuthController(JwtTokenGenerator tokenGenerator, SignInManager<UserEntity> signinManager,
			UserManager<UserEntity> userManager, IMapper mapper)
		{
			_tokenGenerator = tokenGenerator;
			_signinManager = signinManager;
			_userManager = userManager;
			_mapper = mapper;
		}

		[HttpPost]
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

						return Ok(token);
					}
				}
			}

			return StatusCode(401);
		}

	}
}