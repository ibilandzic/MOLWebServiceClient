using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microline.WS.Core.FixedData
{
    public class ARTerms
    {
        private string termsKey;
        private string descr;

        public ARTerms()
        {
            termsKey ="1";
            descr = "Unaprijed";
        }

        public ARTerms(string termsKey, string descr)
        {
            if (String.IsNullOrEmpty(termsKey))
            {
                termsKey = "1";
                descr = "Unaprijed";
            }
            else
            {
                this.termsKey = termsKey;
                this.descr = descr;
            }
        }

        public static Dictionary<string, ARTerms> GetSelect()
        {
            Dictionary<string, ARTerms> terms = new Dictionary<string, ARTerms>();

            terms.Add("1", new ARTerms());
            terms.Add("2", new ARTerms("2","Po računu"));
            terms.Add("3", new ARTerms("3", "15 dana"));
            terms.Add("4", new ARTerms("4", "30 dana"));
            terms.Add("5", new ARTerms("5", "7 dana"));
            terms.Add("8", new ARTerms("8", "22 dana"));
            terms.Add("k", new ARTerms("k", "B2B 7 dana"));
            terms.Add("r", new ARTerms("r", "B2B 15 dana"));
            terms.Add("t", new ARTerms("t", "B2B 22 dana"));
            terms.Add("w", new ARTerms("w", "B2B po računu"));
            terms.Add("x", new ARTerms("x", "B2B 30 dana"));
            terms.Add("y", new ARTerms("y", "B2B 45 dana"));
            terms.Add("z", new ARTerms("z", "B2B 60 dana"));

            return terms;
        }
    }
}
