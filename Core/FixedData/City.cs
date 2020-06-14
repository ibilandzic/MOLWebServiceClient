using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microline.WS.Core.FixedData
{
    public class City
    {
      

        public static Dictionary<int?, string> GetAvailableCities()
        {
            Dictionary<int?, string> cities = new Dictionary<int?, string>();
            cities.Add(null, "Prazno");
            cities.Add(1, "Zagreb");
            cities.Add(2, "Split");
            cities.Add(3, "Rijeka");

            return cities;

        }

    }

    
}
