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
            DtChk();
            OfficeChk();
        }

        private void DtChk()
        {
            txtStt.Text += "Checking date format...\n";
            RegistryKey rkey = Registry.CurrentUser.OpenSubKey(@"Control Panel\International");
            string sd = rkey.GetValue("sShortDate", "x") as string;
            if (sd == "x")
                txtStt.Text += "\tCan't check\n";
            else if (sd == "dd/MM/yyyy")
                txtStt.Text += "\tOK\n";
            else
            {
                txtStt.Text += "\tCurrent format is " + sd + ", try to change it...\n";
                rkey.Close();
                try
                {
                    rkey = Registry.CurrentUser.OpenSubKey(@"Control Panel\International", true);
                    rkey.SetValue("sShortDate", "dd/MM/yyyy");
                    txtStt.Text += "\tOK\n";
                }
                catch (UnauthorizedAccessException)
                {
                    txtStt.Text += "\tCan't be changed\n";
                }
                catch (System.Security.SecurityException)
                {
                    txtStt.Text += "\tCan't be changed\n";
                }
            }
        }

        private void OfficeChk()
        {
            txtStt.Text += "Checking Office...\n";
            string[] vVer = {"7.0","8.0","9.0","10.0","11.0","12.0","14.0","15.0","16.0"};
            string[] vName = { "Office 97", "Office 98", "Office 2000", "Office XP", "Office 2003",
                "Office 2007", "Office 2010", "Office 2013", "Office 2016"};
            for (int i = 0; i < vVer.Count(); ++i)
            {
                if (WordChk(vVer[i]) || ExcelChk(vVer[i]) || AccessChk(vVer[i]) || PowerPntChk(vVer[i]))
                    txtStt.Text += "\tFound " + vName[i] + '\n';
            }
        }

        private bool WordChk(string ver)
        {
            string k = @"SOFTWARE\Microsoft\Office\" + ver + @"\Word\InstallRoot";
            RegistryKey rkey = Registry.LocalMachine.OpenSubKey(k);
            if (rkey == null)
                return false;
            string path = rkey.GetValue("Path", "x") as string;
            if(path == "x")
                return false;
            if(System.IO.File.Exists(path + "WINWORD.EXE"))
                return true;
            return false;
        }

        private bool ExcelChk(string ver)
        {
            string k = @"SOFTWARE\Microsoft\Office\" + ver + @"\Excel\InstallRoot";
            RegistryKey rkey = Registry.LocalMachine.OpenSubKey(k);
            if (rkey == null)
                return false;
            string path = rkey.GetValue("Path", "x") as string;
            if (path == "x")
                return false;
            if (System.IO.File.Exists(path + "EXCEL.EXE"))
                return true;
            return false;
        }

        private bool PowerPntChk(string ver)
        {
            string k = @"SOFTWARE\Microsoft\Office\" + ver + @"\PowerPoint\InstallRoot";
            RegistryKey rkey = Registry.LocalMachine.OpenSubKey(k);
            if (rkey == null)
                return false;
            string path = rkey.GetValue("Path", "x") as string;
            if (path == "x")
                return false;
            if (System.IO.File.Exists(path + "POWERPNT.EXE"))
                return true;
            return false;
        }

        private bool AccessChk(string ver)
        {
            string k = @"SOFTWARE\Microsoft\Office\" + ver + @"\Access\InstallRoot";
            RegistryKey rkey = Registry.LocalMachine.OpenSubKey(k);
            if (rkey == null)
                return false;
            string path = rkey.GetValue("Path", "x") as string;
            if (path == "x")
                return false;
            if (System.IO.File.Exists(path + "MSACCESS.EXE"))
                return true;
            return false;
        }
    }
}
