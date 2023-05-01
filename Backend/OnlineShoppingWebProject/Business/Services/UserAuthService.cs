using AutoMapper;
using Business.Dto;
using Business.Result;
using Business.TokenHelper;
using Business.Util;
using Data.Models;
using Data.UnitOfWork;

namespace Business.Services
{
	public class UserAuthService : IUserAuthService
	{
		private IUserTokenIssuer _userTokenIssuer;

		private IUnitOfWork _unitOfWork;

		private IMapper _mapper;

		private IAuthHelper authHelper = new AuthHelper();

		private IUserHelper userHelper;

		public UserAuthService(IUserTokenIssuer userTokenIssuer, IUnitOfWork unitOfWork, IMapper mapper)
		{
			_userTokenIssuer = userTokenIssuer;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			userHelper = new UserHelper(_unitOfWork);
		}

		public IServiceOperationResult LoginUser(LoginUserDTO loginDto)
		{
			IServiceOperationResult operationResult;

			IUser user = userHelper.FindUserByUsername(loginDto.Username);

			if(user == null)
			{
				operationResult = new ServiceOperationResult(false,ServiceOperationErrorCode.NotFound);
				return operationResult;
			}

			if (!authHelper.IsPasswordValid(loginDto.Password, user.Password))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.Unauthorized);
				return operationResult;
			}

			string token = _userTokenIssuer.IssueUserJwt(user);

			if (token == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.InternalServerError);
				return operationResult;
			}

			operationResult = new ServiceOperationResult(true, new JwtDto(token));

			return operationResult;
		}

		public IServiceOperationResult RegisterUser(RegisterUserDTO registerDto)
		{
			IServiceOperationResult operationResult;

			if (userHelper.FindUserByUsername(registerDto.Username) != null ||
				userHelper.FindUserByEmail(registerDto.Email) != null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.Conflict, "User already exists!");
				return operationResult;
			}

			registerDto.Password = authHelper.HashPassword(registerDto.Password);

			if(authHelper.IsPasswordWeak(registerDto.Password) || !authHelper.IsEmailValid(registerDto.Email))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest);
				return operationResult;
			}

			if (registerDto.Type == UserType.Admin.ToString())
			{
				Admin admin = _mapper.Map<Admin>(registerDto);
				_unitOfWork.AdminRepository.Add(admin);
			}
			else if (registerDto.Type == UserType.Customer.ToString())
			{
				Customer customer = _mapper.Map<Customer>(registerDto);
				_unitOfWork.CustomerRepository.Add(customer);
			}
			else if(registerDto.Type == UserType.Seller.ToString())
			{
				Seller seller = _mapper.Map<Seller>(registerDto);
				_unitOfWork.SellerRepository.Add(seller);
			}
			else
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
