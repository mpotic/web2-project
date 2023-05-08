using AutoMapper;
using Business.Dto;
using Business.Dto.User;
using Business.Result;
using Business.Services.Interfaces;
using Business.TokenHelper;
using Business.Util;
using Data.Models;
using Data.UnitOfWork;

namespace Business.Services
{
	public class UserService : IUserService
	{
		readonly IUserTokenIssuer _tokenIssuer;

		readonly IMapper _mapper;

		readonly IUserHelper _userHelper;

		readonly IUnitOfWork _unitOfWork;

		public UserService(IUserTokenIssuer tokenIssuer, IMapper mapper, IUserHelper helper, IUnitOfWork unitOfWork)
		{
			_tokenIssuer = tokenIssuer;
			_mapper = mapper;
			_userHelper = helper;
			_unitOfWork = unitOfWork;
		}

		public IServiceOperationResult GetUserFromToken(JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			string username = _tokenIssuer.GetUsernameFromToken(jwtDto.Token);
			IUser user = _userHelper.FindUserByUsername(username);
			if(user == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);
				
				return operationResult;
			}

			UserDto userDto = _mapper.Map<UserDto>(user);
			operationResult = new ServiceOperationResult(true, userDto);

			return operationResult;
		}

		public IServiceOperationResult UpdateUser(UserDto newUserDto, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			IUser newUser = _mapper.Map<User>(newUserDto);
			if (!_userHelper.ValidateUser(newUser))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest, "Invalid user data!");
				
				return operationResult;
			}

			string username = _tokenIssuer.GetUsernameFromToken(jwtDto.Token);
			IUser currentUser = _userHelper.FindUserByUsername(username);
			if (currentUser == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);
				
				return operationResult;
			}

			if(newUser.Email != currentUser.Email && _userHelper.FindUserByEmail(newUser.Email) != null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.Conflict, "A user with a given email already exists!");
				
				return operationResult;
			}

			if (newUser.Username != currentUser.Username && _userHelper.FindUserByUsername(newUser.Username) != null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.Conflict, "A user with a given username already exists!");
				
				return operationResult;
			}

			if(currentUser is Admin)
			{
				_unitOfWork.AdminRepository.Update((Admin)currentUser);
			}
			else if(currentUser is Customer)
			{
				_unitOfWork.CustomerRepository.Update((Customer)currentUser);
			}
			else if(currentUser is Seller)
			{
				_unitOfWork.SellerRepository.Update((Seller)currentUser);
			}
			else
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);
				
				return operationResult;
			}

			_unitOfWork.Commit();

			operationResult = new ServiceOperationResult(true);
			
			return operationResult;
		}
	}
}
