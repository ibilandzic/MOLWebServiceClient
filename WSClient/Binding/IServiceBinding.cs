using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Microline.WS.Connector.Service.Binding
{
    public interface IServiceBinding
    {
        CustomBinding GetCustomBinding();

        BasicHttpBinding GetHttpBinding();
    }
}
