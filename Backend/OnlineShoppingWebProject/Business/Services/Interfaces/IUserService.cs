using Business.Dto.Auth;
using Business.Dto.User;
using Business.Result;

namespace Business.Services
{
	public interface IUserService
	{
		IServiceOperationResult GetUser(JwtDto jwtDto);

		IServiceOperationResult GetProfileImage(JwtDto jwtDto);

		IServiceOperationResult UpdateUser(BasicUserInfoDto newUserDto, JwtDto jwtDto);

		IServiceOperationResult ChangePassword(PasswordChangeDto passwordDto, JwtDto jwtDto);

		IServiceOperationResult UploadProfileImage(ProfileImageDto profileDto, JwtDto jwtDto);
	}
}
