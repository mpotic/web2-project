﻿namespace Business.Dto.Auth
{
	public class LoginUserDto : IDto
	{
		public string Username { get; set; }

		public string Password { get; set; }
	}
}
