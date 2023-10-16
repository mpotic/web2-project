using Business.Dto.Auth;
using Business.Result;
using Business.Services;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

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
					return StatusCode((int)operationResult.ErrorCode, operationResult.ErrorMessage);
				}

				return Ok(operationResult.Dto);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpPost("register")]
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
						return StatusCode((int)HttpStatusCode.BadRequest, "Error doing the request...");
					}
				}

				return Ok();
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpPost("google-login")]
		public async Task<IActionResult> GoogleLogin(GoogleToken token)
		{
			try
			{
				IServiceOperationResult operationResult = await _userAuthService.GoogleLogin(token.Token);

				if (!operationResult.IsSuccessful)
				{
					return StatusCode((int)operationResult.ErrorCode, operationResult.ErrorMessage);
				}

				return Ok(operationResult.Dto);
			}
			catch (InvalidJwtException ex)
			{
				// Handle invalid token exception
				return Unauthorized(ex.Message);
			}
			catch (Exception ex)
			{
				// Handle other exceptions
				return StatusCode(500, ex.Message);
			}
		}
	}
}
