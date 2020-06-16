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
    /// Interaction logic for MOLWSClientHome.xaml
    /// </summary>
    public partial class MOLWSClientHome : Page
    {
        private IContext ctx;
        public MOLWSClientHome()
        {
            InitializeComponent();
            ctx = new WSContext(Properties.Settings.Default.AspKey, Properties.Settings.Default.CustomerKey, Properties.Settings.Default.Password, Properties.Settings.Default.ServiceEndpointAddress,
                Properties.Settings.Default.StorageDirectory, Properties.Settings.Default.Cookie);
            if (!ctx.IsMandatoryDataSet)
            {
                MessageBox.Show("Prvo unesite postavke da biste mogli nastaviti");  
            }
        }

        private void NavigateToSettings(object sender, RoutedEventArgs e)
        {
            ClientSettings settings = new ClientSettings();
            NavigationService.Navigate(settings);
        }

        private void NavigateToItemDetails(object sender, RoutedEventArgs e)
        {
            ItemDetails itemDetails = new ItemDetails();
            NavigationService.Navigate(itemDetails);
        }

        private void NavigateToPostSO(object sender, RoutedEventArgs e)
        {
            CreateSO createSO = new CreateSO();
            NavigationService.Navigate(createSO);
        }

        private void NavigateToAdministration(object sender, RoutedEventArgs e)
        {
            Administration admin = new Administration();
            NavigationService.Navigate(admin);
        }
    }
}
