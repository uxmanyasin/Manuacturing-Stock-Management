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

    public partial class CustomerClaims : UserControl
    {
        private readonly ProductDatabaseHelper productDatabaseHelper = new ProductDatabaseHelper();
        private readonly CustomerDatabaseHelper customerDatabaseHelper = new CustomerDatabaseHelper();
        private readonly CustomerClaimsDatabaseHelper customerClaimsDatabaseHelper = new CustomerClaimsDatabaseHelper();
        private readonly CategoryDatabaseHelper categoryDatabaseHelper = new CategoryDatabaseHelper();
        public List<Entities.Category> categoriesList = new List<Category>();
        public List<Product> productlist = new List<Product>();
        public List<Customers> customerlist = new List<Customers>();
        public List<Entities.CustomerClaims> returnList = new List<Entities.CustomerClaims>();
        public ObservableCollection<SaleDetailsViewModel> SaleDetailsList = new ObservableCollection<SaleDetailsViewModel>();


        public CustomerClaims()
        {
            InitializeComponent();
            bool Access = Properties.Configuration.Default.AccessMode;
            if (Access == true)
            {
                PurchaseReport.IsEnabled = false;
            }
        }
        private void CustomerCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                customerlist.Clear();
                customerlist = customerDatabaseHelper.GetCustomers();
                customerlist.Sort((x, y) => string.Compare(x.Name, y.Name));
                CustomerCombo.ItemsSource = customerlist;
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no customers to show.");
                infobox.ShowDialog();
            }
        }

        private void CategoryCombobox_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                categoriesList.Clear();
                categoriesList = categoryDatabaseHelper.GetCategories();
                categoriesList.Sort((x, y) => string.Compare(x.Name, y.Name));
                CategoryCombobox.ItemsSource = categoriesList;
                CategoryCombobox.Items.Refresh();
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no categories to show.");
                infobox.ShowDialog();
            }
        }

        private void ProductCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {

                if (CategoryCombobox.SelectedItem != null)
                {
                    productlist.Clear();
                    var category = CategoryCombobox.SelectedItem as Entities.Category;
                    productlist = productDatabaseHelper.GetCategoryProducts(category.Id);
                    productlist.Sort((x, y) => string.Compare(x.Name, y.Name));
                    ProductCombo.ItemsSource = productlist;
                }
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
                Product p = ProductCombo.SelectedItem as Product;
                var pp = SaleDetailsList.Where(s => s.ProductId == p.Id).FirstOrDefault();
                if (pp == null)
                {
                    SaleDetailsViewModel model = new SaleDetailsViewModel()
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        Quantity = float.Parse(txtQTY.Text),

                        Total = (float.Parse(txtQTY.Text) * float.Parse(txtPrice.Text)),
                        Price = float.Parse(txtPrice.Text)
                    };
                    SaleDetailsList.Add(model);
                }
                else
                {
                    pp.Quantity = float.Parse(txtQTY.Text);
                    pp.Total = (float.Parse(txtQTY.Text) * float.Parse(txtPrice.Text));
                    pp.Price = float.Parse(txtPrice.Text);
                }


                ProductCartGrid.ItemsSource = SaleDetailsList;
                ProductCartGrid.Items.Refresh();
                txtPrice.Text = "0.0";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                InfoBox infobox = new InfoBox("Please select a product enter quantity to add product to your list.");
                infobox.ShowDialog();
            }
        }

        private void ReturnDone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                float total = 0;
                float TQty = 0;
                foreach (var data in SaleDetailsList)
                {

                    total = (data.Total) + total;
                    TQty = (data.Quantity + TQty);
                    LblTAmount.Content = total.ToString();

                }
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There is no product is your list minimum 1 product is required in your list.");
                infobox.ShowDialog();
            }
        }

        private void btnSaveReturn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveReturn();
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("ERROR:All Feilds are required.");
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
                txtQTY.Text = "0";
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("There is no selected product to remove.");
                infobox.ShowDialog();
            }
        }

        private void SaveReturn()
        {
            try
            {
                int id = Convert.ToInt32(CustomerCombo.SelectedValue.ToString());
                string guid = System.Guid.NewGuid().ToString();
                guid = guid.Substring(0, 5);
                float amount = float.Parse(LblTAmount.Content.ToString());
              
                Entities.CustomerClaims Claim = new Entities.CustomerClaims()
                {
                    CId = guid,
                    ClaimDate = PurchaseDatePicker.SelectedDate.Value,
                    CustomerId = id,
                    TotalPrice = amount,
                    GatePass = txtGatePass.Text
                };
                customerClaimsDatabaseHelper.CreateClaim(Claim, SaleDetailsList);
                InfoBox infobox = new InfoBox("New Return has been added successfully.");
                infobox.ShowDialog();
                SaleDetailsList.Clear();
                CategoryCombobox.SelectedItem = null;
                ProductCombo.SelectedItem = null;
                txtPrice.Text = "";
                txtQTY.Text = "";
                LblTAmount.Content = "";
                ProductCartGrid.ItemsSource = null;
            }
            catch (Exception)
            {
               
                InfoBox infobox = new InfoBox("ERROR:All Feilds are required.");
                infobox.ShowDialog();
            }
        }

        private void btnGridDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Claim = ReturnListView.SelectedItem as Entities.CustomerClaims;
                ViewCustomerClaimDetails claimDetails = new ViewCustomerClaimDetails(Claim);
                claimDetails.ShowDialog();
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there is no selected claim to show its details.");
                infobox.ShowDialog();
            }
        }

        private void btnDeleteReturn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Return = ReturnListView.SelectedItem as Entities.CustomerClaims;
                customerClaimsDatabaseHelper.DeleteReturn(Return);
                InfoBox infobox = new InfoBox("Return Deleted.");
                infobox.ShowDialog();
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong...");
                infobox.ShowDialog();
            }
        }

        private void BtnLoadAll_Click(object sender, RoutedEventArgs e)
        {
            AllClaims();
        }

        private void CustomerComboSearch_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                customerlist.Clear();
                customerlist = customerDatabaseHelper.GetCustomers();
                customerlist.Sort((x, y) => string.Compare(x.Name, y.Name));
                CustomerComboSearch.ItemsSource = customerlist;
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no customers to show.");
                infobox.ShowDialog();
            }
        }

        private void SearchCustomerReturns_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void SearchByDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime date = Datepicker.SelectedDate.Value.Date;
                returnList.Clear();
                returnList = customerClaimsDatabaseHelper.GetDatedCustomerClaims(date);
                returnList.Sort((x, y) => x.ClaimDate.CompareTo(y.ClaimDate));

                ReturnListView.ItemsSource = returnList;
                ReturnListView.Items.Refresh();
                LblTotalReturns.Content = returnList.Count.ToString();
                LbltotalAmount.Content = returnList.Sum(s => s.TotalPrice);
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no returns to show.");
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
                returnList = customerClaimsDatabaseHelper.GetBetweenDatesReturn(dateFrom,dateTo);
                returnList.Sort((x, y) => x.ClaimDate.CompareTo(y.ClaimDate));

                ReturnListView.ItemsSource = returnList;
                ReturnListView.Items.Refresh();
                LblTotalReturns.Content = returnList.Count.ToString();
                LbltotalAmount.Content = returnList.Sum(s => s.TotalPrice);
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no returns to show.");
                infobox.ShowDialog();
            }
        }

        private void AllClaims() 
        {
            try
            {
                returnList.Clear();
                returnList = customerClaimsDatabaseHelper.GetClaims();
                returnList.Sort((x, y) => x.ClaimDate.CompareTo(y.ClaimDate));

                ReturnListView.ItemsSource = returnList;
                ReturnListView.Items.Refresh();
                LblTotalReturns.Content = returnList.Count.ToString();
                LbltotalAmount.Content = returnList.Sum(s => s.TotalPrice);
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no returns to show.");
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
                returnList = customerClaimsDatabaseHelper.ClaimByID(id);
                returnList.Sort((x, y) => x.ClaimDate.CompareTo(y.ClaimDate));

                ReturnListView.ItemsSource = returnList;
                ReturnListView.Items.Refresh();
                LblTotalReturns.Content = returnList.Count.ToString();
                LbltotalAmount.Content = returnList.Sum(s => s.TotalPrice);
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no returns to show.");
                infobox.ShowDialog();
            }
        }

        private void CustomerComboSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var customer = CustomerComboSearch.SelectedItem as Entities.Customers;
                returnList.Clear();
                returnList = customerClaimsDatabaseHelper.GetSpecificCustomerClaims(customer.Id);
                returnList.Sort((x, y) => x.ClaimDate.CompareTo(y.ClaimDate));
                ReturnListView.ItemsSource = returnList;
                ReturnListView.Items.Refresh();
                LblTotalReturns.Content = returnList.Count.ToString();
                LbltotalAmount.Content = returnList.Sum(s => s.TotalPrice);
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no returns to show.");
                infobox.ShowDialog();
            }
        }
    }
}
