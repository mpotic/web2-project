using Business.Dto.Auth;
using Business.Result;
using Google.Apis.Auth;

namespace Business.Services
{
	public interface IUserAuthService
	{
		IServiceOperationResult LoginUser(LoginUserDto loginDto);

		IServiceOperationResult RegisterUser(RegisterUserDto registerDto);

		IServiceOperationResult GoogleLogin(GoogleJsonWebSignature.Payload payload);
	}
}
