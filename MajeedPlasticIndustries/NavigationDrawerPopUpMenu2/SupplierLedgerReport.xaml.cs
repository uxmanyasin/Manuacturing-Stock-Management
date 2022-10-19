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
using CrystalDecisions.CrystalReports.Engine;
using System.Collections.ObjectModel;
using Path =System.IO.Path;

namespace BizBook
{
    
    public partial class SupplierLedgerReport : Window
    {
        ObservableCollection<Reporting.SupplierLedgerModel> _supplierLedger;
      
        Entities.Suppliers _suppliers;
        string _Tpurchases;
        string _TCashIn;
        string _TBalance;
        string _PreviousBalance;
        string _StartDate;
        string _EndDate;
       
        public SupplierLedgerReport(Entities.Suppliers suppliers, ObservableCollection<Reporting.SupplierLedgerModel> supplierLedger, string TPurchase, string TCashIn, string TBalance , string PreviousBalance , string StartDate, string EndDate)
        {
            InitializeComponent();
            _supplierLedger = supplierLedger;
            _suppliers = suppliers;
            _Tpurchases = TPurchase;
            _TCashIn = TCashIn;
            _TBalance = TBalance.ToString();
            _PreviousBalance = PreviousBalance;
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
                report1.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Reporting\SupplierLedger.rpt"));
                report1.SetDataSource(_supplierLedger);
                report1.SetParameterValue("pCustomer", _suppliers.Name);
                report1.SetParameterValue("pCustomerContact", _suppliers.Contact);
                report1.SetParameterValue("pCustomerAddress", _suppliers.Address);
                report1.SetParameterValue("pPrintDate", date);
                report1.SetParameterValue("?pStartDate", _StartDate);
                report1.SetParameterValue("?pEndDate", _EndDate);

                report1.SetParameterValue("pTotalPurchase", _Tpurchases);
                report1.SetParameterValue("pTotalCashOut", _TCashIn);
                report1.SetParameterValue("pTotalBalance", _TBalance);
                report1.SetParameterValue("pPreviousBalance", _PreviousBalance);
                crystalReportsViewer1.ViewerCore.ReportSource = report1;
            }
            catch (Exception ex)
            {
                InfoBox infobox = new InfoBox(ex.Message/*"ERROR:There is no data to generate report."*/);
                infobox.ShowDialog();
            }
        }
    }
}
