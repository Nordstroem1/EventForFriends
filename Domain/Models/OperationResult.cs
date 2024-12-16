namespace Domain.Models
{
    public class OperationResult<T>
    {
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
        public string FailLocation { get; set; }
        public OperationResult(bool succeeded, string errorMessage, T data, string location)
        {
            Succeeded = succeeded;
            ErrorMessage = errorMessage;
            Data = data;
            FailLocation = location;
        }
        public static OperationResult<T> Success(T data)
        {
            return new OperationResult<T>(true, string.Empty, data, null);
        }
        public static OperationResult<T> Fail(string errorMessage, string location)
        {
            return new OperationResult<T>(false, errorMessage, default, location);
        }
    }
}
