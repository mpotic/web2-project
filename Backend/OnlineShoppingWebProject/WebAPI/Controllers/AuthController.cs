using Business.Dto;
using Business.Result;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace WebAPI.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		IUserAuthService _userAuthService;

		public AuthController(IUserAuthService userAuthService)
		{
			_userAuthService = userAuthService;
		}

		[HttpPost("login")]
		public IActionResult LoginUser([FromBody] LoginUserDto loginDto)
		{
			try
			{
				IServiceOperationResult operationResult = _userAuthService.LoginUser(loginDto);

				if (!operationResult.IsSuccessful)
				{
					if (operationResult.ErrorCode == ServiceOperationErrorCode.Unauthorized)
					{
						return StatusCode((int)HttpStatusCode.Unauthorized);
					}
					else if (operationResult.ErrorCode == ServiceOperationErrorCode.NotFound)
					{
						return StatusCode((int)HttpStatusCode.NotFound);
					}
					
					return StatusCode((int)HttpStatusCode.BadRequest);
				}

				return Ok(operationResult.Dto);
			}
			catch (Exception e)
			{
				return StatusCode((int)HttpStatusCode.InternalServerError);
			}
		}

		[HttpPost("regiser")]
		public IActionResult RegisterUser([FromForm]RegisterUserDto registerDto)
		{
			try
			{
				IServiceOperationResult operationResult = _userAuthService.RegisterUser(registerDto);

				if (!operationResult.IsSuccessful)
				{
					if (operationResult.ErrorCode == ServiceOperationErrorCode.Conflict)
					{
						return StatusCode((int)HttpStatusCode.Conflict, operationResult.ErrorMessage);
					}
					else
					{
						return StatusCode((int)HttpStatusCode.BadRequest);
					}
				}

				return StatusCode((int)HttpStatusCode.Created);
			}
			catch (Exception e)
			{
				return StatusCode((int)HttpStatusCode.InternalServerError);
			}
		}
	}
}
