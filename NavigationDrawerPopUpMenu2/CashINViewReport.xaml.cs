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
    
    public partial class CashINViewReport : Window
    {
        ObservableCollection<CashInOutModel> cashInModels = new ObservableCollection<CashInOutModel>();
       // Entities.Customers _customer;
        string Company;
        string OwnerName;
        string Contact;
        string Address;

        public CashINViewReport(Entities.Customers customers, ObservableCollection<CashInOutModel> cashinmodel, float totalcashin)
        {
            InitializeComponent();
        }
    }
}
