using Microline.WS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Microline.WS.XMLModel
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute()]
    public class Item
    {
        private string key;
        private int quantity;

        
        public Item()
        {
            key = "";
            quantity = 0;
        }

        public Item(string itemKey, int qty)
        {
            key = itemKey;
            quantity = qty;
        }

        [XmlElementAttribute("key")]
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }

        [XmlElementAttribute("quantity")]
        public string Quantity
        {
            get
            {
                return quantity.ToString();
            }
            set
            {
                if (checkQuantityValue(value)) quantity = Microline.WS.Core.Convert.DataConverter.ToInt32(value);
                else throw new InvalidValueException("Value is not integer");
            }
        }




        private bool checkQuantityValue(string input)
        {
            int qty;
            return int.TryParse(input, out qty);
        }

    }
}
