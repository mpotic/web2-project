using Data.Models;
using Google.Apis.Auth;

namespace Business.Util
{
	internal interface IGoogleHelper
	{
		Customer GetCustomerFromGoogleToken(GoogleJsonWebSignature.Payload payload);
	}
}
