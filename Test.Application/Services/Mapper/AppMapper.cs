using AutoMapper;
using TestApi.Domain.Entities.Posts;
using TestApi.Domain.Entities.Users;
using TestApi.Domain.Models.Posts;
using TestApi.Domain.Models.Users;

namespace TestApi.Application.Services.Mapper
{
    public class AppMapper : Profile
    {
        public AppMapper()
        {
            PostMapping();
            UserMapping();
        }

        public void PostMapping()
        {
            CreateMap<AddPostModel, Post>();
            CreateMap<UpdatePostModel, Post>();

            CreateMap<Post, PostModel>()
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.UserId));
        }

        public void UserMapping()
        {
            CreateMap<AddUserModel, User>();
            CreateMap<UpdateUserModel, User>();

            CreateMap<User, UserModel>();
        }
    }
}
