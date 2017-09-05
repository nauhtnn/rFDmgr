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
using Microsoft.Win32;

namespace Clnt
{
    /// <summary>
    /// Interaction logic for Checker.xaml
    /// </summary>
    public partial class Checker : Page
    {
        public Checker()
        {
            InitializeComponent();
        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            txtStt.Text += "Checking date format...\n";
            RegistryKey rkey = Registry.CurrentUser.OpenSubKey(@"Control Panel\International", true);
            string sd = rkey.GetValue("sShortDate", "x") as string;
            if (sd == "x")
                txtStt.Text += "\tcan't check\n";
            else if (sd == "dd/MM/yyyy")
                txtStt.Text += "\tOK\n";
            else
            {
                txtStt.Text += "\tCurrent format is " + sd + ", try to fix it...\n";
                try
                {
                    rkey.SetValue("sShortDate", "dd/MM/yyyy");
                    txtStt.Text += "\tOK\n";
                }
                catch (UnauthorizedAccessException)
                {
                    txtStt.Text += "\tException";
                }
                catch (System.Security.SecurityException)
                {
                    txtStt.Text += "\tException";
                }
                //rkey.SetValue("sLongDate", "dd/MM/yy");
            }
        }
    }
}
