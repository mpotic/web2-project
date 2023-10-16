using Business.Dto.Article;
using Business.Dto.Auth;
using Business.Result;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace WebAPI.Controllers
{
	[Route("api/seller")]
	[ApiController]
	public class SellerController : ControllerBase
	{
		ISellerService _sellerService;

		public SellerController(ISellerService sellerService)
		{
			_sellerService = sellerService;
		}

		[HttpGet("articles")]
		[Authorize(Roles = "Seller")]
		public IActionResult AllArticles()
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = _sellerService.GetAllArticles(jwtDto);

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
		[Authorize(Roles = "Seller")]
		public IActionResult FinishedOrders()
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = _sellerService.GetFinishedOrders(jwtDto);

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
		[Authorize(Roles = "Seller")]
		public IActionResult PendingOrders()
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = _sellerService.GetPendingOrders(jwtDto);

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

		[HttpGet("article")]
		[Authorize(Roles = "Seller")]
		public IActionResult GetArticleDetails([FromQuery]string name)
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = _sellerService.GetArticleDetails(jwtDto, name);

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

		[HttpGet("order")]
		[Authorize(Roles = "Seller")]
		public IActionResult GetOrderDetails([FromQuery] long id)
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult operationResult = _sellerService.GetOrderDetails(jwtDto, id);

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

		[HttpPost("article")]
		[Authorize(Roles = "Seller")]
		public IActionResult AddArticle([FromForm] NewArticleDto article)
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult result = _sellerService.AddArticle(article, jwtDto);

				if (!result.IsSuccessful)
				{
					return StatusCode((int)result.ErrorCode, result.ErrorMessage);
				}
				
				return Ok();
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpPut("article")]
		[Authorize(Roles = "Seller")]
		public IActionResult UpdateArticle([FromBody] ArticleUpdateDto article)
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult result = _sellerService.UpdateArticle(article, jwtDto);

				if (!result.IsSuccessful)
				{
					return StatusCode((int)result.ErrorCode, result.ErrorMessage);
				}

				return Ok();
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpPut("product-image")]
		[Authorize(Roles = "Seller")]
		public IActionResult UpdateArticleProductImage([FromForm] ArticleProductImageUpdateDto article)
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult result = _sellerService.UpdateArticleProductImage(article, jwtDto);

				if (!result.IsSuccessful)
				{
					return StatusCode((int)result.ErrorCode, result.ErrorMessage);
				}

				return Ok();
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpDelete("article")]
		[Authorize(Roles = "Seller")]
		public IActionResult DeleteArticle([FromQuery] string name)
		{
			try
			{
				string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
				JwtDto jwtDto = new JwtDto(token);

				IServiceOperationResult result = _sellerService.DeleteArticle(name, jwtDto);

				if (!result.IsSuccessful)
				{
					return StatusCode((int)result.ErrorCode, result.ErrorMessage);
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
