using Common.Enums;
using System.Runtime.Serialization;

namespace WebProjekat.Common
{
    [DataContract]
    public record Error(string Message, ErrorType Type);
    [DataContract]
    public class Result
    {
        [DataMember]
        public bool IsSuccess { get; private set; }
        [DataMember]
        public Error? Error { get; private set; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, Error? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public Result() { }

        public static Result Success() => new Result(true, null);
        public static Result Failure(string message, ErrorType type = ErrorType.Failure)
            => new Result(false, new Error(message, type));
    }
    [DataContract]
    public class Result<T> : Result
    {
        [DataMember]
        public T Value { get; private set; }

        protected Result(T value, bool isSuccess, Error? error)
            : base(isSuccess, error)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(value, true, null);
        public static new Result<T> Failure(string message, ErrorType type = ErrorType.Failure)
                    => new Result<T>(default!, false, new Error(message, type));
    }
}
