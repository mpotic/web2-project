using Microsoft.AspNetCore.Http;

namespace Business.Dto.User
{
	public class ProfileImageDto : IDto
	{
		public IFormFile ProfileImage { get; set; }
	}
}
