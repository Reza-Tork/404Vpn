using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class Result<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public Result(bool isSuccess, string message, T? data = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public Result(bool isSuccess, T? data = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = null;
        }

        public static Result<T> Success(string message, T? data = null)
        {
            return new Result<T>(true, message, data);
        }
        public static Result<T> Failure(string message, T? data = null)
        {
            return new Result<T>(false, message, data);
        }
    }
}
