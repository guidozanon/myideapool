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

			CreateMap<UserEntity, User>()
				.ForMember(x => x.Name, map => map.MapFrom(x => x.UserName));

			CreateMap<User, UserEntity>()
				.ForMember(x => x.UserName, map => map.MapFrom(x => x.Name));
		}
	}
}
