using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microline.WS.Core.FixedData
{
    public class ShipViaFOBKey
    {
        private int id;
        private bool chargeTransport;
        private string description;

        public ShipViaFOBKey()
        {
            id = 1;
            chargeTransport = false;
            description = "Podižemo sami";
        }

        public ShipViaFOBKey(int identifier, bool chargable, string description)
        {
            id = identifier;
            chargeTransport = chargable;
            this.description = description;
        }

        public int Id { get => id; set => id = value; }
        public bool ChargeTransport { get => chargeTransport; set => chargeTransport = value; }
        public string Description { get => description; set => description = value; }

        /// <summary>
        /// Returns available list
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, ShipViaFOBKey> GetAvailableList()
        {
            Dictionary<int, ShipViaFOBKey> select = new Dictionary<int, ShipViaFOBKey>();
            select.Add(1,new ShipViaFOBKey());
            select.Add(3,new ShipViaFOBKey(3, true, "Paketi do vrata, zaračunavamo prijevoz"));
            select.Add(5,new ShipViaFOBKey(5, true, "Palete do vrata, zaračunavamo prijevoz"));
            select.Add(9,new ShipViaFOBKey(9, false, "HP express, plaća i zove kupac"));
            select.Add(11, new ShipViaFOBKey(11, false, "Overseas, plaća i zove kupac"));
            select.Add(12, new ShipViaFOBKey(12, false, "Intereuropa, plaća i zove kupac"));
            select.Add(14, new ShipViaFOBKey(14, false, "In time, plaća i zove kupac"));
            select.Add(15, new ShipViaFOBKey(15, false, "GLS, plaća i zove kupac"));
            select.Add(16, new ShipViaFOBKey(16, true, "DHL"));
            select.Add(17, new ShipViaFOBKey(17, true, "HP"));
            select.Add(18, new ShipViaFOBKey(18, true, "GLS paketi do vrata, zaračunavamo prijevoz"));

            return select;
        }

        public static Dictionary<int, string> GetSimplifiedAvailableList()
        {
            Dictionary<int, string> select = new Dictionary<int, string>();
            var sourceDictionary = GetAvailableList();

            foreach(KeyValuePair<int, ShipViaFOBKey> fob in sourceDictionary)
            {
                select.Add(fob.Key, fob.Value.description);
            }

            return select;
        }
    }

    
}
