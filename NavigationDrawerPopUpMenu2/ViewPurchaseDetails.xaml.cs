using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using BizBook.Entities;
using BizBook.Models;

namespace BizBook
{
   
    public partial class ViewPurchaseDetails : Window
    {
        Entities.Purchase _purchase = new Entities.Purchase();
        private readonly PurchaseDatabaseHelper purchaseDatabaseHelper = new PurchaseDatabaseHelper();
        private readonly PaymentsDatabaseHelper paymentsDatabaseHelper = new PaymentsDatabaseHelper();
        public List<Entities.Purchase_Details> Purchase_DetailsList = new List<Purchase_Details>();
        public ObservableCollection<SaleDetailsViewModel> PurchaseDetails = new ObservableCollection<SaleDetailsViewModel>();
        public bool closecheck = false;

        public ViewPurchaseDetails(Entities.Purchase purchase)
        {
            InitializeComponent();
            _purchase = purchase;
            LblTitle.Content = _purchase.suppliers.Name;
            LblContact.Content = _purchase.suppliers.Contact;
            LblAddress.Content = _purchase.suppliers.Address;
            LblEntryDate.Content = _purchase.EntryDate;
            LblDate.Content = _purchase.PurchaseDate;
            DatePicker.DisplayDate = _purchase.PurchaseDate;
            LblTotalAmount.Text = _purchase.TotalPrice.ToString();
            LblDisAmount.Text = _purchase.Freight.ToString();
            lblCash.Text = _purchase.Cash.ToString();
            lblGatePass.Text =  _purchase.GatePass;
            LblPurchaseAmount.Text = _purchase.PurchaseAmount.ToString();
            string id = _purchase.PId;
            Purchase_DetailsList = purchaseDatabaseHelper.GetSinglePurchaseDetails(id);
            PurchaseListView.ItemsSource = Purchase_DetailsList;
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

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ObservableCollection<Reporting.PurchasePrintDetailsModel> purchasePrint = new ObservableCollection<Reporting.PurchasePrintDetailsModel>();
                foreach (var purchaselist_ in Purchase_DetailsList)
                {
                    Reporting.PurchasePrintDetailsModel model = new Reporting.PurchasePrintDetailsModel()
                    {
                        ProductName = purchaselist_.rawmaterial.Name,
                        Quantity = purchaselist_.Quantity,
                        Total = purchaselist_.Total.ToString(),
                        Price = purchaselist_.Rate.ToString(),
                        ProductId = purchaselist_.RawId
                    };
                    purchasePrint.Add(model);
                }
                PurchaseReportView purchaseReportView = new PurchaseReportView(_purchase, purchasePrint, _purchase.TotalProducts.ToString(), _purchase.TotalQuantity.ToString());
                purchaseReportView.ShowDialog();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("ERROR:There is no data to generate report.");
                infobox.ShowDialog();
            }
        }

        private void btnDeleteDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var model = PurchaseListView.SelectedItem as Entities.Purchase_Details;
                purchaseDatabaseHelper.DeletePurchaseDetails(model);
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("ERROR:There is no selected raw material to delete.");
                infobox.ShowDialog();
            }
        }

        private void btnEditDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.Purchase_Details purchase_Details = PurchaseListView.SelectedItem as Entities.Purchase_Details;
                EditRawMaterial editRawMaterial = new EditRawMaterial(purchase_Details);
                editRawMaterial.ShowDialog();
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("ERROR:There is no selected raw material to show for editing...");
                infobox.ShowDialog();
            }
        }

        private void BtnUpdatePurchase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.PurchaseDatabaseHelper helper = new PurchaseDatabaseHelper();
                var _list =  helper.GetPurchaseDetails(_purchase.PId);
                float amount = _list.Sum(s=> s.Total);
                _purchase.Freight = Convert.ToInt32(LblDisAmount.Text);
                _purchase.PurchaseAmount = amount;
                _purchase.TotalPrice =  amount + float.Parse(LblDisAmount.Text);
                _purchase.Cash = Convert.ToInt32(lblCash.Text);
                purchaseDatabaseHelper.EditPurchaseAmounts(_purchase);
                paymentsDatabaseHelper.EditPurchaseAmountPayment(_purchase.PId, amount);
                closecheck = true;
                InfoBox infobox = new InfoBox("Purchase info updated.");
                infobox.ShowDialog();

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("ERROR:No fields should be empty.");
                infobox.ShowDialog();
            }
        }

        private void btnUpdateDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.PurchaseDatabaseHelper helper = new PurchaseDatabaseHelper();
               
                _purchase.PurchaseDate = DatePicker.SelectedDate.Value.Date;
                purchaseDatabaseHelper.EditPurchaseDate(_purchase);
                paymentsDatabaseHelper.EditPurchasePaymentDate(_purchase);
                closecheck = true;
                InfoBox infobox = new InfoBox("Purchase Date updated.");
                infobox.ShowDialog();

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something Went Wrong.");
                infobox.ShowDialog();
            }
        }

        private void btnUpdateGatePass_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.PurchaseDatabaseHelper helper = new PurchaseDatabaseHelper();

                _purchase.GatePass = lblGatePass.Text;
                purchaseDatabaseHelper.EditGatePass(_purchase);
               
                InfoBox infobox = new InfoBox("Gate Pass updated.");
                infobox.ShowDialog();

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something Went Wrong.");
                infobox.ShowDialog();
            }
        }
    }
}
