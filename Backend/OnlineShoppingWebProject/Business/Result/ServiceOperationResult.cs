using Business.Dto;

namespace Business.Result
{
	public class ServiceOperationResult : IServiceOperationResult
	{
		public ServiceOperationResult()
		{
		}

		public ServiceOperationResult(bool isSuccessful)
		{
			IsSuccessful = isSuccessful;
		}

		public ServiceOperationResult(bool isSuccessful, IDto dto)
		{
			IsSuccessful = isSuccessful;
			Dto = dto;
		}

		public ServiceOperationResult(bool isSuccessful, ServiceOperationErrorCode errorCode) : this(isSuccessful)
		{
			ErrorCode = errorCode;
		}

		public ServiceOperationResult(bool isSuccessful, ServiceOperationErrorCode errorCode, string errorMessage)
		{
			IsSuccessful = isSuccessful;
			ErrorCode = errorCode;
			ErrorMessage = errorMessage;
		}

		public bool IsSuccessful { get; set; }

		public ServiceOperationErrorCode ErrorCode { get; set; }

		public string ErrorMessage { get; set; }

		public IDto Dto { get; set; }
	}
}
