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
	public class IdeasController : ControllerBase
	{
		private readonly IIdeasService _ideaService;
		private readonly GlobalConfiguration _config;
		private readonly IMapper _mapper;


		public IdeasController(IIdeasService ideaService, GlobalConfiguration config, IMapper mapper)
		{
			_ideaService = ideaService;
			_config = config;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<IdeaModel>>> Get(int page = 0)
		{
			var ideas = await _ideaService.List()
					.Skip(page * _config.IdeasPageSize)
					.Take(_config.IdeasPageSize)
					.ProjectTo<IdeaModel>(_mapper.ConfigurationProvider)
					.ToListAsync();

			return Ok(ideas);
		}

		[HttpPost]
		public async Task<ActionResult> Create(IdeaModel idea)
		{
			//TODO complete
			return Ok();
		}

		[HttpPut]
		public async Task<ActionResult> Update(IdeaModel idea)
		{
			//TODO complete
			return Ok();
		}

		[HttpDelete]
		public async Task<ActionResult> Delete(int id)
		{
			//TODO complete
			return Ok();
		}
	}
}