using Data.Models;

namespace Business.TokenHelper
{
	public interface IUserTokenIssuer
	{
		string IssueAdminJwt(IAdmin admin);

		string IssueCostumerJwt(ICustomer customer);

		string IssueSellerJwt(ISeller seller);
		
		string IssueUserJwt(IUser user);
	}
}
