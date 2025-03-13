namespace OficinaTech.Domain.Common
{
    public class DomainResult<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public string Error { get; }

        private DomainResult(T value, bool isSuccess, string error)
        {
            Value = value;
            IsSuccess = isSuccess;
            Error = error;
        }

        public static DomainResult<T> Success(T value) => new DomainResult<T>(value, true, null);
        public static DomainResult<T> Failure(string error) => new DomainResult<T>(default, false, error);
    }
}