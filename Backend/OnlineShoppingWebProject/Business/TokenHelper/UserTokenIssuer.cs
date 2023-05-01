using Data.Models;
using Microsoft.Extensions.Configuration;
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

		public UserTokenIssuer(IConfiguration configuration)
		{
			_key = configuration.GetSection("SecretKey").Value;
		}

		public string IssueJwt(IEnumerable<Claim> claims)
		{
			SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
			var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

			var tokenOptions = new JwtSecurityToken(
				issuer: "http://localhost:44301", 
				claims: claims, 
				expires: DateTime.Now.AddMinutes(360),
				signingCredentials: signinCredentials
			);

			string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

			return token;
		}

		public string IssueAdminJwt(IAdmin admin)
		{
			List<Claim> claims = new List<Claim>() 
			{ 
				new Claim(ClaimTypes.Role, UserRole.Admin.ToString()),
				new Claim(ClaimTypes.NameIdentifier, admin.Username),
				new Claim("role", UserRole.Admin.ToString())
			};

			string token = IssueJwt(claims);

			return token;
		}

		public string IssueCostumerJwt(ICustomer customer)
		{
			List<Claim> claims = new List<Claim>() 
			{ 
				new Claim(ClaimTypes.Role, UserRole.Customer.ToString()),
				new Claim(ClaimTypes.NameIdentifier, customer.Username)
			};

			string token = IssueJwt(claims);

			return token;
		}

		public string IssueSellerJwt(ISeller seller)
		{
			List<Claim> claims = new List<Claim>() 
			{ 
				new Claim(ClaimTypes.Role, UserRole.Seller.ToString()),
				new Claim(ClaimTypes.NameIdentifier, seller.Username)
			};

			string token = IssueJwt(claims);

			return token;
		}

		public string IssueUserJwt(IUser user)
		{
			if (user.GetType().Equals(typeof(Admin)))
			{
				return IssueAdminJwt((Admin)user);
			}
			else if (user.GetType().Equals(typeof(Customer)))
			{
				return IssueCostumerJwt((Customer)user);
			}
			else if (user.GetType().Equals(typeof(Seller)))
			{
				return IssueSellerJwt((Seller)user);
			}

			return null;
		}
	}
}
