using Microline.WS.Core.Convert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Microline.WS.XMLModel
{
    [SerializableAttribute()]
    [XmlRoot("so")]
    public class SO : XMLModelFoundation
    {
        private DateTime moment;
        private bool deliverImmediately;
        private string documentNumber;
        private string shipToKey;
        private string shipToName;
        private string shipToAddress1;
        private int? shipToCityId;
        private string shipToAttention;
        private string shipViaKey;
        private int shipViaAndFobKey;
        private string termsKey;
        private bool payAfterSold;
        private List<Item> items;

        private int? pk;

        public SO(): this(false, null, null, null, null, null, null, null, 1, null, false, null)
        {

        }

        public SO(bool deliverImmediately, string documentNumber, string shipToKey, string shipToName, string shipToAddress1, int? shipToCityId, string shipToAttention, string shipViaKey,
            int shipViaAndFobKey, string termsKey, bool payAfterSold, List<Item> items)
        {
            this.moment = DateTime.Now;
            this.deliverImmediately = deliverImmediately;
            this.documentNumber = documentNumber;
            this.shipToKey = shipToKey;
            this.shipToName = shipToName;
            this.shipToAddress1 = shipToAddress1;
            this.shipToCityId = shipToCityId;
            this.shipToAttention = shipToAttention;
            this.shipViaKey = shipViaKey;
            this.shipViaAndFobKey = shipViaAndFobKey;
            this.termsKey = termsKey;
            this.payAfterSold = payAfterSold;
            this.items = items;
        }



        [XmlElementAttribute("moment")]
        public String Moment
        {
            get
            {
                return moment.ToString("dd.MM.yyyy. HH:mm:ss");
            }

            set
            {
                if (value== null)
                {
                    moment = DateTime.Now;
                }
                else
                {
                    DateTime date;
                    if (!DateTime.TryParse(value, out date))
                    {
                        date = DateTime.Now;
                    }

                    moment = date;
                }
            }
        }

        [XmlElementAttribute("deliverImmediately")]
        public string DeliverImmediately
        {
            get { return deliverImmediately ? "true" : "false"; }
            set { deliverImmediately = DataConverter.ToBoolean(value); }
        }

        /// <summary>
        /// optional
        /// </summary>
        [XmlElementAttribute("documentNumber")]
        public string DocumentNumber
        {
            get { return documentNumber; }
            set { documentNumber = value; }
        }

        /// <summary>
        /// optional
        /// </summary>
        [XmlElementAttribute("shipToKey")]
        public string ShipToKey
        {
            get {
                return shipToKey;
            }
            set
            {
                shipToKey = value;
            }

        }

        [XmlElementAttribute("shipToName")]
        public string ShipToName { get => shipToName; set => shipToName = value; }

        [XmlElementAttribute("shipToAddress1")]
        public string ShipToAddress1 { get => shipToAddress1; set => shipToAddress1 = value; }

        [XmlElementAttribute("shipToCityId")]
        public string ShipToCityId
        {
            get
            {
                return shipToCityId.HasValue ? shipToCityId.Value.ToString() : "";
            }
            set
            {
                if (value != null && value != "") shipToCityId = DataConverter.ToInt32(value);
                else shipToCityId = null;
            }
        }

        [XmlElementAttribute("shipToAttention")]
        public string ShipToAttention { get => shipToAttention; set => shipToAttention = value; }

        /// <summary>
        /// optional
        /// </summary>
        [XmlElementAttribute("shipViaKey")]
        public string ShipViaKey { get => shipViaKey; set => shipViaKey = value; }

        [XmlElementAttribute("shipViaAndFobKey")]
        public int ShipViaAndFobKey {
            get { return shipViaAndFobKey; }
            set { shipViaAndFobKey = value; }
        }

        [XmlElementAttribute("termsKey")]
        public string TermsKey { get => termsKey; set => termsKey = value; }

        [XmlElementAttribute("payAfterSold")]
        public string  PayAfterSold { get => payAfterSold ? "true" : "false"; set => payAfterSold = DataConverter.ToBoolean(value); }

        [XmlArray("items")]
        [XmlArrayItem("item")]
        public List<Item> Items { get { return items; } set => items = value; }

    }
}
