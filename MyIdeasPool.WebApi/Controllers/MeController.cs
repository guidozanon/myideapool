using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyIdeasPool.Core.Services;
using MyIdeasPool.WebApi.Configuration;
using MyIdeasPool.WebApi.Helpers;
using MyIdeasPool.WebApi.Models;

namespace MyIdeasPool.WebApi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize]
	public class MeController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly GlobalConfiguration _config;
		private readonly IMapper _mapper;


		public MeController(IUserService userService, GlobalConfiguration config, IMapper mapper)
		{
			_config = config;
			_mapper = mapper;
			_userService = userService;
		}

		[HttpGet]
		public ActionResult<UserModel> Get()
		{
			var user = _mapper.Map<UserModel>(_userService.CurrentUser);

			user.AvatarUrl = user.Email.GenerateImageUrl();

			return user;
		}
	}
}