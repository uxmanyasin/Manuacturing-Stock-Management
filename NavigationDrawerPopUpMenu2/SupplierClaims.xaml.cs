using BizBook.Entities;
using BizBook.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BizBook
{
   
    public partial class SupplierClaims : UserControl
    {
        private readonly SupplierDatabaseHelper supplierDatabaseHelper = new SupplierDatabaseHelper();
        private readonly RawMaterialDatabaseHelper rawMaterialDatabaseHelper = new RawMaterialDatabaseHelper();
        private readonly SupplierClaimsDatabaseHelper supplierClaimsDatabaseHelper = new SupplierClaimsDatabaseHelper();
        public List<Entities.Suppliers> suppliersList = new List<Suppliers>();
        public List<Entities.RawMaterial> rawMaterialList = new List<Entities.RawMaterial>();
        public List<Entities.SupplierClaims> returnList = new List<Entities.SupplierClaims>();
        public ObservableCollection<SaleDetailsViewModel> SaleDetailsList = new ObservableCollection<SaleDetailsViewModel>();

        public SupplierClaims()
        {
            InitializeComponent();
            bool Access = Properties.Configuration.Default.AccessMode;
            if (Access == true)
            {
                PurchaseReport.IsEnabled = false;
            }
        }

        private void SupplierCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                suppliersList.Clear();
                suppliersList = supplierDatabaseHelper.GetSuppliers().ToList();
                suppliersList.Sort((x, y) => string.Compare(x.Name, y.Name));
                SupplierCombo.ItemsSource = suppliersList;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no suppliers to show.");
                infobox.ShowDialog();
            }
        }

        private void RawCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                rawMaterialList.Clear();
                rawMaterialList = rawMaterialDatabaseHelper.GetRawMaterial().ToList();
                rawMaterialList.Sort((x, y) => string.Compare(x.Name, y.Name));
                RawCombo.ItemsSource = rawMaterialList;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no products to show.");
                infobox.ShowDialog();
            }
        }

        private void btnToList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.RawMaterial p = RawCombo.SelectedItem as Entities.RawMaterial;
                var pp = SaleDetailsList.Where(s => s.ProductId == p.Id).FirstOrDefault();
                if (pp == null)
                {
                    SaleDetailsViewModel model = new SaleDetailsViewModel()
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        Quantity = float.Parse(TxtQty.Text),
                        Total = (float.Parse(TxtQty.Text) * float.Parse(txtPrice.Text)),
                        Price = float.Parse(txtPrice.Text)
                    };
                    SaleDetailsList.Add(model);
                }
                else
                {
                    pp.Quantity = pp.Quantity + Convert.ToInt32(TxtQty.Text);
                    pp.Total = pp.Total + (float.Parse(TxtQty.Text) * float.Parse(txtPrice.Text));
                }

               
                ProductCartGrid.ItemsSource = SaleDetailsList;
                ProductCartGrid.Items.Refresh();
            }
            catch (Exception ex)
            {

                InfoBox infobox = new InfoBox("Error:Please select product and enter quantity to add it your list.");
                infobox.ShowDialog();
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaleDetailsViewModel model = ProductCartGrid.SelectedItem as SaleDetailsViewModel;
                SaleDetailsList.Remove(model);
                txtPrice.Text = "0";

            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("There is no selected raw material to remove.");
                infobox.ShowDialog();
            }
        }

        private void ReturnDone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
             
                float totalAmount = SaleDetailsList.Sum(s => s.Total);
                LblTAmount.Content = totalAmount.ToString();
             

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Error:There is no product in product list.Minimum 1 product is required.");
                infobox.ShowDialog();
            }
        }

        private void btnSaveReturn_Click(object sender, RoutedEventArgs e)
        {
            SaveReturn();
        }

        private void SaveReturn() 
        {
            try
            {
                int id = Convert.ToInt32(SupplierCombo.SelectedValue.ToString());
                string guid = System.Guid.NewGuid().ToString();
                guid = guid.Substring(0, 5);
                float amount = float.Parse(LblTAmount.Content.ToString());

                Entities.SupplierClaims Claim = new Entities.SupplierClaims()
                {
                    CId = guid,
                    ClaimDate = PurchaseDatePicker.SelectedDate.Value,
                    SupplierId = id,
                    TotalPrice = amount,
                    GatePass = txtGatePass.Text
                };
                supplierClaimsDatabaseHelper.CreateClaim(Claim,SaleDetailsList);
                InfoBox infobox = new InfoBox("New Return has been added successfully.");
                infobox.ShowDialog();
                SaleDetailsList.Clear();
                RawCombo.SelectedItem = null;
                txtPrice.Text = "";
                TxtQty.Text = "";
                LblTAmount.Content = "";
                ProductCartGrid.ItemsSource = null;
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("ERROR:All Feilds are required.");
                infobox.ShowDialog();
            }
        }

        private void btnDeleteReturn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Return = ReturnListView.SelectedItem as Entities.SupplierClaims;
                supplierClaimsDatabaseHelper.DeleteReturn(Return);
                InfoBox infobox = new InfoBox("Selected Claim Deleted..");
                infobox.ShowDialog();
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there is no selected claim to delete.");
                infobox.ShowDialog();
            }
        }

        private void btnGridDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Claim = ReturnListView.SelectedItem as Entities.SupplierClaims;
                ViewSupplierClaimDetails claimDetails = new ViewSupplierClaimDetails(Claim);
                claimDetails.ShowDialog();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there is no selected claim to show its details.");
                infobox.ShowDialog();
            }
        }

        private void BtnLoadAll_Click(object sender, RoutedEventArgs e)
        {
            AllClaims();
        }

        private void SupplierComboSearch_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                suppliersList.Clear();
                suppliersList = supplierDatabaseHelper.GetSuppliers().ToList();
                suppliersList.Sort((x, y) => string.Compare(x.Name, y.Name));
                SupplierComboSearch.ItemsSource = suppliersList;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no suppliers to show.");
                infobox.ShowDialog();
            }
        }

        private void SearchSupplierReturns_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void SearchByDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime date = Datepicker.SelectedDate.Value.Date;

                returnList.Clear();
                returnList = supplierClaimsDatabaseHelper.GetDatedSupplierClaims(date);
                returnList.Sort((x, y) => x.ClaimDate.CompareTo(y.ClaimDate));

                ReturnListView.ItemsSource = returnList;
                ReturnListView.Items.Refresh();
                LblTotalReturns.Content = returnList.Count.ToString();
                LbltotalAmount.Content = returnList.Sum(s => s.TotalPrice);
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no claims to show.");
                infobox.ShowDialog();
            }
        }

        private void SearchDateBetween_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dateFrom = datepickerFrom.SelectedDate.Value.Date;
                DateTime dateTo = datepickerTo.SelectedDate.Value.Date;

                returnList.Clear();
                returnList = supplierClaimsDatabaseHelper.GetBetweenDatesReturn(dateFrom,dateTo);
                returnList.Sort((x, y) => x.ClaimDate.CompareTo(y.ClaimDate));

                ReturnListView.ItemsSource = returnList;
                ReturnListView.Items.Refresh();
                LblTotalReturns.Content = returnList.Count.ToString();
                LbltotalAmount.Content = returnList.Sum(s => s.TotalPrice);
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no claims to show.");
                infobox.ShowDialog();
            }
        }

        private void AllClaims() 
        {
            try
            {
                returnList.Clear();
                returnList = supplierClaimsDatabaseHelper.GetClaims();
                returnList.Sort((x, y) => x.ClaimDate.CompareTo(y.ClaimDate));

                ReturnListView.ItemsSource = returnList;
                ReturnListView.Items.Refresh();
                LblTotalReturns.Content = returnList.Count.ToString();
                LbltotalAmount.Content = returnList.Sum(s=> s.TotalPrice);
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no claims to show.");
                infobox.ShowDialog();
            }
        }

        private void PurchaseReport_Initialized(object sender, EventArgs e)
        {
            AllClaims();
        }

        private void btnSearchByID_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                returnList.Clear();
                string id = txtClaimID.Text;
                returnList = supplierClaimsDatabaseHelper.ClaimsByID(id);
                returnList.Sort((x, y) => x.ClaimDate.CompareTo(y.ClaimDate));

                ReturnListView.ItemsSource = returnList;
                ReturnListView.Items.Refresh();
                LblTotalReturns.Content = returnList.Count.ToString();
                LbltotalAmount.Content = returnList.Sum(s => s.TotalPrice);
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no claims to show.");
                infobox.ShowDialog();
            }
        }

        private void SupplierComboSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var supplier = SupplierComboSearch.SelectedItem as Entities.Suppliers;

                returnList.Clear();
                returnList = supplierClaimsDatabaseHelper.GetSpecificSupplierClaims(supplier.Id);
                returnList.Sort((x, y) => x.ClaimDate.CompareTo(y.ClaimDate));

                ReturnListView.ItemsSource = returnList;
                ReturnListView.Items.Refresh();
                LblTotalReturns.Content = returnList.Count.ToString();
                LbltotalAmount.Content = returnList.Sum(s => s.TotalPrice);
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no claims to show.");
                infobox.ShowDialog();
            }
        }
    }
}
