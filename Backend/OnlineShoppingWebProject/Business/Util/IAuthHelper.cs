﻿namespace Business.Util
{
	public interface IAuthHelper
	{
		bool IsPasswordValid(string inputPassword, string databasePassword);
		
		string HashPassword(string originalPassword);

		bool IsPasswordWeak(string password);

		bool IsEmailValid(string email);
	}
}
