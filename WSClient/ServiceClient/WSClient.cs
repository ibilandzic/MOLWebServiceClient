using Microline.WS.Connector.Service;
using Microline.WS.Connector.Service.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Microline.WS.Connector.Service.Client
{
    public class WSClient
    {
        private string endpointUrl;

        public WSClient() { }
        public WSClient(string url)
        {
            endpointUrl = url;
        }

        public MOLSoapClient getClient()
        {
            if (String.IsNullOrEmpty(endpointUrl)) throw new MissingFieldException("Url is not set");
            MOLBinding binding = new MOLBinding();
            MOLSoapClient client = new MOLSoapClient(binding.GetHttpBinding(), new EndpointAddress(endpointUrl));

            return client;
        }

        public MOLSoapClient getClient(string url)
        {
            endpointUrl = url;
            return getClient();
        }
    }
}
