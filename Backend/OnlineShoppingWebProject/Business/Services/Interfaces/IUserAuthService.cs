using Business.Dto;
using Business.Result;

namespace Business.Services
{
	public interface IUserAuthService
	{
		IServiceOperationResult LoginUser(LoginUserDTO loginDto);

		IServiceOperationResult RegisterUser(RegisterUserDTO registerDto);
	}
}
