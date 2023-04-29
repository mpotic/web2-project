using Data.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.TokenHelper
{
	public class UserTokenIssuer : IUserTokenIssuer
	{
		string _key;

		public UserTokenIssuer(string key)
		{
			_key = key;
		}

		public string IssueUserJWT(IEnumerable<Claim> claims)
		{
			SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
			var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

			var tokenOptions = new JwtSecurityToken(
				issuer: "http://localhost:4000", 
				claims: claims, 
				expires: DateTime.Now.AddMinutes(360),
				signingCredentials: signinCredentials
			);

			string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

			return token;
		}

		public string IssueAdminJWT(IAdmin admin)
		{
			List<Claim> claims = new List<Claim>() 
			{ 
				new Claim(ClaimTypes.Role, UserClaims.Admin.ToString()),
				new Claim(ClaimTypes.NameIdentifier, admin.Username)
			};

			string token = IssueUserJWT(claims);

			return token;
		}

		public string IssueCostumerJWT(ICustomer customer)
		{
			List<Claim> claims = new List<Claim>() 
			{ 
				new Claim(ClaimTypes.Role, UserClaims.Customer.ToString()),
				new Claim(ClaimTypes.NameIdentifier, customer.Username)
			};

			string token = IssueUserJWT(claims);

			return token;
		}

		public string IssueSellerJWT(ISeller seller)
		{
			List<Claim> claims = new List<Claim>() 
			{ 
				new Claim(ClaimTypes.Role, UserClaims.Seller.ToString()),
				new Claim(ClaimTypes.NameIdentifier, seller.Username)
			};

			string token = IssueUserJWT(claims);

			return token;
		}
	}
}
