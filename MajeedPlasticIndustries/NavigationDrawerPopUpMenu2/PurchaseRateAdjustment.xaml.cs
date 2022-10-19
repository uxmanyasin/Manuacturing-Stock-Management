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
   
    public partial class PurchaseRateAdjustment : Window
    {
        private readonly Models.RawMaterialDatabaseHelper rawMaterialDatabaseHelper = new Models.RawMaterialDatabaseHelper();
        private readonly Models.SupplierDatabaseHelper supplierDatabaseHelper = new Models.SupplierDatabaseHelper();
        List<Entities.Suppliers> suppliersList = new List<Entities.Suppliers>();
        List<Entities.RawMaterial> rawMaterialsList = new List<Entities.RawMaterial>();
        List<Entities.Purchase_Details> purchaseDetailsList = new List<Entities.Purchase_Details>();
        private readonly Models.PurchaseDatabaseHelper purchaseDatabaseHelper = new Models.PurchaseDatabaseHelper();
        private readonly Models.PaymentsDatabaseHelper paymentsDatabaseHelper = new Models.PaymentsDatabaseHelper();
        public PurchaseRateAdjustment()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ProductCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                rawMaterialsList.Clear();
                rawMaterialsList = rawMaterialDatabaseHelper.GetRawMaterial();
                rawMaterialsList.Sort((x, y) => string.Compare(x.Name, y.Name));

                ProductCombo.ItemsSource = rawMaterialsList;
                ProductCombo.Items.Refresh();

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no raw  materials to show.");
                infobox.ShowDialog();
            }
        }

        private void SupplierCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                suppliersList.Clear();
                suppliersList = supplierDatabaseHelper.GetSuppliers();
                suppliersList.Sort((x, y) => string.Compare(x.Name, y.Name));
                SupplierCombo.ItemsSource = suppliersList;
                SupplierCombo.Items.Refresh();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no suppliers  to show.");
                infobox.ShowDialog();
            }
        }

        private void btnAdjustPricing_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void btnAdjustPricing_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime Fromdate = datePicker.SelectedDate.Value.Date;
                Entities.RawMaterial _product = ProductCombo.SelectedItem as Entities.RawMaterial;
                DateTime ToDate = DateTime.Now.Date;

                Entities.Suppliers _customer = SupplierCombo.SelectedItem as Entities.Suppliers;

                purchaseDetailsList = purchaseDatabaseHelper.GetDatedRawMaterialPurchases(_product.Id, Fromdate, ToDate, _customer.Id);
                foreach (var item in purchaseDetailsList)
                {
                    Entities.Purchase _purchase = purchaseDatabaseHelper.GetPurchase(item.PurchaseId);
                    float xSales = _purchase.TotalPrice; float xSubTotal = item.Total; float Remainder = xSales - xSubTotal;
                    float newSubTotal = item.Quantity * (float.Parse(txtRate.Text));
                    float newTotalPrice = newSubTotal + Remainder;
                    item.Rate = float.Parse(txtRate.Text);
                    item.Total = newSubTotal;
                    _purchase.TotalPrice = newTotalPrice;
                    purchaseDatabaseHelper.EditPurchase(_purchase);
                    purchaseDatabaseHelper.UpdateRates(item);
                    paymentsDatabaseHelper.EditPurchaseAmountPayment(_purchase.PId, _purchase.TotalPrice);

                }
                InfoBox infobox = new InfoBox("Rate of " + _product.Name + " updated succesfully.");
                infobox.ShowDialog();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}
