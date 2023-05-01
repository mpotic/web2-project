using System.Net.Mail;

namespace Business.Util
{
	public class AuthHelper : IAuthHelper
	{
		public bool IsPasswordValid(string inputPassword, string databasePassword)
		{
			return BCrypt.Net.BCrypt.Verify(inputPassword, databasePassword);
		}

		public string HashPassword(string originalPassword)
		{
			return BCrypt.Net.BCrypt.HashPassword(originalPassword);
		}

		public bool IsPasswordWeak(string password)
		{
			password = password.Trim();

			if(password.Length < 6)
			{
				return true;
			}

			return false;
		}

		public bool IsEmailValid(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				return false;
			}

			try
			{
				var mailAddress = new MailAddress(email);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
