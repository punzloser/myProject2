using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Result
{
    public class ErrorResult<T> : CommonResult<T>
    {
        public ErrorResult()
        {
        }

        public ErrorResult(string message)
        {
            IsSuccessed = false;
            Message = message;
        }
    }
}
