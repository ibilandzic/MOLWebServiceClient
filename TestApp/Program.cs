using Microline.WS.Connector.Service;
using Microline.WS.Connector.Service.Client;
using Microline.WS.Core.Context;
using Microline.WS.Core.Convert;
using Microline.WS.XMLModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        
        static void  Main(string[] args)
        {
            DateTime startDate = DateTime.Now;
            testItemList();
            DateTime endDate = DateTime.Now;
            Console.WriteLine("Fetch completed");
            Console.WriteLine(String.Format("Fetched completed in {0} seconds", (endDate - startDate).TotalSeconds));
        }


        private static void testItemList()
        {
            string aspKey = "micpg";
            string customerKey = "kupac";
            string password = "bestpc";
            string cookie = "39983830859dJFaogrSxpgIcfvfy1T";

            System.IO.FileInfo f = new FileInfo(String.Format(@"C:\Temp\itemList_{0}.xml", DateTime.Now.ToString("ddMMyyyy_HH_mm")));

            using (StreamWriter sw = f.AppendText())
            {
                try
                {
                    WSContext ctx = new WSContext(aspKey, customerKey, password, @"http://www.microline.hr/WebServices/MOL.asmx", @"C:\Temp", cookie);
                    WSClient client = new WSClient(ctx);
                    var report = client.GetAllItemsFiltered(null, "sam", "itemsFilteredSam");
                    sw.WriteLine(report);
                }
                catch (Exception ex)
                {
                    sw.WriteLine(ex.Message);
                    sw.WriteLine(ex.StackTrace);
                    if (ex.InnerException != null)
                    {

                        sw.WriteLine(ex.InnerException.Message);
                        sw.WriteLine(ex.InnerException.StackTrace);

                        if (ex.InnerException.InnerException != null)
                        {

                            sw.WriteLine(ex.InnerException.InnerException.Message);
                            sw.WriteLine(ex.InnerException.InnerException.StackTrace);
                        }
                    }
                }
            }
        }

        private static async Task testItemListAsync()
        {
            string aspKey = "micpg";
            string customerKey = "kupac";
            string password = "bestpc";
            string cookie = "39983830859dJFaogrSxpgIcfvfy1T";

            System.IO.FileInfo f = new FileInfo(String.Format(@"C:\Temp\itemList_{0}.xml", DateTime.Now.ToString("ddMMyyyy_HH_mm")));

            using(StreamWriter sw = f.AppendText())
            {
                try
                {
                    WSContext ctx = new WSContext(aspKey, customerKey, password, @"http://www.microline.hr/WebServices/MOL.asmx", @"C:\Temp", cookie);
                    WSClient client = new WSClient(ctx);
                    var report = await client.GetAllItemsFilteredAsync(null, "sam", "itemsFilteredSam");
                    sw.WriteLine(report);
                }
                catch(Exception ex)
                {
                    sw.WriteLine(ex.Message);
                    sw.WriteLine(ex.StackTrace);
                    if (ex.InnerException != null)
                    {

                        sw.WriteLine(ex.InnerException.Message);
                        sw.WriteLine(ex.InnerException.StackTrace);

                        if (ex.InnerException.InnerException != null)
                        {

                            sw.WriteLine(ex.InnerException.InnerException.Message);
                            sw.WriteLine(ex.InnerException.InnerException.StackTrace);
                        }
                    }
                }
            }

            
        }

        private static void testXml()
        {
            System.IO.FileInfo f = new FileInfo(@"C:\Temp\Report_XMLSerialization.txt");

            using (StreamWriter sw = f.AppendText())
            {
                try
                {
                    SO so = new SO(false, null, null, "Pero Perić", "Ilica 1", 1, "Perici", null, 1, "1", false, new System.Collections.Generic.List<Item> { new Item("abc", 1), new Item("def", 3) });
                    string result = so.SerializeToStringNoDeclaration();
                    sw.WriteLine(result);
                }
                catch (Exception ex)
                {
                    sw.WriteLine(ex.Message);
                    sw.WriteLine(ex.StackTrace);
                    if (ex.InnerException != null)
                    {

                        sw.WriteLine(ex.InnerException.Message);
                        sw.WriteLine(ex.InnerException.StackTrace);

                        if (ex.InnerException.InnerException != null)
                        {

                            sw.WriteLine(ex.InnerException.InnerException.Message);
                            sw.WriteLine(ex.InnerException.InnerException.StackTrace);
                        }
                    }

                }
            }
        }
    }
}
