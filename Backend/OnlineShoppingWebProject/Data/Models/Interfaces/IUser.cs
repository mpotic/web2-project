using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
	public interface IUser
	{
		long Id { get; set; }

		string Firstname { get; set; }

		string Lastname { get; set; }

		string Username { get; set; }

		string Email { get; set; }

		string Password { get; set; }

		string Address { get; set; }

		byte[] ProfileImage { get; set; }
	}
}
