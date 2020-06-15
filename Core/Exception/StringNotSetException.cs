using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microline.WS.Core.Exceptions
{
    public class StringNotSetException : ErrorException
    {
        public StringNotSetException(string message) : base(message)
        {

        }

        public StringNotSetException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
