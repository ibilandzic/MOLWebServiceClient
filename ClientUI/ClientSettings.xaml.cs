using Microline.WS.Core;
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
    /// Interaction logic for ClientSettings.xaml
    /// </summary>
    public partial class ClientSettings : Page
    {
        public ClientSettings()
        {
            InitializeComponent();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(this.AspKeyInput.Text) && !String.IsNullOrEmpty(CustmerKeyInput.Text) && !String.IsNullOrEmpty(CustomerPasswordInput.Text)
                && !String.IsNullOrEmpty(EndpointInput.Text) && !string.IsNullOrEmpty(DirectoryInput.Text))
            {
                try
                {
                    Properties.Settings.Default.Save();
                    MessageBox.Show("Postavke su uspješno spremljene");
                }
                catch(Exception ex)
                {
                    ExceptionHandler.Log(ex, DirectoryInput.Text);
                    MessageBox.Show("Došlo je do greške");
                }
            }
            else
            {
                if (String.IsNullOrEmpty(AspKeyInput.Text)) AspKeyInput.Background = Brushes.LightSalmon;
                if (String.IsNullOrEmpty(CustmerKeyInput.Text)) CustmerKeyInput.Background = Brushes.LightSalmon;
                if (String.IsNullOrEmpty(CustomerPasswordInput.Text)) CustomerPasswordInput.Background = Brushes.LightSalmon;
                if (String.IsNullOrEmpty(EndpointInput.Text)) EndpointInput.Background = Brushes.LightSalmon;
                if (String.IsNullOrEmpty(DirectoryInput.Text)) DirectoryInput.Background = Brushes.LightSalmon;

            }


        }

        private void CheckValue(object sender, TextChangedEventArgs e)
        {
            bool checkValue = false;
            TextBox senderField = sender as TextBox;
            if (String.IsNullOrEmpty(senderField.Text))
            {
                senderField.Background = Brushes.LightSalmon;
                checkValue = true;
            }
            else senderField.Background = Brushes.White;

            if (checkValue) MessageBox.Show("Provjerite vrijednosti u poljima");
        }

        private void GoHome(object sender, RoutedEventArgs e)
        {
            MOLWSClientHome home = new MOLWSClientHome();
            NavigationService.Navigate(home);
        }
    }
}
