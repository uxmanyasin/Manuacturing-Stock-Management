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
using System.Collections.ObjectModel;
using Path = System.IO.Path;
using CrystalDecisions.CrystalReports.Engine;

namespace BizBook
{
    
    public partial class CashOutReportView : Window
    {
        ObservableCollection<CashInOutModel> cashInOutModels;
        Entities.Suppliers _suppliers;
        int totalpay;
        string Company;
        string OwnerName;
        string Contact;
        string Address;
        public CashOutReportView(ObservableCollection<CashInOutModel> _cashoutmodel)
        {
            InitializeComponent();
           // _suppliers = suppliers;
            cashInOutModels = _cashoutmodel;
           // totalpay = totalcashout;
            Company = Properties.Configuration.Default.Company;
            OwnerName = Properties.Configuration.Default.Owner_Name;
            Contact = Properties.Configuration.Default.Telephone;
            Address = Properties.Configuration.Default.Address;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string date = System.DateTime.Now.ToString();
                ReportDocument report1 = new ReportDocument();
                report1.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"\..\Reporting\CashOutReport.rpt"));
                report1.SetDataSource(cashInOutModels);
               // report1.SetParameterValue("pCustomer", _suppliers.Name);
                //report1.SetParameterValue("pCustomerContact", _suppliers.Contact);
                //report1.SetParameterValue("pCustomerAddress", _suppliers.Address);
                report1.SetParameterValue("pPrintDate", date);
                report1.SetParameterValue("pOwnerName", OwnerName);
                report1.SetParameterValue("pOwnerContact", Contact);
                report1.SetParameterValue("pCompanyAddress", Address);
                report1.SetParameterValue("pComapny", Company);
                //report1.SetParameterValue("pTotalPay", totalpay);

                crystalReportsViewer1.ViewerCore.ReportSource = report1;
                report1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
               // InfoBox infobox = new InfoBox(ex.Message/*"ERROR:There is no data to generate report."*/);
                //infobox.ShowDialog();
            }
        }
    }
}
