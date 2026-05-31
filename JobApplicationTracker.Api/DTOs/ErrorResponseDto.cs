namespace JobApplicationTracker.DTOs;

public class ErrorResponseDto
{
    public string Message { get; set; }
    public string Code { get; set; }
    
    /*public ErrorResponseDto()
    {
    }*/

    public ErrorResponseDto(string code, string message)
    {
        Code = code;
        Message = message;
    }
}