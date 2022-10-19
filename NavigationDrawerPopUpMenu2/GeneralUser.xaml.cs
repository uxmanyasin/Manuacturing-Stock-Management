using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;


namespace BizBook
{
    
    public partial class GeneralUser : Window
    {
        public GeneralUser()
        {
            InitializeComponent();
            LBLDisplayName.Text = Properties.Configuration.Default.Username.TrimEnd();
            //LBLCompanyName.Text = Properties.Configuration.Default.Company.TrimEnd();

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            CreateBackup();
            Login l = new Login();
            l.Show();
            this.Close();
        }

        private void btnHide_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.UserControl usc = null;
            MainDock.Children.Clear();

            switch (((System.Windows.Controls.ListViewItem)((System.Windows.Controls.ListView)sender).SelectedItem).Name)
            {

                case "ControlSale":
                    usc = new Sale();
                    MainDock.Children.Add(usc);
                    break;
                case "ControlPurchase":
                    usc = new Purchase();
                    MainDock.Children.Add(usc);
                    break;
                case "ControlCategories":
                    usc = new Categories();
                    MainDock.Children.Add(usc);
                    break;
                case "ControlProducts":
                    usc = new Products();
                    MainDock.Children.Add(usc);
                    break;
                case "ControlRawMaterial":
                    usc = new RawMaterial();
                    MainDock.Children.Add(usc);
                    break;
                case "ControlRawStock":
                    usc = new RawStock();
                    MainDock.Children.Add(usc);
                    break;
                case "ControlStock":
                    usc = new Inventory();
                    MainDock.Children.Add(usc);
                    break;
               
                case "ControlCustomerClaims":
                    usc = new CustomerClaims();
                    MainDock.Children.Add(usc);
                    break;
                case "ControlSupplierClaims":
                    usc = new SupplierClaims();
                    MainDock.Children.Add(usc);
                    break;
                case "ControlCashOut":
                    New_Cash_OUT new_Cash_OUT = new New_Cash_OUT();
                    new_Cash_OUT.ShowDialog();
                    break;
                case "ControlCashIn":
                    New_CashINAdvance new_CashINAdvance = new New_CashINAdvance();
                    new_CashINAdvance.ShowDialog();
                    break;
                default:
                    break;
            }

        }

        private void log_out_Click(object sender, RoutedEventArgs e)
        {
            CreateBackup();
            Login l = new Login();
            l.Show();
            this.Close();
        }

        private void Backup_Click(object sender, RoutedEventArgs e)
        {
            CreateBackup();
        }

        private void account_settings_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void BankPayments_Click(object sender, RoutedEventArgs e)
        {
            MainDock.Children.Clear();
            System.Windows.Controls.UserControl usc = new BankCheques();
            MainDock.Children.Add(usc);
        }

        private void CreateBackup()
        {
            try
            {
                DateTime d = DateTime.Now;
                string dd = d.Day + "-" + d.Month + "-" + d.Year;
                string servername = "DESKTOP-Q61CFGE";
                string dbName = "MajeedPlasticIndustry";
                string path = @"Data Source=" + servername + ";Integrated Security=True;Initial Catalog=" + dbName + "";
                SqlConnection con = new SqlConnection(path);
                con.Open();
                string str = "USE " + dbName + ";";
                string str1 = "BACKUP DATABASE " + dbName + " TO DISK = 'E:\\Database\\" + dbName + "-" + dd + ".Bak' WITH FORMAT,MEDIANAME = 'Z_SQLServerBackups',NAME = 'Full Backup of " + dbName + "';";
                SqlCommand cmd1 = new SqlCommand(str, con);
                SqlCommand cmd2 = new SqlCommand(str1, con);
                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();

            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,unable to create backup...");
                infobox.ShowDialog();
                this.Close();
            }
        }

        private void SaleRate_settings_Click(object sender, RoutedEventArgs e)
        {
            RateAdjustment rateAdjustment = new RateAdjustment();
            rateAdjustment.ShowDialog();
        }

        private void PurchaseRate_settings_Click(object sender, RoutedEventArgs e)
        {
            PurchaseRateAdjustment purchaseRate = new PurchaseRateAdjustment();
            purchaseRate.ShowDialog();
        }
    }
}
