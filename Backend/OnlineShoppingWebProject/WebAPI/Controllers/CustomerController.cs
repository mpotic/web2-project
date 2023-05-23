using Business.Dto.Auth;
using Business.Dto.Order;
using Business.Result;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace WebAPI.Controllers
{
	[Route("api/customer")]
	[ApiController]
	public class CustomerController : ControllerBase
	{
		ICustomerService _customerService;

		public CustomerController(ICustomerService customerService)
		{
			_customerService = customerService;
		}

		[HttpGet("articles")]
		[Authorize(Roles = "Customer")]
		public IActionResult GetAllArticles()
		{
			try
			{
				IServiceOperationResult operationResult = _customerService.GetAllArticles();

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

		[HttpGet("finished-orders")]
		[Authorize(Roles = "Customer")]
		public IActionResult GetFinishedOrders()
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = _customerService.GetFinishedOrders(jwtDto);

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

		[HttpGet("pending-orders")]
		[Authorize(Roles = "Customer")]
		public IActionResult GetPendingOrders()
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = _customerService.GetPendingOrders(jwtDto);

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

		[HttpPost("order")]
		[Authorize(Roles = "Customer")]
		public IActionResult PlaceOrder(PlaceOrderDto orderDto)
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = _customerService.PlaceOrder(orderDto, jwtDto);

				if (!operationResult.IsSuccessful)
				{
					return StatusCode((int)operationResult.ErrorCode, operationResult.ErrorMessage);
				}

				return Ok();
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpDelete("order")]
		[Authorize(Roles = "Customer")]
		public IActionResult CancelOrder(long orderId)
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = _customerService.CancelOrder(orderId, jwtDto);

				if (!operationResult.IsSuccessful)
				{
					return StatusCode((int)operationResult.ErrorCode, operationResult.ErrorMessage);
				}

				return Ok();
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
