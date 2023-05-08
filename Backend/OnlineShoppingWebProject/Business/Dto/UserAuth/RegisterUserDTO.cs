using Microsoft.AspNetCore.Http;

namespace Business.Dto
{
	public class RegisterUserDto : IDto
	{
		public string Firstname { get; set; }

		public string Lastname { get; set; }

		public string Username { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public string Address { get; set; }

		public string Type { get; set; }

		public IFormFile ProfileImage { get; set; }
	}
}
