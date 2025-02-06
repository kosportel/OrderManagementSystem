﻿namespace OrderManagement.Application.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? Error { get; }
        public T? Value { get; }

        private Result(T value, bool isSuccess, string? error)
        {
            Value = value;
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result<T> Success(T value) => new(value, true, null);
        public static Result<T> Failure(string error) => new(default, false, error);
    }
}
