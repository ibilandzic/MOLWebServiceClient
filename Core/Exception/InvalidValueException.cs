using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microline.WS.Core
{
    public class InvalidValueException : ErrorException 
    {
        public InvalidValueException(string message) : base(message)
        {

        }

        public InvalidValueException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
