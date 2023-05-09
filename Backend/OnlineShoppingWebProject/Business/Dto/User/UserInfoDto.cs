using System;

namespace Business.Dto.User
{
	public class UserInfoDto : IDto
	{
		public string Firstname { get; set; }

		public string Lastname { get; set; }

		public string Username { get; set; }

		public string Address { get; set; }

		public DateTime Birthdate { get; set; }

		public string Email { get; set; }
	}
}
