using AutoMapper;
using Business.Dto;
using Data.Models;

namespace Business.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<User, RegisterUserDTO>().ReverseMap();

			CreateMap<Admin, RegisterUserDTO>().ReverseMap();

			CreateMap<Customer, RegisterUserDTO>().ReverseMap();
			 
			CreateMap<Seller, RegisterUserDTO>().ReverseMap();
		}
	}
}
