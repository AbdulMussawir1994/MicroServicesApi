namespace ProductsApi.Helpers
{
    public class MobileResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; } = false;

        public static MobileResponse<T> Success(T data, string message = "Success", bool status = true) => new MobileResponse<T>() { Data = data, Message = message, IsSuccess = status };

        public static MobileResponse<T> Fail(string message, bool status = false) => new MobileResponse<T>() { Message = message, IsSuccess = status };

        public static MobileResponse<T> EmptySuccess(T data, string message = "Success") => new() { IsSuccess = true, Message = message, Data = data };
    }
}
