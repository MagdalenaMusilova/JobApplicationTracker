namespace JobApplicationTracker.Services;

public class AuthServiceResultDto<T>
{
    public bool Succeeded { get; private init; }

    public T? Value { get; private init; }

    public string? ErrorMessage { get; private init; }

    public int StatusCode { get; private init; }

    public static AuthServiceResultDto<T> Success(T value)
    {
        return new AuthServiceResultDto<T>
        {
            Succeeded = true,
            Value = value,
            StatusCode = StatusCodes.Status200OK
        };
    }

    public static AuthServiceResultDto<T> Failure(int statusCode, string errorMessage)
    {
        return new AuthServiceResultDto<T>
        {
            Succeeded = false,
            StatusCode = statusCode,
            ErrorMessage = errorMessage
        };
    }
}