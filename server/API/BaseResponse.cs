using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loltest.API
{
    public class BaseResponse<T>
    {
        public string message { get; set; }

        public T[] data { get; set; }
    }
}