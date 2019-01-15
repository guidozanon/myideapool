using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyIdeasPool.Core.Services;
using MyIdeasPool.WebApi.Models;

namespace MyIdeasPool.WebApi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
	public class MeController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IMapper _mapper;


		public MeController(IUserService userService, IMapper mapper)
		{
			_mapper = mapper;
			_userService = userService;
		}

		[HttpGet]
		public ActionResult<UserModel> Get()
		{
			var user = _mapper.Map<UserModel>(_userService.CurrentUser);

			return user;
		}
	}
}