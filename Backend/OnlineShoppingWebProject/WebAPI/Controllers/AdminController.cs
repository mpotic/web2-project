using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
	[Route("api/admin")]
	[ApiController]
	public class AdminController : ControllerBase
	{
		[HttpGet("all-sellers")]
		[Authorize(Roles = "Admin")]
		public IActionResult GetAllSellers()
		{
			return Ok();
		}
	}
}
