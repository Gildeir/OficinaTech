using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinaTech.Infrastructure.ExternalServices
{
    public class ExternalServiceResult<T>(T? value, bool isSucess, string? error)
    {
        public T? Value { get; set; } = value;
        public bool IsSucess { get; set; } = isSucess;
        public string? Error { get; set; } = error;

        public static ExternalServiceResult<T> Success(T value) => new ExternalServiceResult<T>(value, true, null);
        public static ExternalServiceResult<T> Failure(string error) => new ExternalServiceResult<T>(default, false, error);
    }
}
