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
using System.Collections.ObjectModel;

namespace BizBook
{
   
    public partial class PurchaseReportView : Window
    {
        Entities.Purchase _purchasae;
        ObservableCollection<Reporting.PurchasePrintDetailsModel> _purchaseDetails;
        string totalproducts;
        string totalquantity;
        public PurchaseReportView(Entities.Purchase purchase, ObservableCollection<Reporting.PurchasePrintDetailsModel> purchaseDetails, string Tquantity, string TotalProducts)
        {
            InitializeComponent();
            _purchasae = purchase;
            _purchaseDetails = purchaseDetails;
            totalproducts = Tquantity;
            totalquantity = TotalProducts;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string date = System.DateTime.Now.ToString();
                //string ownerName = Properties.Configuration.Default.Owner_Name;
                //string contact = Properties.Configuration.Default.Telephone;
                //string address = Properties.Configuration.Default.Address;
                //string company = Properties.Configuration.Default.Company;
                CrystalDecisions.CrystalReports.Engine.ReportDocument report1 = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                report1.Load(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Reporting\PurchaseReport.rpt"));
                report1.SetDataSource(_purchaseDetails);
                report1.SetParameterValue("pCustomer", _purchasae.suppliers.Name);
                report1.SetParameterValue("pCustomerContact", _purchasae.suppliers.Contact);
                report1.SetParameterValue("pCustomerAddress", _purchasae.suppliers.Address);
                report1.SetParameterValue("pOrderTotalAmount", _purchasae.TotalPrice);
                report1.SetParameterValue("pFreight", _purchasae.Freight);
                report1.SetParameterValue("pOrderDate", _purchasae.PurchaseDate);
                report1.SetParameterValue("pCash", _purchasae.Cash);
                report1.SetParameterValue("pPurchaseAmount", _purchasae.PurchaseAmount);

                report1.SetParameterValue("pPrintDate", date);
              
                
                report1.SetParameterValue("pOrderId", _purchasae.GatePass);
                crystalReportsViewer1.ViewerCore.ReportSource = report1;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there is no data generate report.");
                infobox.ShowDialog();
            }

        }
    }
}
