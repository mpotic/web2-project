using AutoMapper;
using Business.Dto;
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
			CreateMap<User, UserDto>().ReverseMap();

			CreateMap<Admin, UserDto>().ReverseMap();

			CreateMap<Customer, UserDto>().ReverseMap();

			CreateMap<Seller, UserDto>().ReverseMap();
		}
	}
}
