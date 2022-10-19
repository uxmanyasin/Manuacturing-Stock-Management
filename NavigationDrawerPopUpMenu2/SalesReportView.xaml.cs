using BizBook.Entities;
using CrystalDecisions.CrystalReports.Engine;

using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace BizBook
{
    
    public partial class SalesReportView : Window
    {
        Entities.Sale _sale;
        ObservableCollection<Reporting.SalePrintDetailsModel>  _saleDetails;
        //string totalproducts;
        //string totalquantity;
        public SalesReportView(Entities.Sale sale, ObservableCollection<Reporting.SalePrintDetailsModel>  saleDetails)
        {
            InitializeComponent();
            _sale = sale;
            _saleDetails = saleDetails;
          
            try
            {
                string date = System.DateTime.Now.ToString();
                //string ownerName = Properties.Configuration.Default.Owner_Name;
                //string contact = Properties.Configuration.Default.Telephone;
                //string address = Properties.Configuration.Default.Address;
                //string company = Properties.Configuration.Default.Company;
                ReportDocument report1 = new ReportDocument();
                report1.Load(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Reporting\SalesReport.rpt"));
                report1.SetDataSource(_saleDetails);
                report1.SetParameterValue("pCustomer", _sale.customers.Name);
                report1.SetParameterValue("pCustomerContact", _sale.customers.Contact);
                report1.SetParameterValue("pCustomerAddress", _sale.customers.Address);
                report1.SetParameterValue("pOrderTotalAmount", _sale.TotalPrice);
                report1.SetParameterValue("pOrderDate", _sale.SaleDate);
                report1.SetParameterValue("pOrderId", _sale.GatePass);
                report1.SetParameterValue("pPrintDate", date);
               
              //  report1.SetParameterValue("pTotalQuantity", totalquantity);
               // report1.SetParameterValue("pTotalProducts", totalproducts);
             
                crystalReportsViewer1.ViewerCore.ReportSource = report1;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there is no data to generate report.");
                infobox.ShowDialog();
            }
        }

        public SalesReportView(Entities.Sale sale, ObservableCollection<Reporting.SalePrintDetailsModel> saleDetails,bool indicator)
        {
            InitializeComponent();
            _sale = sale;
            _saleDetails = saleDetails;
           
            try
            {
                string date = System.DateTime.Now.ToString();
                //string ownerName = Properties.Configuration.Default.Owner_Name;
                //string contact = Properties.Configuration.Default.Telephone;
                //string address = Properties.Configuration.Default.Address;
                //string company = Properties.Configuration.Default.Company;
                ReportDocument report1 = new ReportDocument();
                report1.Load(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Reporting\SalesReport.rpt"));
                report1.SetDataSource(_saleDetails);
                report1.SetParameterValue("pCustomer", _sale.CustomerName);
                report1.SetParameterValue("pCustomerContact", "NA");
                report1.SetParameterValue("pCustomerAddress", "NA");
                report1.SetParameterValue("pOrderTotalAmount", _sale.TotalPrice);
              
                report1.SetParameterValue("pOrderDate", _sale.SaleDate);
              
                report1.SetParameterValue("pPrintDate", date);
           
                //report1.SetParameterValue("pTotalQuantity", totalquantity);
                //report1.SetParameterValue("pTotalProducts", totalproducts);
                crystalReportsViewer1.ViewerCore.ReportSource = report1;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there is no data to generate report.");
                infobox.ShowDialog();
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
