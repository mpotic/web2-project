using Data.Models;

namespace Business.TokenHelper
{
	public interface IUserTokenIssuer
	{
		string IssueAdminJWT(IAdmin admin);

		string IssueCostumerJWT(ICustomer customer);

		string IssueSellerJWT(ISeller seller);
	}
}
