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
    
    public partial class ViewCustomerClaimDetails : Window
    {
        Entities.CustomerClaims _Claims = new Entities.CustomerClaims();
        public List<Entities.CustomerClaimDetails> details = new List<Entities.CustomerClaimDetails>();
        private readonly Models.CustomerClaimsDatabaseHelper helper =  new Models.CustomerClaimsDatabaseHelper();
        private readonly Models.PaymentsDatabaseHelper paymentsDatabaseHelper = new Models.PaymentsDatabaseHelper();
        public bool closecheck = false;
        public ViewCustomerClaimDetails(Entities.CustomerClaims Claim)
        {
            InitializeComponent();
            _Claims = Claim;
            LblTitle.Content = _Claims.customers.Name;
            LblContact.Content = _Claims.customers.Contact;
            LblAddress.Content = _Claims.customers.Address;
            LblDate.Content = _Claims.ClaimDate.ToString();
            LblTotalAmount.Content = _Claims.TotalPrice.ToString();
            details = helper.GetClaimDetails(_Claims.CId);
            lblGatePass.Text =  _Claims.GatePass;
            ClaimListView.ItemsSource = details;

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (closecheck == true)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Before Close window,you have to click on 'Update Amount' Button.");
            }
        }

        private void btnEditDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var detail = ClaimListView.SelectedItem as Entities.CustomerClaimDetails;
                EditProduct editProduct = new EditProduct(detail,true);
                editProduct.ShowDialog();
            }
            catch (Exception ex)
            {

                InfoBox infobox = new InfoBox(ex.ToString());
                infobox.ShowDialog();
            }
        }

        private void btnDeleteDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var detail = ClaimListView.SelectedItem as Entities.CustomerClaimDetails;
                helper.DeleteSingleDetail(detail);
                InfoBox infobox = new InfoBox("Return Product deleted..");
                infobox.ShowDialog();
            }
            catch (Exception ex)
            {

                InfoBox infobox = new InfoBox(ex.ToString());
                infobox.ShowDialog();
            }
        }

        private void BtnUpdatePurchase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              
                Models.CustomerClaimsDatabaseHelper helper = new Models.CustomerClaimsDatabaseHelper();
                var _list = helper.GetClaimDetails(_Claims.CId);
                float amount = _list.Sum(s=> s.Total);
                _Claims.TotalPrice = amount;
                helper.EditReturnAmount(_Claims);
                paymentsDatabaseHelper.EditCustomerClaimPayment(_Claims.CId, amount);
                closecheck = true;
                InfoBox infobox = new InfoBox("Return Amount Updated.");
                infobox.ShowDialog();
            }
            catch (Exception ex)
            {

                InfoBox infobox = new InfoBox(ex.ToString());
                infobox.ShowDialog();
            }
        }

        private void btnUpdateDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.CustomerClaimsDatabaseHelper helper = new Models.CustomerClaimsDatabaseHelper();
                _Claims.ClaimDate = DatePicker.SelectedDate.Value.Date;
                helper.EditClaimDate(_Claims);
                paymentsDatabaseHelper.EditCustomerClaimDate(_Claims);
                closecheck = true;
                InfoBox infobox = new InfoBox("Return Date Updated.");
                infobox.ShowDialog();
            }
            catch (Exception ex)
            {

                InfoBox infobox = new InfoBox(ex.ToString());
                infobox.ShowDialog();
            }
        }

        private void btnUpdateGatePass_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.CustomerClaimsDatabaseHelper helper = new Models.CustomerClaimsDatabaseHelper();
                _Claims.GatePass = lblGatePass.Text;
                helper.EditGatePass(_Claims);
              
                InfoBox infobox = new InfoBox("Gate Pass Updated.");
                infobox.ShowDialog();
            }
            catch (Exception ex)
            {

                InfoBox infobox = new InfoBox(ex.ToString());
                infobox.ShowDialog();
            }
        }
    }
}
