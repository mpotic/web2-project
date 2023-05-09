using Business.Dto;
using Business.Result;
using Business.Services;
using Microsoft.AspNetCore.Http;
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
					return StatusCode((int)operationResult.ErrorCode);
				}

				return Ok(operationResult.Dto);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
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

				return Ok();
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
