using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyIdeasPool.Core.Services;
using MyIdeasPool.WebApi.Configuration;
using MyIdeasPool.WebApi.Models;

namespace MyIdeasPool.WebApi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly GlobalConfiguration _config;
		private readonly IMapper _mapper;


		public UsersController(IUserService userService, GlobalConfiguration config, IMapper mapper)
		{
			_config = config;
			_mapper = mapper;
			_userService = userService;
		}

		[HttpPost]
		public async Task<ActionResult<TokenModel>> Signup(SignupModel signup)
		{
			//TODO complete
			if (ModelState.IsValid)
			{

			}

			return Ok();
		}
	}
}