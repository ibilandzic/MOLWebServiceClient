using Microline.WS.Core.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Microline.WS.Connector.Service.Client
{
    public class ResultHandler
    {
        private string result;
        private IContext ctx;

        public ResultHandler(IContext ctx, string result)
        {
            this.ctx = ctx;
            this.result = result;
        }

        public string Result { get => result; set { result = value; } }

        
        /// <summary>
        /// Used to extract keys from parent - child xml
        /// </summary>
        /// <param name="parentName"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public List<string> GetChildKeyList(string parentName, string childName)
        {
            List<string> resLst = new List<string>();
            if (!String.IsNullOrEmpty(result) && !String.IsNullOrEmpty(parentName) && !String.IsNullOrEmpty(childName))
            {
                XmlDocument doc = Microline.WS.XMLModel.Util.CreateXmlDocumentFromString(result);
                foreach(XmlElement el in doc.GetElementsByTagName(parentName))
                {
                    foreach(XmlElement child in el.GetElementsByTagName(childName))
                    {
                        if (!String.IsNullOrEmpty(child.InnerText)) resLst.Add(child.InnerText.TrimEnd());
                    }
                }
            }

            return resLst;
        }
    }
}
