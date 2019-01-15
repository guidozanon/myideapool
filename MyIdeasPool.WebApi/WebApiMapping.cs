using AutoMapper;
using MyIdeasPool.Core.Domain;
using MyIdeasPool.Core.Models;
using MyIdeasPool.WebApi.Models;

namespace MyIdeasPool.WebApi
{
	public class WebApiMappingProfile : Profile
	{
		public WebApiMappingProfile()
		{
			CreateMap<Idea, IdeaModel>();
			CreateMap<IdeaModel, Idea>();

			CreateMap<User, UserModel>();

			CreateMap<SignupModel, UserEntity>()
				.ForMember(x=>x.UserName, mapper=>mapper.MapFrom(x=>x.Email));
		}
	}
}
