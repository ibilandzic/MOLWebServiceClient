﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microline.WS.Core
{
    public class StringNotSetException : Exception
    {
        public StringNotSetException(string message) : base(message)
        {

        }

        public StringNotSetException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}