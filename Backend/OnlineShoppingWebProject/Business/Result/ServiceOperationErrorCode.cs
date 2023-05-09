namespace Business.Result
{
	public enum ServiceOperationErrorCode
	{
		BadRequest = 400,
		Unauthorized = 401,
		NotFound = 404,
		InternalServerError = 500,
		Conflict = 409
	}
}
