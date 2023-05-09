﻿using AutoMapper;
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
		private IUserTokenIssuer _tokenIssuer;

		private IUnitOfWork _unitOfWork;

		private IMapper _mapper;

		private IAuthHelper authHelper = new AuthHelper();

		private IUserHelper userHelper;

		public UserAuthService(IUserTokenIssuer userTokenIssuer, IUnitOfWork unitOfWork, IMapper mapper)
		{
			_tokenIssuer = userTokenIssuer;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			userHelper = new UserHelper(_unitOfWork);
		}

		public IServiceOperationResult LoginUser(LoginUserDto loginDto)
		{
			IServiceOperationResult operationResult;

			IUser user = userHelper.FindUserByUsername(loginDto.Username);

			if (user == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);

				return operationResult;
			}

			if (!authHelper.IsPasswordValid(loginDto.Password, user.Password))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.Unauthorized);

				return operationResult;
			}

			string token = _tokenIssuer.IssueUserJwt(user);

			if (token == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.InternalServerError);

				return operationResult;
			}

			operationResult = new ServiceOperationResult(true, new JwtDto(token));

			return operationResult;
		}

		public IServiceOperationResult RegisterUser(RegisterUserDto registerDto)
		{
			IServiceOperationResult operationResult;

			if (userHelper.FindUserByUsername(registerDto.Username) != null ||
				userHelper.FindUserByEmail(registerDto.Email) != null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.Conflict, "User already exists!");

				return operationResult;
			}

			if (authHelper.IsPasswordWeak(registerDto.Password) || !authHelper.IsEmailValid(registerDto.Email))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest);

				return operationResult;
			}

			registerDto.Password = authHelper.HashPassword(registerDto.Password);

			IUser newUser = CreateUserAndAddToRepository(registerDto);
			if (newUser == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest);

				return operationResult;
			}

			_unitOfWork.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}

		private IUser CreateUserAndAddToRepository(RegisterUserDto registerDto)
		{
			if (registerDto.Type == UserType.Admin.ToString())
			{
				Admin admin = _mapper.Map<Admin>(registerDto);
				userHelper.UploadProfileImage(admin, registerDto.ProfileImage);
				_unitOfWork.AdminRepository.Add(admin);

				return admin;
			}
			else if (registerDto.Type == UserType.Customer.ToString())
			{
				Customer customer = _mapper.Map<Customer>(registerDto);
				userHelper.UploadProfileImage(customer, registerDto.ProfileImage);
				_unitOfWork.CustomerRepository.Add(customer);

				return customer;
			}
			else if (registerDto.Type == UserType.Seller.ToString())
			{
				Seller seller = _mapper.Map<Seller>(registerDto);
				seller.ApprovalStatus = SellerApprovalStatus.Pending;
				userHelper.UploadProfileImage(seller, registerDto.ProfileImage);
				_unitOfWork.SellerRepository.Add(seller);

				return seller;
			}

			return null;
		}
	}
}
