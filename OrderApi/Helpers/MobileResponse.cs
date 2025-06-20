namespace OrderApi.Helpers;

public class MobileResponse<T>
{
    public bool Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static MobileResponse<T> Success(T data, string message = "Success") =>
        new() { Status = true, Message = message, Data = data };

    public static MobileResponse<T> Fail(string message = "Failure") =>
        new() { Status = false, Message = message };

    public static MobileResponse<T> EmptySuccess(T data, string message = "Success") =>
        new() { Status = true, Message = message, Data = data };
}
