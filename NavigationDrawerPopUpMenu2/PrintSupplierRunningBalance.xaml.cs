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
   
    public partial class PrintSupplierRunningBalance : Window
    {
        List<Reporting.CustomersRunningBalance> _RunningBalanceList;
        string _Balance = "";
        public PrintSupplierRunningBalance(List<Reporting.CustomersRunningBalance> RunningBalanceList,string running_Balance)
        {
            InitializeComponent();
            _RunningBalanceList = RunningBalanceList;
            _Balance = running_Balance;
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string date = System.DateTime.Now.ToString();
                ReportDocument report1 = new ReportDocument();
                report1.Load(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Reporting\CashOutReport.rpt"));
                report1.SetDataSource(_RunningBalanceList);
                report1.SetParameterValue("pPrintDate", date);
                report1.SetParameterValue("pPrintBalance", _Balance);

                crystalReportsViewer1.ViewerCore.ReportSource = report1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                InfoBox infobox = new InfoBox("Something went wrong,there are no Data to Print.");
                infobox.ShowDialog();
            }
        }
    }
}
