using Business.Dto.Auth;
using Business.Result;
using System.Threading.Tasks;

namespace Business.Services
{
	public interface IUserAuthService
	{
		IServiceOperationResult LoginUser(LoginUserDto loginDto);

		IServiceOperationResult RegisterUser(RegisterUserDto registerDto);

		Task<IServiceOperationResult> GoogleLogin(string idToken);
	}
}
