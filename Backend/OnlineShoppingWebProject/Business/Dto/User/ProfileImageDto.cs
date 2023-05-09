using Microsoft.AspNetCore.Http;

namespace Business.Dto.User
{
	public class ProfileImageDto
	{
		public IFormFile ProfileImage { get; set; }
	}
}
