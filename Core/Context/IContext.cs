using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microline.WS.Core.Context
{
    public interface IContext
    {
        string AspKey { get;}
        string CustomerKey { get;}
        string Password { get;}
        string ServiceURl { get; set; }
        string Directory { get;}
        string Cookie { get;}
        bool IsSavingPossible { get; }
        bool IsMandatoryDataSet { get; }
    }
}
