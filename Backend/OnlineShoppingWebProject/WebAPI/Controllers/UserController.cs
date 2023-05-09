using Business.Dto;
using Business.Dto.User;
using Business.Result;
using Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace WebAPI.Controllers
{
	[Route("api/user")]
	[ApiController]
	public class UserController : ControllerBase
	{
		IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPut("update-user")]
		[Authorize]
		public IActionResult UpdateUser([FromBody] BasicUserInfoDto userDto)
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = _userService.UpdateUser(userDto, jwtDto);

				if (!operationResult.IsSuccessful)
				{
					return StatusCode((int)operationResult.ErrorCode, operationResult.ErrorMessage);
				}

				return Ok();
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpPut("change-password")]
		[Authorize]
		public IActionResult ChangePassword([FromBody] PasswordChangeDto passwordDto)
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = _userService.ChangePassword(passwordDto, jwtDto);

				if (!operationResult.IsSuccessful)
				{
					return StatusCode((int)operationResult.ErrorCode, operationResult.ErrorMessage);
				}

				return Ok();
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpPut("get-user")]
		[Authorize]
		public IActionResult GetUser()
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = _userService.GetUser(jwtDto);

				if (!operationResult.IsSuccessful)
				{
					return StatusCode((int)operationResult.ErrorCode, operationResult.ErrorMessage);
				}

				return Ok(operationResult.Dto);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpPut("change-profile-image")]
		[Authorize]
		public IActionResult ChangeProfileImage([FromForm] ProfileImageDto profileImageDto)
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = _userService.UploadProfileImage(profileImageDto, jwtDto);

				if (!operationResult.IsSuccessful)
				{
					return StatusCode((int)operationResult.ErrorCode, operationResult.ErrorMessage);
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
