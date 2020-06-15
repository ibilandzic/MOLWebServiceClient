using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microline.WS.Core
{
    public class ErrorException : Exception
    {
        public ErrorException(string message) : base(message)
        {
            ExceptionHandler.Log(this);
        }

        public ErrorException(string message, Exception innerException) : base(message, innerException)
        {
            ExceptionHandler.Log(this);
        }

        
    }
}
