namespace Inkr.Shared
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public string? Error { get; private set; }
        public int StatusCode { get; private set; }

        private Result(T data, int statusCode)
        {
            IsSuccess = true;
            Data = data;
            StatusCode = statusCode;
            Error = null;
        }

        private Result(string error, int statusCode)
        {
            IsSuccess = false;
            Error = error;
            StatusCode = statusCode;
            Data = default;
        }

        public static Result<T> Success(T data, int statusCode) => new(data, statusCode);
        public static Result<T> Failure(string error, int statusCode) => new(error, statusCode);
    }
}