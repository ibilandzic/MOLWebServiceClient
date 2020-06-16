
using Microline.WS.Connector.Service.Client;
using Microline.WS.Core;
using Microline.WS.Core.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Microline.WS.Client.UI
{
    /// <summary>
    /// Interaction logic for ItemDetails.xaml
    /// </summary>
    public partial class ItemDetails : Page
    {
        private WSClient client;
        IContext ctx;
        public ItemDetails()
        {//string aspKey, string customerKey, string password, string serviceUrl, string directory, string cookie
            InitializeComponent();
            ctx = new WSContext(Properties.Settings.Default.AspKey, Properties.Settings.Default.CustomerKey, Properties.Settings.Default.Password, Properties.Settings.Default.ServiceEndpointAddress,
                Properties.Settings.Default.StorageDirectory, Properties.Settings.Default.Cookie);
            client = new WSClient(ctx);
            getARTerms();
            getTrademarkList();
            getItemTypes();
           

        }

        private async Task getARTerms()
        {
            var result = await client.GetARTermsAsync();
            ARTermsOptions.ItemsSource = result;
            ARTermsOptions.SelectedValuePath = "Key";
            ARTermsOptions.DisplayMemberPath = "Value";
        }

        private async Task getTrademarkList()
        {
            var result = await client.GetTrademarkListAsync();
            result.Add("", null);
            TrademarkList.ItemsSource = result;
            TrademarkList.SelectedValuePath = "Key";
            TrademarkList.DisplayMemberPath = "Value";


        }

        private async Task getItemTypes()
        {
            var result = await client.GetItemTypesAsync();
            result.Add("", null);
            ItemTypeList.ItemsSource = result;
            ItemTypeList.SelectedValuePath = "Key";
            ItemTypeList.DisplayMemberPath = "Value";
        }

        private void GetItems(object sender, RoutedEventArgs e)
        {
            getItemsAsync();
        }

        private void GoHome(object sender, RoutedEventArgs e)
        {
            MOLWSClientHome home = new MOLWSClientHome();
            NavigationService.Navigate(home);
        }

        private async Task getItemsAsync()
        {
            string arTerm = ARTermsOptions.SelectedValue != null ? ARTermsOptions.SelectedValue.ToString() : "1";
            string itemType = ItemTypeList.SelectedValue != null ? ItemTypeList.SelectedValue.ToString() : null;
            string trademarkKey = TrademarkList.SelectedValue!=null ? TrademarkList.SelectedValue.ToString() : null;
            string countTry = ItemQuatity.Text;
            string itemKey = ItemKey.Text;
            bool extraInfo = ExtraInfo.IsChecked ?? false;

            int qty = 0;
            if ( String.IsNullOrEmpty(countTry) || !int.TryParse(countTry, out qty)) qty = 0;

            DateTime start = DateTime.Now;
            DateTime end;
            try
            {
                string msg;

                if (!String.IsNullOrEmpty(itemKey)) //single item fetch
                {
                    var result = await client.GetItemDetails(itemKey, itemKey, arTerm, 0, extraInfo);
                    msg = result;
                }
                else //fetch multiple items
                {
                    if (!String.IsNullOrEmpty(itemType) || !String.IsNullOrEmpty(trademarkKey))
                    {
                        var result = await client.GetItemsFilteredAsync(itemType, trademarkKey, "itemFiltered", arTerm, qty, extraInfo);
                        msg = result;
                    }
                    else
                    {
                        var result = await client.GetAllItemsAsync("allItems", qty, arTerm, extraInfo);
                        msg = result;
                    }
                }

                end = DateTime.Now;
                MessageBox.Show(String.Format("Vrijeme izvođenja: {2} minuta i {1} sekundi, {0}", msg, (end - start).Seconds, (end-start).Minutes));
            }
            catch (Exception ex)
            {
                ExceptionHandler.Log(ex, ctx.Directory);
                end = DateTime.Now;
                MessageBox.Show(String.Format("Došlo je do greške. Vrijeme izvođenja: {0}", (end - start).TotalSeconds));
            }
        }

    }
}
