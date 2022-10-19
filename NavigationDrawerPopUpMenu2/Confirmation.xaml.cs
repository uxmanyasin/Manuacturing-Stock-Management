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

namespace BizBook
{
   
    public partial class Confirmation : Window
    {
        public int _id;
        public Models.PaymentsDatabaseHelper helper = new Models.PaymentsDatabaseHelper();
        public Confirmation(int id)
        {
            InitializeComponent();
            _id = id;
             
        }
        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var model = helper.GetPayment(_id);
                helper.DeletePayment(model);
                InfoBox infobox = new InfoBox("Payment Deleted Successfully.");
                infobox.ShowDialog();
                this.Close();
            }
            catch (Exception)
            {
               
                InfoBox infobox = new InfoBox("Something went wrong.please try again.");
                infobox.ShowDialog();
                this.Close();
            }
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
