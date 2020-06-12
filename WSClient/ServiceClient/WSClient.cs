using Microline.WS.Connector.Service;
using Microline.WS.Connector.Service.Binding;
using Microline.WS.Core.Context;
using Microline.WS.Core.Convert;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Microline.WS.Connector.Service.Client
{
    public class WSClient
    {

        private IContext ctx;
        public WSClient(IContext ctx)
        {
            this.ctx = ctx;
        }

        /// <summary>
        /// Returns client
        /// </summary>
        /// <returns></returns>
        public MOLSoapClient getClient()
        {
            if (String.IsNullOrEmpty(ctx.ServiceURl)) throw new MissingFieldException("Url is not set");
            MOLBinding binding = new MOLBinding();
            MOLSoapClient client = new MOLSoapClient(binding.GetHttpBinding(), new EndpointAddress(ctx.ServiceURl));

            return client;
        }


        /// <summary>
        /// Returns client with different url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public MOLSoapClient getClient(string url)
        {
            if (!String.IsNullOrEmpty(url)) this.ctx.ServiceURl = url;
            return getClient();
        }

        #region Get items details
        /// <summary>
        /// Gets all active items
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="termsKey"></param>
        /// <returns></returns>
        public string GetAllItems(string fileName,int qty = 0, string termsKey = "1", bool detailed = true)
        {
            StringBuilder sb = new StringBuilder();
            List<string> items = getItemKeyList();
            if (items.Count == 0)
            {
                sb.AppendLine("Nije dohvaćen niti jedan artikal");
            }
            else
            {
                if (String.IsNullOrEmpty(fileName)) fileName = "allItems";
                getItemDetails(fileName, items, ref sb, qty, termsKey, detailed);
            }

            return sb.ToString();

        }

        /// <summary>
        /// Gets all active items details in async matter
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="qty"></param>
        /// <param name="termsKey"></param>
        /// <param name="detailed"></param>
        /// <returns></returns>
        public async Task<string> GetAllItemsAsync(string fileName, int qty = 0, string termsKey = "1", bool detailed = true)
        {
            StringBuilder sb = new StringBuilder();
            List<string> items = getItemKeyList();
            if (items.Count == 0)
            {
                sb.AppendLine("Nije dohvaćen niti jedan artikal");
            }
            else
            {
                if (String.IsNullOrEmpty(fileName)) fileName = "allItems";
                var result = await getItemDetailsAsync(fileName, items, qty, termsKey, detailed);
                sb.AppendLine(result);
            }

            return sb.ToString();
        }

        

        /// <summary>
        /// Gets all active items filtered by item type and/or item trademark
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="tradeMark"></param>
        /// <param name="fileName"></param>
        /// <param name="termsKey"></param>
        /// <param name="qty"></param>
        /// <param name="detailed"></param>
        /// <returns></returns>
        public string GetItemsFiltered(string itemType, string tradeMark, string fileName, string termsKey = "1", int qty = 0, bool detailed = true)
        {
            StringBuilder sb = new StringBuilder();
            List<string> items = getItemKeyListFiltered(itemType, tradeMark);
            if (items.Count == 0)
            {
                sb.AppendLine("Nije dohvaćen niti jedan artikal");
            }
            else
            {
                if (String.IsNullOrEmpty(fileName)) fileName = "allItems";
                getItemDetails(fileName, items, ref sb, qty, termsKey, detailed);
            }

            return sb.ToString();
        }


        public async Task<string> GetItemsFilteredAsync(string itemType, string tradeMark, string fileName, string termsKey = "1", int qty = 0, bool detailed = true)
        {
            StringBuilder sb = new StringBuilder();
            List<string> items = getItemKeyListFiltered(itemType, tradeMark);
            if (items.Count == 0)
            {
                sb.AppendLine("Nije dohvaćen niti jedan artikal");
            }
            else
            {
                if (String.IsNullOrEmpty(fileName)) fileName = "allItems";
                var result = await getItemDetailsAsync(fileName, items, qty, termsKey, detailed);
                sb.AppendLine(result);
            }

            return sb.ToString();

        }

        #endregion

        #region Getting helper data (terms, subtypes)
        /// <summary>
        /// Gets all available terms
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> GetARTerms()
        {
            Dictionary<string, string> terms = new Dictionary<string, string>();

            try
            {
                MOLSoapClient client = getClient();
                string result = client.arTermsList(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie);

                if (!String.IsNullOrEmpty(result))
                {
                    XmlDocument doc = Microline.WS.XMLModel.Util.CreateXmlDocumentFromString(result);
                    foreach(XmlElement el in doc.GetElementsByTagName("term"))
                    {
                        string key = ((XmlElement)el.GetElementsByTagName("key")[0]).InnerText;
                        string description = ((XmlElement)el.GetElementsByTagName("description")[0]).InnerText;
                        if (!terms.ContainsKey(key)) terms.Add(key, description);
                    }
                }

                return terms;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        public async Task<Dictionary<string,string>> GetARTermsAsync()
        {
            Dictionary<string, string> terms = new Dictionary<string, string>();

            try
            {
                MOLSoapClient client = getClient();
                var result = await client.arTermsListAsync(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie);

                if (result != null && !String.IsNullOrEmpty(result.Body.arTermsListResult))
                {
                    XmlDocument doc = Microline.WS.XMLModel.Util.CreateXmlDocumentFromString(result.Body.arTermsListResult);
                    foreach (XmlElement el in doc.GetElementsByTagName("term"))
                    {
                        string key = ((XmlElement)el.GetElementsByTagName("key")[0]).InnerText;
                        string description = ((XmlElement)el.GetElementsByTagName("description")[0]).InnerText;
                        if (!terms.ContainsKey(key)) terms.Add(key, description);
                    }
                }

                return terms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets item types list
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetItemTypes()
        {
            Dictionary<string, string> types = new Dictionary<string, string>();

            try
            {
                MOLSoapClient client = getClient();
                string result = client.itemTypeList(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie);

                if (!String.IsNullOrEmpty(result))
                {
                    XmlDocument doc = Microline.WS.XMLModel.Util.CreateXmlDocumentFromString(result);
                    foreach (XmlElement el in doc.GetElementsByTagName("type"))
                    {
                        string key = ((XmlElement)el.GetElementsByTagName("key")[0]).InnerText;
                        string description = ((XmlElement)el.GetElementsByTagName("description")[0]).InnerText;
                        if (!types.ContainsKey(key)) types.Add(key, description);
                    }
                }

                return types;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets all item types asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetItemTypesAsync()
        {
            Dictionary<string, string> types = new Dictionary<string, string>();

            try
            {
                MOLSoapClient client = getClient();
                var result = await client.itemTypeListAsync(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie);

                if (result != null && !String.IsNullOrEmpty(result.Body.itemTypeListResult))
                {
                    XmlDocument doc = Microline.WS.XMLModel.Util.CreateXmlDocumentFromString(result.Body.itemTypeListResult);
                    foreach (XmlElement el in doc.GetElementsByTagName("type"))
                    {
                        string key = ((XmlElement)el.GetElementsByTagName("key")[0]).InnerText;
                        string description = ((XmlElement)el.GetElementsByTagName("description")[0]).InnerText;
                        if (!types.ContainsKey(key)) types.Add(key, description);
                    }
                }

                return types;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Gets trademark list
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetTrademarkList()
        {
            Dictionary<string, string> trademarks = new Dictionary<string, string>();

            try
            {
                MOLSoapClient client = getClient();
                string result = client.tradeMarkList(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie);

                if (!String.IsNullOrEmpty(result))
                {
                    XmlDocument doc = Microline.WS.XMLModel.Util.CreateXmlDocumentFromString(result);
                    foreach (XmlElement el in doc.SelectNodes("tradeMarks/tradeMark"))
                    {
                        string key = el.SelectSingleNode("key").InnerText;//((XmlElement)el.GetElementsByTagName("key")[0]).InnerText;
                        string description = el.SelectSingleNode("description").InnerText;
                        if (!trademarks.ContainsKey(key)) trademarks.Add(key, description);
                    }
                }

                return trademarks;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets trademark list async
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetTrademarkListAsync()
        {
            Dictionary<string, string> trademarks = new Dictionary<string, string>();

            try
            {
                MOLSoapClient client = getClient();
                var result = await client.tradeMarkListAsync(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie);
                if (result != null && !String.IsNullOrEmpty(result.Body.tradeMarkListResult))
                {
                    XmlDocument doc = Microline.WS.XMLModel.Util.CreateXmlDocumentFromString(result.Body.tradeMarkListResult);
                    foreach (XmlElement el in doc.SelectNodes("tradeMarks/tradeMark"))
                    {
                        string key = el.SelectSingleNode("key").InnerText;//((XmlElement)el.GetElementsByTagName("key")[0]).InnerText;
                        string description = el.SelectSingleNode("description").InnerText;
                        if (!trademarks.ContainsKey(key)) trademarks.Add(key, description);
                    }
                }

                return trademarks;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion

        #region Private methods
        /// <summary>
        /// Get list of all items
        /// </summary>
        /// <returns></returns>
        private List<string> getItemKeyList()
        {
            try
            {
                MOLSoapClient client = getClient();
                string result = client.itemList(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie);
                return new ResultHandler(ctx, result).GetChildKeyList("items", "key");
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        /// <summary>
        /// Item list filtered by item type and/or trademark
        /// 
        /// Returns empty list of no filter is applied
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="tradeMarkKey"></param>
        /// <returns></returns>
        private List<string> getItemKeyListFiltered(string itemType, string tradeMarkKey)
        {
            try
            {
                if (!String.IsNullOrEmpty(itemType) || !String.IsNullOrEmpty(tradeMarkKey))
                {
                    MOLSoapClient client = getClient();
                    string result = client.itemListFiltered(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie, tradeMarkKey, itemType, true);
                    return new ResultHandler(ctx, result).GetChildKeyList("items", "key");
                }
                else
                {
                    return new List<string>();
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Gets detaild for item list. Item list can be fetch in diffrent wazs
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="items"></param>
        /// <param name="sb"></param>
        /// <param name="qty"></param>
        /// <param name="termsKey"></param>
        private void getItemDetails(string fileName, List<string> items, ref StringBuilder sb, int qty = 0, string termsKey = "1", bool detailed = true)
        {
            MOLSoapClient client = getClient();
            if (ctx.IsSavingPossible && !String.IsNullOrEmpty(fileName) && items != null &&items.Count > 0)
            {
                System.IO.FileInfo f = new FileInfo(String.Format(@"{0}\{2}_{1}.xml", ctx.Directory, DateTime.Now.ToString("yyyyMMddHHmm"), fileName));
                using (StreamWriter sw = f.AppendText())
                {
                    try
                    {
                        sw.WriteLine("<items>");
                        foreach (string item in items)
                        {
                            string result = client.itemData(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie, item, qty, detailed, termsKey);
                            sw.WriteLine(DataConverter.FormatAsXML(result,true));
                        }
                        sw.WriteLine("</items>");
                        sb.AppendLine("Uspješno!");

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (sw != null) {
                            sw.Flush();
                            sw.Close();
                        }
                    }
                }

            }
            else sb.AppendLine("Dohvaćanje i spremanje nije moguće");
        }


        /// <summary>
        /// Async method that starts multiple tasks and asynchronusly writes them to file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="items"></param>
        /// <param name="qty"></param>
        /// <param name="termsKey"></param>
        /// <param name="detailed"></param>
        /// <returns></returns>
        private async Task<string> getItemDetailsAsync(string fileName, List<string> items, int qty = 0, string termsKey = "1", bool detailed = true)
        {
            MOLSoapClient client = getClient();
            StringBuilder sb = new StringBuilder();
            if (ctx.IsSavingPossible && !String.IsNullOrEmpty(fileName) && items != null && items.Count > 0)
            {
                System.IO.FileInfo f = new FileInfo(String.Format(@"{0}\{2}_{1}.xml", ctx.Directory, DateTime.Now.ToString("yyyyMMddHHmm"), fileName));
                using (StreamWriter sw = f.AppendText())
                {
                    sw.WriteLine("<items>");
                    var itemDataTasks = new List<Task<itemDataResponse>>();
                    foreach (string item in items) itemDataTasks.Add(client.itemDataAsync(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie, item, qty, detailed, termsKey));

                    sb.AppendLine("Dohvaća se " + itemDataTasks.Count + " artikala");
                    while (itemDataTasks.Count > 0)
                    {
                        Task<itemDataResponse> finishedTask = await Task.WhenAny(itemDataTasks);
                        string result = finishedTask.Result.Body.itemDataResult;
                        sw.WriteLine(DataConverter.FormatAsXML(result, true));
                        itemDataTasks.Remove(finishedTask);
                    }

                    sb.AppendLine("Uspjšeno!");
                }
            }
            else sb.AppendLine("Ne dohvaća se ništa je spremanje nije omogućeno");
            return sb.ToString();
        }

        /// <summary>
        /// Get filtered item lst
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="tradeMarkKey"></param>
        /// <returns></returns>
        

        #endregion

    }
}
