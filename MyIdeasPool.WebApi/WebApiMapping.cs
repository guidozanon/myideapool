using AutoMapper;
using MyIdeasPool.Core.Models;
using MyIdeasPool.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdeasPool.WebApi
{
	public class WebApiMappingProfile : Profile
	{
		public WebApiMappingProfile()
		{
			CreateMap<Idea, IdeaModel>();
			CreateMap<IdeaModel, Idea>();

			CreateMap<User, UserModel>();
		}
	}
}
