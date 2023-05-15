namespace Business.Dto.User
{
	public class PasswordChangeDto : IDto
	{
		public string OldPassword { get; set; }

		public string NewPassword { get; set; }
	}
}
