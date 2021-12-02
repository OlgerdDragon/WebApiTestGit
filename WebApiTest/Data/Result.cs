using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiGeneralGrpc.Data
{
    public class Result<T>
    {
        public bool Successfully { get; set; }
        public T Element { get; set; }
        public Exception ExceptionMessage { get; set; }
        public Result(Exception ex)
        {
            ExceptionMessage = ex;
            Successfully = false;
        }
        public Result(T _result)
        {
            Element = _result;
            Successfully = true;
        }
    }
}
