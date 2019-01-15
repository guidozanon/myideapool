using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyIdeasPool.Core.Models;
using MyIdeasPool.Core.Services;
using MyIdeasPool.WebApi.Configuration;
using MyIdeasPool.WebApi.Models;

namespace MyIdeasPool.WebApi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class IdeasController : ControllerBase
	{
		private readonly IIdeasService _ideaService;
		private readonly IOptions<GlobalConfiguration> _config;
		private readonly IMapper _mapper;


		public IdeasController(IIdeasService ideaService, IOptions<GlobalConfiguration> config, IMapper mapper)
		{
			_ideaService = ideaService;
			_config = config;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<IdeaModel>>> Get([FromQuery]int page =1)
		{
			var ideas = await _ideaService.List()
					.Skip((page - 1) * _config.Value.IdeasPageSize)
					.Take(_config.Value.IdeasPageSize)
					.ProjectTo<IdeaModel>(_mapper.ConfigurationProvider)
					.ToListAsync();

			return Ok(ideas);
		}

		[HttpPost]
		public async Task<IActionResult> Create(IdeaModel idea)
		{
			if (ModelState.IsValid)
			{
				var newIdea = _mapper.Map<Idea>(idea);

				var savedIdea = await _ideaService.Save(newIdea);

				return Ok(_mapper.Map<IdeaModel>(savedIdea));
			}

			return BadRequest(ModelState);
		}

		[HttpPut]
		[Route("{id}")]
		public async Task<ActionResult> Update([FromRoute]Guid id, IdeaModel idea)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var updated = _mapper.Map<Idea>(idea);

					updated.Id = id;

					var updatedIdea = await _ideaService.Update(updated);

					return Ok(_mapper.Map<IdeaModel>(updatedIdea));
				}
				catch (InvalidOperationException)
				{
					return NotFound();
				}
				catch (Exception ex)
				{
					return BadRequest(ex.Message);
				}
			}
			return BadRequest(ModelState);
		}

		[HttpDelete]
		[Route("{id}")]
		public async Task<IActionResult> Delete([FromRoute]Guid id)
		{
			if (ModelState.IsValid)
			{
				await _ideaService.Delete(id);

				return StatusCode(204);
			}
			return BadRequest(ModelState);
		}
	}
}