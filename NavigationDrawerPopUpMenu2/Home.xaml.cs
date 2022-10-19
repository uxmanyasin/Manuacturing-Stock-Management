using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace BizBook
{
    public partial class Home : Window
    {
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
        public Home()
        {
            InitializeComponent();
            Timer.Tick += new EventHandler(Timer_Click);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }
        private void Timer_Click(object sender, EventArgs e)

        {
            string time = DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm:ss");
            LBLDisplayName.Text = time;
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
                case "ControlVendors":
                    usc = new Supplier();
                    MainDock.Children.Add(usc);
                    break;
                case "ControlCustomers":
                    usc = new Customer();
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
                case "ControlReports":
                    usc = new Reports();
                    MainDock.Children.Add(usc);
                    break;
                case "ControlCashOut":
                    usc = new MultiPaymentsControl(false);
                    MainDock.Children.Add(usc);
                    break;
                case "ControlCashIn":
                    usc = new MultiPaymentsControl(true);
                    MainDock.Children.Add(usc);
                    break;
                default:
                    break;
            }
        }
        private void Log_out_Click(object sender, RoutedEventArgs e)
        {
            CreateBackup();
            Login l = new Login();
            l.Show();
            this.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
         //LBLDisplayName.Text = Properties.Configuration.Default.Username.TrimEnd();
         //LBLCompanyName.Text = Properties.Configuration.Default.Company.TrimEnd();
        }

        private void Account_settings_Click(object sender, RoutedEventArgs e)
        {
            Account_Settings s = new Account_Settings();
            s.ShowDialog();
        }

        private void Configuration_Click(object sender, RoutedEventArgs e)
        {
            Configuration_Settings s = new Configuration_Settings();
            s.ShowDialog();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnHide_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

       

        private void Backup_Click(object sender, RoutedEventArgs e)
        {

            CreateBackup();
        }

        private void btnReminder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.UserControl usc = null;
            MainDock.Children.Clear();
            usc = new ReminderPayments();
            MainDock.Children.Add(usc);
        }

        private void CreateBackup() 
        {
            try
            {
                DateTime d = DateTime.Now;
                string dd = d.Day + "-" + d.Month + "-"+ d.Year;
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

        private void BankPayments_Click(object sender, RoutedEventArgs e)
        {
            MainDock.Children.Clear();
            System.Windows.Controls.UserControl usc = new BankCheques();
            MainDock.Children.Add(usc);
            
        }

        private void RateAdjustments_Click(object sender, RoutedEventArgs e)
        {
            RateAdjustment rateAdjustment = new RateAdjustment();
            rateAdjustment.ShowDialog();

        }

        private void PurchaseRateAdjustments_Click(object sender, RoutedEventArgs e)
        {
            PurchaseRateAdjustment purchaseRate = new PurchaseRateAdjustment();
            purchaseRate.ShowDialog();
        }

        private void btnProtection_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

