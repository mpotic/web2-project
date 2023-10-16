using AutoMapper;
using Business.Dto.Auth;
using Business.Dto.User;
using Business.Result;
using Business.TokenHelper;
using Business.Util;
using Data.Models;
using Data.Repository.Util;
using Data.UnitOfWork;

namespace Business.Services
{
	public class UserService : IUserService
	{
		readonly IUserTokenIssuer _tokenIssuer;

		readonly IMapper _mapper;

		readonly IUnitOfWork _unitOfWork;

		readonly IUserHelper userHelper;

		readonly IUserRepositoryManager userRepositoryManager;

		public UserService(IUserTokenIssuer tokenIssuer, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_tokenIssuer = tokenIssuer;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			userHelper = new UserHelper(unitOfWork);
			userRepositoryManager = new UserRepositoryManager(unitOfWork);
		}

		public IServiceOperationResult GetUser(JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			IUser user = userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (user == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);

				return operationResult;
			}
			
			UserInfoDto userDto = _mapper.Map<UserInfoDto>(user);
			operationResult = new ServiceOperationResult(true, userDto);

			return operationResult;
		}

		public IServiceOperationResult GetProfileImage(JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			IUser user = userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (user == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);

				return operationResult;
			}

			if (user.ProfileImage == null)
			{
				operationResult = new ServiceOperationResult(true);
				return operationResult;
			}

			byte[] image = userHelper.GetProfileImage(user.ProfileImage);
			if (image == null)
			{
				//operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Users profile image has not been found!");
				operationResult = new ServiceOperationResult(true);
				return operationResult;
			}

			ProfileImageByteArrDto imageDto = new ProfileImageByteArrDto() { ProfileImage = image };
			operationResult = new ServiceOperationResult(true, imageDto);

			return operationResult;
		}

		public IServiceOperationResult UpdateUser(BasicUserInfoDto newUserDto, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			IUser newUser = _mapper.Map<User>(newUserDto);

			IUser currentUser = userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (currentUser == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);

				return operationResult;
			}

			if (currentUser == null || newUser == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);
				
				return operationResult;
			}

			if (newUser.Username != currentUser.Username && userHelper.FindUserByUsername(newUser.Username) != null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.Conflict, "A user with a given username already exists!");
				
				return operationResult;
			}

			userHelper.UpdateProfileImagePath(currentUser, newUser.Username);
			userHelper.UpdateBasicUserData(currentUser, newUser);

			if(!userRepositoryManager.UpdateUser(currentUser))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);
				
				return operationResult;
			}

			_unitOfWork.Commit();

			operationResult = new ServiceOperationResult(true);
			
			return operationResult;
		}

		public IServiceOperationResult ChangePassword(PasswordChangeDto passwordDto, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			IUser user = userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (user == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);

				return operationResult;
			}

			IAuthHelper authHelper = new AuthHelper();
			if(!authHelper.IsPasswordValid(passwordDto.OldPassword, user.Password))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.Unauthorized, "Invalid current password!");

				return operationResult;
			}

			if (authHelper.IsPasswordWeak(passwordDto.NewPassword))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest, "Password should be at least 6 characters long!");

				return operationResult;
			}

			string newHashedPass = authHelper.HashPassword(passwordDto.NewPassword);
			user.Password = newHashedPass;

			if (!userRepositoryManager.UpdateUser(user))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest);

				return operationResult;
			}

			_unitOfWork.Commit();

			operationResult = new ServiceOperationResult(true);
			
			return operationResult;
		}

		public IServiceOperationResult UploadProfileImage(ProfileImageDto profileDto, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			IUser user = userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (user == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);

				return operationResult;
			}

			if (!userHelper.UploadProfileImage(user, profileDto.ProfileImage))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest);

				return operationResult;
			}

			if (!userRepositoryManager.UpdateUser(user))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest);

				return operationResult;
			}

			_unitOfWork.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}
	}
}
