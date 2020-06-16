using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microline.WS.Core.Context
{
    public class WSContext : IContext
    {
        private string aspKey;
        private string customerKey;
        private string password;
        private string serviceUrl;
        private string directory;
        private string cookie;

        public WSContext():this("micpg","kupac","bestpc", "http://www.microline.hr/WebServices/MOL.asmx", @"C:\Temp", null)
        {

        }

        public WSContext(string aspKey, string customerKey, string password, string serviceUrl, string directory, string cookie)
        {
            this.aspKey = aspKey;
            this.customerKey = customerKey;
            this.password = password;
            this.serviceUrl = serviceUrl;
            this.directory = directory;
            this.cookie = cookie;
        }

        public string AspKey {
            get {
                return aspKey;
            }
        }

        public string CustomerKey {
            get {
                return customerKey;
            }
        }

        public string Password
        {
            get { return password; }
        }

        

        public string ServiceURl
        {

            get { return serviceUrl; }
            set
            {
                serviceUrl = value;
                if (String.IsNullOrEmpty(serviceUrl)) throw new ArgumentNullException("Service url must me a non null and non empty string");
            }
        }

        public string Directory
        {
            get { return directory; }
        }

        public string Cookie {
            get { return cookie; }
        }

        public bool IsSavingPossible
        {
            get
            {
                return !String.IsNullOrEmpty(directory) && System.IO.Directory.Exists(directory);
            }
        }

        public bool IsMandatoryDataSet
        {
            get
            {
                return !String.IsNullOrEmpty(aspKey) && !String.IsNullOrEmpty(customerKey) && !String.IsNullOrEmpty(password) && !String.IsNullOrEmpty(serviceUrl) && !String.IsNullOrEmpty(cookie);
            }
        }
    }
}
