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
    /// Interaction logic for Administration.xaml
    /// </summary>
    public partial class Administration : Page
    {
        private WSClient client;
        IContext ctx;
        public Administration()
        {
            InitializeComponent();
            WindowHeight = 600;
            ctx = new WSContext(Properties.Settings.Default.AspKey, Properties.Settings.Default.CustomerKey, Properties.Settings.Default.Password, Properties.Settings.Default.ServiceEndpointAddress,
                Properties.Settings.Default.StorageDirectory, Properties.Settings.Default.Cookie);
            client = new WSClient(ctx);
        }

        private void GetSO(object sender, RoutedEventArgs e)
        {
            getSOAsync();
        }

        private void DeliverSO(object sender, RoutedEventArgs e)
        {
            deliverSOAsync();
        }

        private void DeleteSO(object sender, RoutedEventArgs e)
        {
            deleteSOAsync();
        }

        private void GetSI(object sender, RoutedEventArgs e)
        {
            getSIAsync();
        }


        private void GetStatement(object sender, RoutedEventArgs e)
        {
            getCustomerStatement();
        }


        private void GoHome(object sender, RoutedEventArgs e)
        {
            MOLWSClientHome home = new MOLWSClientHome();
            NavigationService.Navigate(home);
        }

        #region Async method to handle SO
        private async Task getSOAsync()
        {
            string orderNumber = SONumber.Text;
            bool extraData = soExtraData.IsChecked ?? false;
            if (!String.IsNullOrEmpty(orderNumber))
            {
                try
                {
                    var result = await client.GetSODataAsync(orderNumber, String.Format("so{0}", orderNumber), extraData);
                    MessageBox.Show(result);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Log(ex, ctx.Directory);
                    MessageBox.Show("Došlo je do greške");
                }

            }
            else
            {
                MessageBox.Show("Nedostaje broj narudžbe");
            }
        }



        private async Task deliverSOAsync()
        {
            string orderNumber = SONumber.Text;
            if (!String.IsNullOrEmpty(orderNumber))
            {
                try
                {
                    var result = await client.DeliverSOAsync(orderNumber);
                    MessageBox.Show(result);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Log(ex, ctx.Directory);
                    MessageBox.Show("Došlo je do greške");
                }

            }
            else
            {
                MessageBox.Show("Nedostaje broj narudžbe");
            }
        }


        private async Task deleteSOAsync()
        {
            string orderNumber = SONumber.Text;
            if (!String.IsNullOrEmpty(orderNumber))
            {
                try
                {
                    var result = await client.DeleteSOAsync(orderNumber);
                    MessageBox.Show(result);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Log(ex, ctx.Directory);
                    MessageBox.Show("Došlo je do greške");
                }

            }
            else
            {
                MessageBox.Show("Nedostaje broj narudžbe");
            }
        }

        private async Task getSIAsync()
        {
            string invoiceNumber = SINumber.Text;

            if (!String.IsNullOrEmpty(invoiceNumber))
            {
                try
                {
                    var result = await client.GetSIDataAsync(invoiceNumber, String.Format("si{0}", invoiceNumber));
                    MessageBox.Show(result);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Log(ex, ctx.Directory);
                    MessageBox.Show("Došlo je do greške");
                }

            }
            else
            {
                MessageBox.Show("Nedostaje broj narudžbe");
            }

        }

        private async Task getCustomerStatement()
        {
            try
            {
                var result = await client.CustomerStatementAsync("customerStatement");
                MessageBox.Show(result);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Log(ex, ctx.Directory);
                MessageBox.Show("Došlo je do greške");
            }
        }
        #endregion

        
    }
}
