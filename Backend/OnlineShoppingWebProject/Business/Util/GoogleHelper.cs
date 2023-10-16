using Data.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace Business.Util
{
	internal class GoogleHelper : IGoogleHelper
	{
		private readonly IUserHelper userHelper;

		internal GoogleHelper(IUserHelper userHelper)
		{
			this.userHelper = userHelper;
		} 

		public Customer GetCustomerFromGoogleToken(GoogleJsonWebSignature.Payload payload)
		{
			var user = new Customer
			{
				Firstname = payload.GivenName,
				Lastname = payload.FamilyName,
				Username = payload.Email.Split('@')[0],
				Email = payload.Email,
			};

			//userHelper.UploadProfileImage(user, DownloadProfileImage(payload.Picture));

			return user;
		}

		private IFormFile DownloadProfileImage(string imageUrl)
		{
			using (var httpClient = new HttpClient())
			{
				var response = httpClient.GetAsync(imageUrl).Result;

				if (response.IsSuccessStatusCode)
				{
					var stream = response.Content.ReadAsStreamAsync().Result;
					var formFile = new FormFile(stream, 0, stream.Length, "profileImage", "profile.jpg");

					return formFile;
				}
			}

			return null;
		}

	}
}
