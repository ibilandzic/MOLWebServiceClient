using Microline.WS.Connector.Service;
using Microline.WS.Connector.Service.Binding;
using Microline.WS.Core;
using Microline.WS.Core.Context;
using Microline.WS.Core.Convert;
using Microline.WS.Core.Exceptions;
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
            if (String.IsNullOrEmpty(ctx.ServiceURl)) throw new StringNotSetException("Url is not set");
            if (!ctx.IsMandatoryDataSet) throw new StringNotSetException("Context is missing mandatory data.");
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

        /// <summary>
        /// Get single item data
        /// </summary>
        /// <param name="itemKey"></param>
        /// <param name="fileName"></param>
        /// <param name="termsKey"></param>
        /// <param name="qty"></param>
        /// <param name="detailed"></param>
        /// <returns></returns>
        public async Task<string> GetItemDetails(string itemKey, string fileName, string termsKey = "1", int qty = 0, bool detailed = true)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(itemKey))
            {
                if (String.IsNullOrEmpty(fileName)) fileName = itemKey;
                var result = await getItemDetailsAsync(fileName, new List<string>() { itemKey }, qty, termsKey, detailed);
                sb.AppendLine(result);
            }
            else sb.AppendLine("Šifra artikla je obavezan podatak");

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


        /// <summary>
        /// Async post so, argument is so
        /// </summary>
        /// <param name="so"></param>
        /// <returns></returns>

        public async Task<string> PostSOAsync(Microline.WS.XMLModel.SO so)
        {
            if (so == null) throw new InvalidDataException("SO must be instanciated object");
            else
            {
                var response = await PostSOAsync(so.SerializeToStringNoDeclaration());
                return response;
            }
        }
        /// <summary>
        /// Post SO
        /// </summary>
        /// <param name="soAsXML"></param>
        /// <returns></returns>
        public async Task<string> PostSOAsync(string soAsXML)
        {
            if (String.IsNullOrEmpty(soAsXML)) throw new InvalidDataException("XML data must be a non null non empty string");
            try
            {
                StringBuilder sb = new StringBuilder();
                MOLSoapClient client = getClient();
                var result = await client.addSOAsync(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie, soAsXML);
                if (result != null && !String.IsNullOrEmpty(result.Body.addSOResult))
                {
                    string modifiedResult = String.Format("<root>{0}</root>", result.Body.addSOResult);
                    XmlDocument doc = Microline.WS.XMLModel.Util.CreateXmlDocumentFromString(modifiedResult);
                    if (doc.SelectSingleNode("root/result") != null) sb.AppendLine("Rezultat: " + doc.SelectSingleNode("root/result").InnerText);
                    if (doc.SelectSingleNode("root/documentNumber") != null) sb.AppendLine("Broj narudžbe: " + doc.SelectSingleNode("root/documentNumber").InnerText);
                    if (doc.SelectSingleNode("root/soPK") != null) sb.AppendLine("PK: " + doc.SelectSingleNode("root/soPK").InnerText);
                }
                else sb.Append("Došlo je do greške, nema odgovora");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Gets data for so including lines
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <param name="extraData"></param>
        /// <returns></returns>
        public async Task<string> GetSODataAsync(string orderNumber, string fileName, bool extraData)
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(orderNumber))
            {
                try
                {
                    int? pk = soPK(orderNumber);

                    if (pk.HasValue)
                    {
                        var result =await getSODataAsync(fileName, pk.Value, extraData);
                        sb.AppendLine(result);
                    }
                    else sb.AppendLine(String.Format("Za NK {0} nije pronađen primarni ključ", orderNumber));
                    
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            else sb.Append("Nedostaje broj narudžbe kupca");

            return sb.ToString();
        }

        /// <summary>
        /// Delivers so by order nmber
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public async Task<string> DeliverSOAsync(string orderNumber)
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(orderNumber))
            {
                try
                {
                    int? pk = soPK(orderNumber);

                    if (pk.HasValue)
                    {
                        MOLSoapClient client = getClient();
                        var result = await client.deliverSOAsync(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie, pk.Value);

                        if (result != null && result.Body != null && result.Body.deliverSOResult != null)
                        {
                            if (result.Body.deliverSOResult.Contains("<result>"))
                            {
                                XmlDocument doc = Microline.WS.XMLModel.Util.CreateXmlDocumentFromString(result.Body.deliverSOResult);
                                XmlNode el = doc.GetElementsByTagName("result")[0];
                                sb.AppendLine(el.InnerText);
                            }
                            else sb.AppendLine("Nedostaje rezultat");
                        }
                    }
                    else sb.AppendLine(String.Format("Za NK {0} nije pronađen primarni ključ", orderNumber));

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else sb.Append("Nedostaje broj narudžbe kupca");

            return sb.ToString();
        }


        /// <summary>
        /// Deletes so async
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public async Task<string> DeleteSOAsync(string orderNumber)
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(orderNumber))
            {
                try
                {
                    int? pk = soPK(orderNumber);

                    if (pk.HasValue)
                    {
                        MOLSoapClient client = getClient();
                        var result = await client.deleteSOAsync(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie, pk.Value);

                        if (result != null && result.Body != null && result.Body.deleteSOResult != null)
                        {
                            if (result.Body.deleteSOResult.Contains("<result>"))
                            {
                                XmlDocument doc = Microline.WS.XMLModel.Util.CreateXmlDocumentFromString(result.Body.deleteSOResult);
                                XmlNode el = doc.GetElementsByTagName("result")[0];
                                sb.AppendLine(el.InnerText);
                            }
                            else sb.AppendLine("Nedostaje rezultat");
                        }
                    }
                    else sb.AppendLine(String.Format("Za NK {0} nije pronađen primarni ključ", orderNumber));

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else sb.Append("Nedostaje broj narudžbe kupca");

            return sb.ToString();
        }


        public async Task<string> GetSIDataAsync(string invoiceNumber, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(invoiceNumber))
            {
                try
                {
                    string sysdocid  = invoiceSysdocid(invoiceNumber);

                    if (!String.IsNullOrEmpty(sysdocid))
                    {
                        var result = await getSalesInvoiceAsync(fileName, sysdocid);
                        sb.AppendLine(result);
                    }
                    else sb.AppendLine(String.Format("Za račun {0} nije pronađen primarni ključ", invoiceNumber));

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else sb.Append("Nedostaje broj narudžbe kupca");

            return sb.ToString();
        }

        /// <summary>
        /// Gets customer statement async
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<string> CustomerStatementAsync(string fileName)
        {
            MOLSoapClient client = getClient();
            StringBuilder sb = new StringBuilder();

            if (ctx.IsSavingPossible && !String.IsNullOrEmpty(fileName))
            {
                System.IO.FileInfo f = new FileInfo(String.Format(@"{0}\{2}_{1}.xml", ctx.Directory, DateTime.Now.ToString("yyyyMMddHHmm"), fileName));
                using (StreamWriter sw = f.AppendText())
                {
                    var result = await client.customerStatementAsync(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie);
                    if (result != null && result.Body != null && result.Body.customerStatementResult != null)
                    {
                        if (result.Body.customerStatementResult.Contains("<result>"))
                        {
                            XmlDocument doc = Microline.WS.XMLModel.Util.CreateXmlDocumentFromString(result.Body.customerStatementResult);
                            XmlNode el = doc.GetElementsByTagName("result")[0];
                            sb.AppendLine(el.InnerText);
                        }
                        else
                        {
                            sw.WriteLine(DataConverter.FormatAsXML(result.Body.customerStatementResult, true));
                            sb.AppendLine("Uspješno");
                        }
                    }
                }
            }
            else sb.AppendLine("Greška! Mogući uzroci: Spremanje nije moguće, nedostaje ime datoteke za spremanje ili neodstaje sistemski broj računa");

            return sb.ToString();
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
            try
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
                            if (finishedTask != null && finishedTask.Result != null && finishedTask.Result.Body != null && finishedTask.Result.Body.itemDataResult != null)
                            {
                                string result = finishedTask.Result.Body.itemDataResult;
                                sw.WriteLine(DataConverter.FormatAsXML(result, true));
                            }
                            itemDataTasks.Remove(finishedTask);
                        }

                        sb.AppendLine("Uspješno!");
                    }
                }
                else sb.AppendLine("Ne dohvaća se ništa je spremanje nije omogućeno");
                return sb.ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private int? soPK(string orderNumber)
        {
            int? pk = null;
            if (!String.IsNullOrEmpty(orderNumber))
            {
                try
                {
                    MOLSoapClient client = getClient();
                    string result =  client.soPK(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie, orderNumber);
                    if (!String.IsNullOrEmpty(result))
                    {
                        int? intTry = DataConverter.ToInt32(result, DataConverter.Action.ReturnNull, -1);
                        if (intTry.HasValue && intTry.Value != -1) pk = intTry;
                    }
  
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return pk;
        }

        /// <summary>
        /// Gets so data async
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="pk"></param>
        /// <param name="extraData"></param>
        /// <returns></returns>
        private async Task<string> getSODataAsync(string fileName, int pk, bool extraData)
        {

            MOLSoapClient client = getClient();
            StringBuilder sb = new StringBuilder();

            if (ctx.IsSavingPossible && !String.IsNullOrEmpty(fileName))
            {
                System.IO.FileInfo f = new FileInfo(String.Format(@"{0}\{2}_{1}.xml", ctx.Directory, DateTime.Now.ToString("yyyyMMddHHmm"), fileName));
                using (StreamWriter sw = f.AppendText())
                {
                    var result = await client.soDataAsync(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie, pk, extraData);
                    if (result != null && result.Body != null && result.Body.soDataResult != null)
                    {
                        if (result.Body.soDataResult.Contains("<result>"))
                        {
                            XmlDocument doc = Microline.WS.XMLModel.Util.CreateXmlDocumentFromString(result.Body.soDataResult);
                            XmlNode el = doc.GetElementsByTagName("result")[0];
                            sb.AppendLine(el.InnerText);
                        }
                        else
                        {
                            sw.WriteLine(DataConverter.FormatAsXML(result.Body.soDataResult, true));
                            sb.AppendLine("Uspješno");
                        }
                    }
                }
            }
            else sb.AppendLine("Spremanje nije moguće ili nedostaje ime datoteke za spremanje");

            return sb.ToString();

        }


        /// <summary>
        /// Returns sysdocid for invoice number
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <returns></returns>
        private string invoiceSysdocid(string invoiceNumber)
        {
            string sysdocid = null;
            if (!String.IsNullOrEmpty(invoiceNumber))
            {
                MOLSoapClient client = getClient();
                string result = client.salesInvoiceSysdocidForNumber(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie, invoiceNumber);
                if (!String.IsNullOrEmpty(result))
                {
                    XmlDocument doc = Microline.WS.XMLModel.Util.CreateXmlDocumentFromString(result);
                    XmlNode el = doc.GetElementsByTagName("sysdocid")[0];
                    sysdocid = el.InnerText;
                }
            }

            return sysdocid;
        }

        /// <summary>
        /// Fetches invoice data, async
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sysdocidDelimited"></param>
        /// <returns></returns>
        private async Task<string> getSalesInvoiceAsync(string fileName, string sysdocidDelimited)
        {
            MOLSoapClient client = getClient();
            StringBuilder sb = new StringBuilder();

            if (ctx.IsSavingPossible && !String.IsNullOrEmpty(fileName) && !String.IsNullOrEmpty(sysdocidDelimited))
            {
                System.IO.FileInfo f = new FileInfo(String.Format(@"{0}\{2}_{1}.xml", ctx.Directory, DateTime.Now.ToString("yyyyMMddHHmm"), fileName));
                using (StreamWriter sw = f.AppendText())
                {
                    var result = await client.salesInvoicesAsync(ctx.AspKey, ctx.CustomerKey, ctx.Password, ctx.Cookie, sysdocidDelimited);
                    if (result != null && result.Body != null && result.Body.salesInvoicesResult != null)
                    {
                        if (result.Body.salesInvoicesResult.Contains("<result>"))
                        {
                            XmlDocument doc = Microline.WS.XMLModel.Util.CreateXmlDocumentFromString(result.Body.salesInvoicesResult);
                            XmlNode el = doc.GetElementsByTagName("result")[0];
                            sb.AppendLine(el.InnerText);
                        }
                        else
                        {
                            sw.WriteLine(DataConverter.FormatAsXML(result.Body.salesInvoicesResult, true));
                            sb.AppendLine("Uspješno");
                        }
                    }
                }
            }
            else sb.AppendLine("Greška! Mogući uzroci: Spremanje nije moguće, nedostaje ime datoteke za spremanje ili neodstaje sistemski broj računa");

            return sb.ToString();
        }

        
        
        
        #endregion

    }
}
