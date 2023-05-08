using Business.Dto;
using Business.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Interfaces
{
	public interface IUserService
	{
		public IServiceOperationResult GetUserFromToken(JwtDto jwtDto);
	}
}
