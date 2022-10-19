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
using System.Windows.Shapes;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using System.Collections.ObjectModel;
using Path = System.IO.Path;

namespace BizBook
{
   
    public partial class CustomerLedgerReport : Window
    {
        ObservableCollection<Reporting.CustomerLedgerModel> _supplierLedger;
        Entities.Customers _customers;
        string _Tpurchases;
        string _TCashIn;
        string _TBalance;
        string _PreviousBalacne;
        string _StartDate;
        string _EndDate;
        public CustomerLedgerReport(Entities.Customers customers, ObservableCollection<Reporting.CustomerLedgerModel> supplierLedger, string TPurchase, string TCashIn, string TBalance, string PreviousBalance , string StartDate , string EndDate)
        {
            InitializeComponent();
            _supplierLedger = supplierLedger;
            _customers = customers;
            _Tpurchases = TPurchase;
            _TCashIn = TCashIn;
            _TBalance = TBalance;
            _PreviousBalacne = PreviousBalance;
            _StartDate = StartDate;
            _EndDate = EndDate;
             //Company = Properties.Configuration.Default.Company;
            //OwnerName = Properties.Configuration.Default.Owner_Name;
            //Contact = Properties.Configuration.Default.Telephone;
            //Address = Properties.Configuration.Default.Address;
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string date = System.DateTime.Now.ToString();
                ReportDocument report1 = new ReportDocument();
                report1.Load(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Reporting\CustomerLedger.rpt"));
                report1.SetDataSource(_supplierLedger);
                report1.SetParameterValue("pCustomer", _customers.Name);
                report1.SetParameterValue("pCustomerContact", _customers.Contact);
                report1.SetParameterValue("pCustomerAddress", _customers.Address);
                report1.SetParameterValue("pPrintDate", date);
                report1.SetParameterValue("pPrintDate", date);
                report1.SetParameterValue("pStartDate", _StartDate);
                report1.SetParameterValue("pEndDate", _EndDate);
                report1.SetParameterValue("pTotalPurchase", _Tpurchases);
                report1.SetParameterValue("pTotalCashIn", _TCashIn);
                report1.SetParameterValue("pTotalBalance", _TBalance);
                report1.SetParameterValue("pPreviousBalance", _PreviousBalacne);
                crystalReportsViewer1.ViewerCore.ReportSource = report1;
            }
            catch (Exception ex)
            {
                //InfoBox infobox = new InfoBox("Something went wrong,there are no Data to Print.");
                //infobox.ShowDialog();
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
