using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Data
{
    public class Result<T>
    {
        public bool Successfully { get; set; }
        public T Element { get; set; }
        public Result()
        {
            Successfully = false;
        }
        public Result(T _result)
        {
            Element = _result;
            Successfully = true;
        }
    }
}
