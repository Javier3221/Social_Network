using AutoMapper;
using Social_Network.Core.Application.ViewModels.Commentary;
using Social_Network.Core.Application.ViewModels.Friend;
using Social_Network.Core.Application.ViewModels.Post;
using Social_Network.Core.Application.ViewModels.User;
using Social_Network.Core.Domain.Entities;

namespace Social_Network.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.FriendUserNames, opt => opt.Ignore());

            CreateMap<UserViewModel, SaveUserViewModel>()
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore())
                .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Posts, opt => opt.Ignore())
                .ForMember(dest => dest.FriendWith, opt => opt.Ignore())
                .ForMember(dest => dest.Friends, opt => opt.Ignore())
                .ForMember(dest => dest.FriendUserNames, opt => opt.Ignore());


            CreateMap<SaveUserViewModel, User>()
                .ForMember(dest => dest.Posts, opt => opt.Ignore())
                .ForMember(dest => dest.Friends, opt => opt.Ignore())
                .ForMember(dest => dest.FriendWith, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());

            CreateMap<PostViewModel, Post>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Post, SavePostViewModel>()
                .ForMember(dest => dest.PostImage, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Commentaries, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<FriendViewModel, Friend>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<CommentaryViewModel, Commentary>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
