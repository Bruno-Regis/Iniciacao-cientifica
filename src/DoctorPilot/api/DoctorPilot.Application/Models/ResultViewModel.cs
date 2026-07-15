namespace DoctorPilot.Application.Models
{
    public class ResultViewModel
    {
        protected ResultViewModel()
        {
            IsSuccess = true;
            Errors = [];
        }

        protected ResultViewModel(IEnumerable<string> errors)
        {
            IsSuccess = false;
            Errors = errors.ToList();
        }

        public bool IsSuccess { get; private set; }
        public IReadOnlyList<string> Errors { get; private set; }

        public static ResultViewModel Success()
            => new ResultViewModel();

        public static ResultViewModel Error(string error)
            => new ResultViewModel([error]);

        public static ResultViewModel Error(IEnumerable<string> errors)
            => new ResultViewModel(errors);
    }

    public class ResultViewModel<T> : ResultViewModel
    {
        private ResultViewModel(T? data) : base()
        {
            Data = data;
        }

        private ResultViewModel(IEnumerable<string> errors) : base(errors)
        {
            Data = default;
        }

        public T? Data { get; private set; }

        public static ResultViewModel<T> Success(T data)
            => new ResultViewModel<T>(data);

        public static ResultViewModel<T> Error(string error)
            => new ResultViewModel<T>([error]);

        public static ResultViewModel<T> Error(IEnumerable<string> errors)
            => new ResultViewModel<T>(errors);
    }
}


