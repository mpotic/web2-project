using AutoMapper;
using Business.Dto;
using Business.Dto.User;
using Business.Result;
using Business.Services.Interfaces;
using Business.TokenHelper;
using Business.Util;
using Data.Models;
using Data.Repository.Util;
using Data.UnitOfWork;
using System.IO;

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

			long id = int.Parse(_tokenIssuer.GetClaimValueFromToken(jwtDto.Token, "id"));
			IUser user = userHelper.FindById(id);
			if(user == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);
				
				return operationResult;
			}

			UserInfoDto userDto = _mapper.Map<UserInfoDto>(user);
			operationResult = new ServiceOperationResult(true, userDto);

			return operationResult;
		}

		public IServiceOperationResult UpdateUser(BasicUserInfoDto newUserDto, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			IUser newUser = _mapper.Map<User>(newUserDto);
			long id = int.Parse(_tokenIssuer.GetClaimValueFromToken(jwtDto.Token, "id"));
			IUser currentUser = userHelper.FindById(id);

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

			UpdateProfileImagePath(currentUser, newUser.Username);
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
			long id = int.Parse(_tokenIssuer.GetClaimValueFromToken(jwtDto.Token, "id"));
			IUser user = userHelper.FindById(id);

			IAuthHelper authHelper = new AuthHelper();
			if(!authHelper.IsPasswordValid(passwordDto.OldPassword, user.Password))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.Unauthorized);

				return operationResult;
			}

			if (authHelper.IsPasswordWeak(passwordDto.NewPassword))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest);

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
			long id = int.Parse(_tokenIssuer.GetClaimValueFromToken(jwtDto.Token, "id"));
			IUser user = userHelper.FindById(id);

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

		/// <summary>
		/// In case that the username has changed, the profile image file name has to be updated which is what this method does.
		/// </summary>
		private void UpdateProfileImagePath(IUser currentUser, string newUsername)
		{
			if (currentUser.Username == newUsername)
			{
				return;
			}

			string oldProfileImagePath = Path.Combine(Directory.GetCurrentDirectory(), userHelper.ProfileImagesRelativePath, currentUser.ProfileImage);

			if (!File.Exists(oldProfileImagePath))
			{
				return;
			}

			string fileExtension = Path.GetExtension(currentUser.ProfileImage);
			string profileImage = newUsername + fileExtension;

			string newProfileImagePath = Path.Combine(Directory.GetCurrentDirectory(), userHelper.ProfileImagesRelativePath, profileImage);
			File.Move(oldProfileImagePath, newProfileImagePath);

			currentUser.ProfileImage = profileImage;
		}
	}
}
