using Business.Dto;
using Business.Dto.User;
using Business.Result;

namespace Business.Services.Interfaces
{
	public interface IUserService
	{
		IServiceOperationResult GetUser(JwtDto jwtDto);

		IServiceOperationResult UpdateUser(BasicUserInfoDto newUserDto, JwtDto jwtDto);

		IServiceOperationResult ChangePassword(PasswordChangeDto passwordDto, JwtDto jwtDto);

		IServiceOperationResult UploadProfileImage(ProfileImageDto profileDto, JwtDto jwtDto);
	}
}
