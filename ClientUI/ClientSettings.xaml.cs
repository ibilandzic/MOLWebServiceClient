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
                    ExceptionHandler.Log(ex);
                    MessageBox.Show("Došlo je do greške");
                }
            }
       
            
        }
    }
}
