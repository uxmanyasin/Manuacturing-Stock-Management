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
using BizBook.Entities;
using BizBook.Models;

namespace BizBook
{
    
    public partial class DuesAlert : Window
    {
        private readonly CustomerDatabaseHelper helper = new CustomerDatabaseHelper();
        int custid;
        public DuesAlert(int id,string name)
        {
            InitializeComponent();
            custid = id;
            lblmessage.Content = name+"'s"+" "+" Dues exceeded from his credit limit.";
        }

       
        private void btnSkip_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.Customers customers = helper.GetCustomer(custid);
                customers.CreditStatus = 1;
                helper.EditCustomer(customers);
                this.Close();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,Please try again.");
                infobox.ShowDialog();
            }
        }
    }
}
