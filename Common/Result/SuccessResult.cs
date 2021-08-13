using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Result
{
    public class SuccessResult<T> : CommonResult<T>
    {
        public SuccessResult()
        {
            IsSuccessed = true;
        }

        public SuccessResult(T resultObj)
        {
            IsSuccessed = true;
            ResultObj = resultObj;
        }

    }
}
