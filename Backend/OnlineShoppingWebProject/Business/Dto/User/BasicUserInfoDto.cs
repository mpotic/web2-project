using System;

namespace Business.Dto.User
{
	public class BasicUserInfoDto : IDto
	{
		public string Firstname { get; set; }

		public string Lastname { get; set; }

		public string Username { get; set; }

		public string Address { get; set; }

		public DateTime? Birthdate { get; set; }
	}
}
