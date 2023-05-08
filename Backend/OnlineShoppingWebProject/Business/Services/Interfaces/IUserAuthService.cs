using Business.Dto;
using Business.Result;

namespace Business.Services
{
	public interface IUserAuthService
	{
		IServiceOperationResult LoginUser(LoginUserDto loginDto);

		IServiceOperationResult RegisterUser(RegisterUserDto registerDto);
	}
}
