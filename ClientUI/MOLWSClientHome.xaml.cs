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
        public MOLWSClientHome()
        {
            InitializeComponent();
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
    }
}
