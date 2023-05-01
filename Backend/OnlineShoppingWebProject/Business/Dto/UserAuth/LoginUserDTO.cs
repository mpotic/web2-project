namespace Business.Dto
{
	public class LoginUserDTO : IDto
	{
		public string Username { get; set; }

		public string Password { get; set; }
	}
}
