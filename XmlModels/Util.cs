using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Microline.WS.XMLModel
{
    public class Util
    {
        public static XmlDocument CreateXmlDocumentFromString(string input)
        {
            if (String.IsNullOrEmpty(input)) throw new ArgumentException("Input must be a non null string");
            else
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(input);
                    return doc;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

    }
}
