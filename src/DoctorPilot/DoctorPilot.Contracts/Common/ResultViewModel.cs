namespace DoctorPilot.Contracts.Common
{
    public class ResultViewModel<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        public static ResultViewModel<T> Success(T data)
            => new() { IsSuccess = true, Data = data };

        public static ResultViewModel<T> Error(string message)
            => new() { IsSuccess = false, Message = message };
    }
}
