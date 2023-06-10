using Business.Dto.Seller;
using Business.Result;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebAPI.Controllers
{
	[Route("api/admin")]
	[ApiController]
	public class AdminController : ControllerBase
	{
		readonly IAdminService _adminService;

		public AdminController(IAdminService adminService)
		{
			_adminService = adminService;
		}

		[HttpGet("sellers")]
		[Authorize(Roles = "Admin")]
		public IActionResult AllSellers()
		{
			try
			{
				IServiceOperationResult operationResult = _adminService.GetAllSellers();

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

		[HttpGet("all-orders")]
		[Authorize(Roles = "Admin")]
		public IActionResult AllOrders()
		{
			try
			{
				IServiceOperationResult operationResult = _adminService.AllOrders();

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

		[HttpPut("seller-approval-status")]
		[Authorize(Roles = "Admin")]
		public IActionResult UpdateSellersApprovalStatus(SellerApprovalStatusDto sellerApprovalStatusDto)
		{
			try
			{
				IServiceOperationResult operationResult = _adminService.UpdateSellerApprovalStatus(sellerApprovalStatusDto);

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
