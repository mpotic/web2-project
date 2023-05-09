﻿using Data.Models;

namespace Business.TokenHelper
{
	public interface IUserTokenIssuer
	{
		string IssueUserJwt(IUser user);

		string GetUsernameFromToken(string tokenString);

		string GetClaimValueFromToken(string tokenString, string claimType);
	}
}
