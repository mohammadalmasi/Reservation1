using System;

namespace Framework.Api
{
    public class ApiResult
    {
        public int ErrorType { get; set; }
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
    }
	public sealed class ApiResult<T> : ApiResult
	{
		public ApiResult() { }
		public ApiResult(T result) { }
		public ApiResult(Exception error) { }
		public ApiResult(T result, int totalCount) { }
		public ApiResult(Exception error, int errorType) { }
		public ApiResult(Exception error, int errorType, string message) { }

		public T Result { get; set; }
		public Exception Error { get; set; }
		public int TotalCount { get; set; }
	}
}
