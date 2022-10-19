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
using Path = System.IO.Path;

namespace BizBook
{
   
    public partial class PrintCustomerRunningBalance : Window
    {
        List<Reporting.CustomersRunningBalance> _RunningBalanceList;
        string _balance;
       
        public PrintCustomerRunningBalance(List<Reporting.CustomersRunningBalance> RunningBalanceList,string running_balance)
        {
            InitializeComponent();
            _RunningBalanceList = RunningBalanceList;
            _balance = running_balance;
        
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string date = System.DateTime.Now.ToString();
                ReportDocument report1 = new ReportDocument();
                report1.Load(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Reporting\CashInReport.rpt"));
                report1.SetDataSource(_RunningBalanceList);
                report1.SetParameterValue("pPrintDate", date);
                report1.SetParameterValue("pOwnerName", _balance);
              
                crystalReportsViewer1.ViewerCore.ReportSource = report1;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no Data to Print.");
                infobox.ShowDialog();
            }
        }
    }
}
