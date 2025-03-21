﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinaTech.Application.Common
{
    public class Result<T>
    {
        public Result(T value, bool isSuccess, string error)
        {
            Value = value;
            IsSuccess = isSuccess;
            Error = error;
        }

        public T Value { get; }
        public bool IsSuccess { get; }
        public string Error { get; }
        public static Result<T> Success(T value) => new Result<T>(value, true, null);

        public static Result<T> Failure(string error) => new Result<T>(default, false, error);
    }
}
