namespace Business.Dto.Auth
{
	public class JwtDto : IDto
	{
		public JwtDto(string token)
		{
			Token = token;
		}
		
		public string Token { get; set; }
	}
}
