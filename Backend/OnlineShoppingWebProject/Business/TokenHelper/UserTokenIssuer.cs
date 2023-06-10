using Business.Dto.User;
using Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Business.TokenHelper
{
	public class UserTokenIssuer : IUserTokenIssuer
	{
		string _key;

		readonly int sslPort = 44301;

		public UserTokenIssuer(IConfiguration configuration)
		{
			_key = configuration.GetSection("SecretKey").Value;
		}

		public string IssueJwt(IEnumerable<Claim> claims)
		{
			SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
			var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

			var tokenOptions = new JwtSecurityToken(
				issuer: $"http://localhost:{sslPort}", 
				claims: claims, 
				expires: DateTime.Now.AddMinutes(360),
				signingCredentials: signinCredentials
			);

			JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
			string token = handler.WriteToken(tokenOptions);

			return token;
		}

		private string IssueAdminJwt(IAdmin admin)
		{
			List<Claim> claims = new List<Claim>() 
			{ 
				new Claim(ClaimTypes.Role, UserType.Admin.ToString()),
				new Claim("role", UserType.Admin.ToString()),
				new Claim("id", admin.Id.ToString()),
				new Claim("username", admin.Username)
			};

			string token = IssueJwt(claims);

			return token;
		}

		private string IssueCostumerJwt(ICustomer customer)
		{
			List<Claim> claims = new List<Claim>() 
			{ 
				new Claim(ClaimTypes.Role, UserType.Customer.ToString()),
				new Claim("role", UserType.Customer.ToString()),
				new Claim("id", customer.Id.ToString()),
				new Claim("username", customer.Username)
			};

			string token = IssueJwt(claims);

			return token;
		}

		private string IssueSellerJwt(ISeller seller)
		{
			List<Claim> claims = new List<Claim>() 
			{ 
				new Claim(ClaimTypes.Role, UserType.Seller.ToString()),
				new Claim("role", UserType.Seller.ToString()),
				new Claim("id", seller.Id.ToString()),
				new Claim("username", seller.Username),
				new Claim("status", seller.ApprovalStatus.ToString())
			};

			string token = IssueJwt(claims);

			return token;
		}

		public string IssueUserJwt(IUser user)
		{
			string token = null;

			if (user.GetType().Equals(typeof(Admin)))
			{
				token = IssueAdminJwt((Admin)user);
			}
			else if (user.GetType().Equals(typeof(Customer)))
			{
				token =  IssueCostumerJwt((Customer)user);
			}
			else if (user.GetType().Equals(typeof(Seller)))
			{
				token =  IssueSellerJwt((Seller)user);
			}

			return token;
		}

		public string GetUsernameFromToken(string tokenString)
		{
			JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
			JwtSecurityToken token = handler.ReadJwtToken(tokenString);
			string username = token.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

			return username;
		}

		public string GetClaimValueFromToken(string tokenString, string claimType)
		{
			JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
			JwtSecurityToken token = handler.ReadJwtToken(tokenString);
			string claimValue = token.Claims.Where(x => x.Type == claimType).FirstOrDefault().Value;

			return claimValue;
		}
	}
}
