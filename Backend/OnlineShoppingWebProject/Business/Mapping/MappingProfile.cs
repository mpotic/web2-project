using AutoMapper;
using Business.Dto.ArticleDto;
using Business.Dto.Auth;
using Business.Dto.User;
using Data.Models;

namespace Business.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			MapAuth();

			MapUser();

			MapArticle();
		}

		public void MapAuth()
		{
			CreateMap<User, RegisterUserDto>().ReverseMap();

			CreateMap<Admin, RegisterUserDto>().ReverseMap();

			CreateMap<Customer, RegisterUserDto>().ReverseMap();

			CreateMap<Seller, RegisterUserDto>().ReverseMap();
		}

		public void MapUser()
		{
			CreateMap<User, BasicUserInfoDto>().ReverseMap();

			CreateMap<Admin, BasicUserInfoDto>().ReverseMap();

			CreateMap<Customer, BasicUserInfoDto>().ReverseMap();

			CreateMap<Seller, BasicUserInfoDto>().ReverseMap();

			CreateMap<User, UserInfoDto>().ReverseMap();

			CreateMap<Admin, UserInfoDto>().ReverseMap();

			CreateMap<Customer, UserInfoDto>().ReverseMap();

			CreateMap<Seller, UserInfoDto>().ReverseMap();
		}

		public void MapArticle()
		{
			CreateMap<Article, ArticleInfoDto>().ReverseMap();

			CreateMap<Article, ArticleUpdateDto>().ReverseMap();

			CreateMap<Article, NewArticleDto>().ReverseMap();
		}
	}
}
