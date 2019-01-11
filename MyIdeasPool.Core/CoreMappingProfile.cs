using AutoMapper;
using MyIdeasPool.Core.Domain;
using MyIdeasPool.Core.Models;

namespace MyIdeasPool.Core
{
	public class CoreMappingProfile : Profile
	{
		public CoreMappingProfile()
		{
			CreateMap<IdeaEntity, Idea>();
			CreateMap<Idea, IdeaEntity>();
		}
	}
}
