using Microline.WS.Connector.Service.Client;
using Microline.WS.Core;
using Microline.WS.Core.Context;
using Microline.WS.Core.Convert;
using Microline.WS.Core.FixedData;
using Microline.WS.XMLModel;
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
    /// Interaction logic for CreateSO.xaml
    /// </summary>
    public partial class CreateSO : Page
    {
        private WSClient client;
        IContext ctx;
        public List<XMLModel.Item> SoLines { get; set; }

        public CreateSO()
        {
            SoLines = new List<XMLModel.Item>();
            InitializeComponent();
            WindowHeight = 800;
            
            ctx = new WSContext(Properties.Settings.Default.AspKey, Properties.Settings.Default.CustomerKey, Properties.Settings.Default.Password, Properties.Settings.Default.ServiceEndpointAddress,
                Properties.Settings.Default.StorageDirectory, Properties.Settings.Default.Cookie);
            client = new WSClient(ctx);
            getARTerms();
            populateFOBKey();
            populateCities();
            SOLinesInput.ItemsSource = SoLines;
            
        }

        private void populateCities()
        {
            var result = Core.FixedData.City.GetAvailableCities();
            ShipToCityId.ItemsSource = result;
            ShipToCityId.SelectedValuePath = "Key";
            ShipToCityId.DisplayMemberPath = "Value";
        }

        private async Task getARTerms()
        {
            var result = await client.GetARTermsAsync();
            ArTermsList.ItemsSource = result;
            ArTermsList.SelectedValuePath = "Key";
            ArTermsList.DisplayMemberPath = "Value";
        }

        private void populateFOBKey()
        {
            var fobKeys = Core.FixedData.ShipViaFOBKey.GetSimplifiedAvailableList();
            ShipViaFOBKey.ItemsSource = fobKeys;
            ShipViaFOBKey.SelectedValuePath = "Key";
            ShipViaFOBKey.DisplayMemberPath = "Value";
        }

        private void AddNewItemLine(object sender, RoutedEventArgs e)
        {
            try
            {
                SoLines.Add(new XMLModel.Item());
                SOLinesInput.Items.Refresh();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Log(ex, ctx.Directory);
                MessageBox.Show("Greška! " + ex.Message);
            }
        }

        private void GoHome(object sender, RoutedEventArgs e)
        {
            MOLWSClientHome home = new MOLWSClientHome();
            NavigationService.Navigate(home);
        }

        private void SubmitSO(object sender, RoutedEventArgs e)
        {
            try
            {
                SO so = prepareSO();
                if (so != null)
                {
                    postSOAsync(so);
                }
                else MessageBox.Show("NK nije instancirana");
            }
            catch(Exception ex)
            {
                ExceptionHandler.Log(ex, ctx.Directory);
                MessageBox.Show("Došlo je do greške");
            }
        }

        private List<Item> prepareSOLines()
        {
            List<Item> soLines = new List<Item>();

            if (SoLines.Count > 0)
            {
                foreach(var item in SoLines)
                {
                    if(DataConverter.ToInt32(item.Quantity) != 0 && !String.IsNullOrEmpty(item.Key)){
                        soLines.Add(item);
                    }
                }
            }

            return soLines;
        }

        private SO prepareSO()
        {
            int? cityId = null;
            if ((int)ShipToCityId.SelectedValue != 0) cityId = (int)ShipToCityId.SelectedValue;
            SO so = new SO(DeliverImmediatelly.IsChecked ?? false, null, ShipToKey.Text, ShipToName.Text, ShipToAddress1.Text, cityId, ShipToAttention.Text, null,
                (int)ShipViaFOBKey.SelectedValue, (string)ArTermsList.SelectedValue, PayAfterSold.IsChecked ?? false, null);

            List<Item> items = prepareSOLines();

            if (items != null && items.Count > 0)
            {
                so.Items = items;
                return so;
            }
            else throw new InvalidValueException("NK mora imati barem jednu liniju, sve količine moraju biti različite od 1");
        }

        private async Task postSOAsync(SO so)
        {
            if (so == null) throw new InvalidValueException("NK nije instancirana");
            else
            {
                try
                {
                    var response = await client.PostSOAsync(so);
                    MessageBox.Show(response);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Log(ex, ctx.Directory);
                    MessageBox.Show("Došlo je do greške");
                }
            }
        }



    }
}
