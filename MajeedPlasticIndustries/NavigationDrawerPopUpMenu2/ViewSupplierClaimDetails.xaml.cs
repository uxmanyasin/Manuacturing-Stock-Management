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
   
    public partial class ViewSupplierClaimDetails : Window
    {
        Entities.SupplierClaims _Claims = new Entities.SupplierClaims();
        public List<Entities.SupplierClaimDetails> details = new List<Entities.SupplierClaimDetails>();
        private readonly Models.SupplierClaimsDatabaseHelper helper = new Models.SupplierClaimsDatabaseHelper();
        private readonly Models.SupplierClaimsDatabaseHelper supplierClaimsDatabaseHelper = new Models.SupplierClaimsDatabaseHelper();
        private readonly Models.PaymentsDatabaseHelper paymentsDatabaseHelper = new Models.PaymentsDatabaseHelper();
        public bool CloseCheck = false;
        public ViewSupplierClaimDetails(Entities.SupplierClaims claims)
        {
            InitializeComponent();
            _Claims = claims;
            LblTitle.Content = _Claims.Suppliers.Name;
            LblContact.Content = _Claims.Suppliers.Contact;
            LblDate.Content = _Claims.ClaimDate.ToString();
            LblAddress.Content = _Claims.Suppliers.Address;
            LblTotalAmount.Content = _Claims.TotalPrice.ToString();
            details = helper.GetClaimDetails(_Claims.CId);
            lblGatePass.Text =  _Claims.GatePass;
            ClaimListView.ItemsSource = details;
            ClaimListView.Items.Refresh();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (CloseCheck == true)
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
                var claim = ClaimListView.SelectedItem as Entities.SupplierClaimDetails;
                EditRawMaterial editRawMaterial = new EditRawMaterial(claim,true);
                editRawMaterial.ShowDialog();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnDeleteDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var claim = ClaimListView.SelectedItem as Entities.SupplierClaimDetails;
                supplierClaimsDatabaseHelper.DeleteSingleDetail(claim);
                InfoBox infobox = new InfoBox("Raw Material deleted.");
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
             
                Models.SupplierClaimsDatabaseHelper helper = new Models.SupplierClaimsDatabaseHelper();
                var _list = helper.GetClaimDetails(_Claims.CId);
                float amount = _list.Sum(s=> s.Total);
                _Claims.TotalPrice = amount;
                helper.EditReturnAmount(_Claims);
                paymentsDatabaseHelper.EditSupplierClaimPayment(_Claims.CId, amount);
                CloseCheck = true;
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

                Models.SupplierClaimsDatabaseHelper helper = new Models.SupplierClaimsDatabaseHelper();

                _Claims.ClaimDate = DatePicker.SelectedDate.Value.Date;
                helper.EditClaimDate(_Claims);
                paymentsDatabaseHelper.EditSupplierClaimDate(_Claims);
                CloseCheck = true;
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

                Models.SupplierClaimsDatabaseHelper helper = new Models.SupplierClaimsDatabaseHelper();

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
