using Microline.WS.Connector.Service.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Microline.WS.Connector.Service.Binding
{
    public class WSBinding : IServiceBinding
    {
        public CustomBinding GetCustomBinding()
        {
            CustomBinding binding = new CustomBinding();
            binding.Name = "MOLWsBinding";
            binding.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap11, System.Text.Encoding.UTF8));

            binding.SendTimeout = new TimeSpan(0, 5, 0);
            binding.CloseTimeout = new TimeSpan(0, 5, 0);
            binding.OpenTimeout = new TimeSpan(0, 5, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 5, 0);

            return binding;

        }

        public BasicHttpBinding GetHttpBinding()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.AllowCookies = true;
            binding.TextEncoding = Encoding.UTF8;

            binding.SendTimeout = new TimeSpan(0, 5, 0);
            binding.CloseTimeout = new TimeSpan(0, 5, 0);
            binding.OpenTimeout = new TimeSpan(0, 5, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 5, 0);

            return binding;
        }

    }
}
