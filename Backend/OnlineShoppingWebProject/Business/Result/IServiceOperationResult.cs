using Business.Dto;

namespace Business.Result
{
	public interface IServiceOperationResult
	{
		bool IsSuccessful { get; set; }

		ServiceOperationErrorCode ErrorCode { get; set; }
		
		string ErrorMessage { get; set; }

		IDto Dto { get; set; }
	}
}
