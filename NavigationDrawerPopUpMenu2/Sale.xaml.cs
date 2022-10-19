using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;

using BizBook.Entities;
using BizBook.Models;

namespace BizBook
{
    
    public partial class Sale : UserControl
    {
        private readonly ProductDatabaseHelper productDatabaseHelper = new ProductDatabaseHelper();
        private readonly CustomerDatabaseHelper customerDatabaseHelper = new CustomerDatabaseHelper();
        private readonly SaleDatabaseHelper saleDatabaseHelper = new SaleDatabaseHelper();
        private readonly Sale_DetailsDatabaseHelper sale_DetailsDatabaseHelper = new Sale_DetailsDatabaseHelper();
        private readonly CategoryDatabaseHelper categoryDatabaseHelper = new CategoryDatabaseHelper();
        private readonly CustomerRateDatabaseHelper customerRateDatabaseHelper = new CustomerRateDatabaseHelper();
        private readonly InventoryDatabaseHelper inventoryDatabaseHelper = new InventoryDatabaseHelper();
        public List<Entities.Category> categoriesList = new List<Category>();
        public List<Entities.Sale_Details> Sale_DetailsList = new List<Sale_Details>();
        public List<Product> productlist = new List<Product>();
        public List<Customers> customerlist = new List<Customers>();
        public List<Entities.Sale> SalesList = new List<Entities.Sale>();
        public ObservableCollection<SaleDetailsViewModel> SaleDetailsList = new ObservableCollection<SaleDetailsViewModel>();
        private readonly DueAmountDatabaseHelper helper = new DueAmountDatabaseHelper();
        public List<Entities.DueAmount> dueAmountsList = new List<Entities.DueAmount>();


        public Sale()
        {
            InitializeComponent();
            txtcustomername.Focus();
           bool Access = Properties.Configuration.Default.AccessMode;
           if(Access == true) 
           {
                SaleReport.IsEnabled = false;
                WalkingSaleReport.IsEnabled = false;
                ProductsSales.IsEnabled = false;
           }
        }
        private void BtnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            New_Customer n = new New_Customer();
            n.ShowDialog();


        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
           
            New_Product p = new New_Product();
            p.ShowDialog();
        }

      

        private void CustomerCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                customerlist.Clear();
                customerlist = customerDatabaseHelper.GetCustomers().ToList();
                customerlist.Sort((x, y) => string.Compare(x.Name, y.Name));
                CustomerCombo.ItemsSource = customerlist;
                CustomerCombo.Items.Refresh();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no customers to show.");
                infobox.ShowDialog();
            }
        }

        private void ProductCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (CategoryCombobox.SelectedItem != null) {
                    productlist.Clear();
                    var category = CategoryCombobox.SelectedItem as Entities.Category;
                    productlist = productDatabaseHelper.GetCategoryProducts(category.Id);
                    productlist.Sort((x, y) => string.Compare(x.Name, y.Name));
                    CustomerCombo.ItemsSource = customerlist;
                    ProductCombo.ItemsSource = productlist;
                }

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no products to show.");
                infobox.ShowDialog();
            }
        }

       
        private void CustomerCombo_MouseLeave(object sender, MouseEventArgs e)
        {
          
        }

        private void ProductCombo_MouseLeave(object sender, MouseEventArgs e)
        {
            
        }

        private void TextBox_MouseLeave(object sender, MouseEventArgs e)
        {
           
        }
       
        

        private void BtnToList_Click(object sender, RoutedEventArgs e)
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
                if (CustomerCombo.SelectedItem != null) 
                {
                    int id = Convert.ToInt32(CustomerCombo.SelectedValue.ToString());
                    Entities.CustomerRate customerRate = customerRateDatabaseHelper.GetcustomerRate(p.Id, id);
                    float rate = float.Parse(txtPrice.Text);
                    if (rate > 0) 
                    {
                        if (customerRate == null)
                        {
                            Entities.CustomerRate customerRate1 = new CustomerRate
                            {
                                CustomerId = id,
                                ProductId = p.Id,
                                Rate = float.Parse(txtPrice.Text)
                            };
                            customerRateDatabaseHelper.CreateCustomerRate(customerRate1);

                        }
                        else
                        {
                            Entities.CustomerRate customerRate1 = new CustomerRate
                            {
                                CustomerId = id,
                                ProductId = p.Id,
                                Rate = float.Parse(txtPrice.Text)
                            };

                            customerRateDatabaseHelper.EditCustomerRate(customerRate1);
                        }
                    }
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

        private void ProductCartGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //try
            //{
            //    var col = e.Column as DataGridColumn;
            //    if (col != null)
            //    {
            //        var colName = col.Header.ToString();
            //        if (colName == "Quantity")
            //        {
            //            int index = e.Row.GetIndex();
            //            var value = e.EditingElement as TextBox;
            //            //int quantity = (value.Text);
            //            MessageBox.Show(quantity.ToString());
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    InfoBox infobox = new InfoBox("Something went wrong.");
            //    infobox.ShowDialog();
            //}
        }

        private void ProductCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                    int id = Convert.ToInt32(ProductCombo.SelectedValue.ToString());
                    Product product = productDatabaseHelper.GetProduct(id);
                    InventoryDatabaseHelper inventoryDatabaseHelper = new InventoryDatabaseHelper();
                    Entities.Inventory inventory = inventoryDatabaseHelper.GetSpecificInventory(product.Id);
                    txtPrice.Text = product.Rate.ToString();
                  
               
            
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there is no selected product.");
                infobox.ShowDialog();
            }
        }

        private void CustomerCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TxtQTY_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void BtnSaveSale_Click(object sender, RoutedEventArgs e)
        {
            SaveSale();
            ProductCartGrid.ItemsSource = null;
            CategoryCombobox.SelectedItem = null;
            txtInHand.Content = "";
            txtPrice.Text = "";
            txtQTY.Text = "";
            txtGatePass.Text = "";
            LblTotalAmount.Content = "";
            CustomerCombo.ItemsSource = null;
            ProductCombo.SelectedItem = null;
        }

        private void SaveSale() 
        {
            

            if (CustomerCombo.SelectedItem != null && txtcustomername.Text == "")
                try
                {
                    int id = Convert.ToInt32(CustomerCombo.SelectedValue.ToString());
                    Entities.Customers _customer = new Customers();
                    _customer = customerDatabaseHelper.GetCustomer(id);
                    float creditLimit = _customer.CreditLimit;
                    
                    string guid = System.Guid.NewGuid().ToString();
                    guid = guid.Substring(0, 5);
                      
                    float saleamount = float.Parse(LblTotalAmount.Content.ToString());
                    int Total_Quantity = (int)SaleDetailsList.Sum(s => s.Quantity);

                    Entities.Sale sale = new Entities.Sale()
                        {
                            SId = guid,
                            TotalPrice = saleamount,
                            SaleDate = SaleDatePicker.SelectedDate.Value.Date,
                            EntryDate = DateTime.Now.Date,
                            Status = true,
                            CustomerId = id,
                            GatePass = txtGatePass.Text,
                            TotalProducts = SaleDetailsList.Count(),
                            TotalQuantity = Total_Quantity
                    };
                        
                    saleDatabaseHelper.CreateSale(sale, SaleDetailsList);
                       
                    InfoBox infobox = new InfoBox("New Sale created successfully.");
                    infobox.ShowDialog();
                     
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    //InfoBox infobox = new InfoBox("Something went wrong,you missed feild or you dont have enough units in your inventory");
                    //infobox.ShowDialog();
                }
            else if (CustomerCombo.SelectedItem == null && txtcustomername.Text != null)
            {
                try
                {
                    string guid = System.Guid.NewGuid().ToString();
                    guid = guid.Substring(0, 5);
                    int saleamount = Convert.ToInt32(LblTotalAmount.Content.ToString());
                    int Total_Quantity = (int)SaleDetailsList.Sum(s => s.Quantity);

                    Entities.Sale sale = new Entities.Sale()
                    {
                        SId = guid,
                        TotalPrice = saleamount,
                        SaleDate = SaleDatePicker.SelectedDate.Value.Date,
                        EntryDate = DateTime.Now.Date,
                        Status = true,
                        Cash = Convert.ToInt32(txtCash.Text),
                        CustomerName = txtcustomername.Text,
                        GatePass = txtGatePass.Text,
                        TotalProducts = SaleDetailsList.Count(),
                        TotalQuantity = Total_Quantity

                    };

                    Payments payment = new Payments()
                    {

                        EntryDate = DateTime.Now.Date,
                      
                        Credit = Convert.ToInt32(txtCash.Text),
                        Debit = 0,
                        CustomerName = sale.CustomerName,
                        SaleId = sale.SId,
                        Date = sale.SaleDate,
                        Type = "Customer",
                        Description = "Walking Customer"
                    };
                    PaymentsDatabaseHelper paymentsDatabaseHelper = new PaymentsDatabaseHelper();
                    saleDatabaseHelper.CreateSale(sale, SaleDetailsList);
                    paymentsDatabaseHelper.CreatePayment(payment);
                    InfoBox infobox = new InfoBox("New Sale created successfully.");
                    infobox.ShowDialog();
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());

                    //InfoBox infobox = new InfoBox("Something went wrong,you missed feild or you dont have enough units in your inventory");
                    //infobox.ShowDialog();
                }
            }
            else { MessageBox.Show("Both Customer's feild should not fill."); }
        }

        private void BtnLoadAll_Click(object sender, RoutedEventArgs e)
        {
            LoadSales();

        }

        private void SearchCustomerCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void SearchCustomerSales_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void SearchDateSales_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime date = SearchSaleDatePicker.SelectedDate.Value.Date;
                Models.SaleDatabaseHelper saleDatabaseHelper = new SaleDatabaseHelper();
                SalesList = saleDatabaseHelper.GetDatedSales(date);
                SalesList.Sort((x, y) => y.SaleDate.CompareTo(x.SaleDate));
                SalesListView.ItemsSource = SalesList;
                SalesListView.Items.Refresh();
                LblTotalSales.Content = SalesList.Count;
                float totalamount = SalesList.Sum(s => s.TotalPrice);
                LblTAmount.Content = totalamount.ToString();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There is no data to show.");
                infobox.ShowDialog();
            }
        }

        private void OrderDone_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                float TotalAmount = SaleDetailsList.Sum(s => s.Total);
                float Quantity = SaleDetailsList.Sum(s => s.Quantity);
                lblTotalProducts.Content = SaleDetailsList.Count().ToString();
                LblTotalAmount.Content = TotalAmount.ToString();
                lblTotalQuantity.Content = Quantity.ToString();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There is no product is your list minimum 1 product is required in your list.");
                infobox.ShowDialog();
            }
        }

        private void CustomerCombo_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CustomerCombo.SelectedItem != null) 
                {
                    SaleDetailsList.Clear();
                    ProductCartGrid.ItemsSource = null;
                    var customer = CustomerCombo.SelectedItem as Entities.Customers;
                    float runningbalance = customerDatabaseHelper.CheckGetSpecificRunningBalance(customer);
                    if (customer.CreditLimit < runningbalance)
                    {

                        DuesAlert duesAlert = new DuesAlert(customer.Id, customer.Name);
                        duesAlert.ShowDialog();
                    }
                }

            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong.");
                infobox.ShowDialog();
            }
        }

        private void LoadSales()
        {
            try
            {
                Models.SaleDatabaseHelper saleDatabaseHelper = new SaleDatabaseHelper();
                SalesList = saleDatabaseHelper.GetSales();
                SalesList.Sort((x, y) => x.SaleDate.CompareTo(y.SaleDate));

                SalesListView.ItemsSource = SalesList;
                SalesListView.Items.Refresh();
                LblTotalSales.Content = SalesList.Count;
                float totalamount = SalesList.Sum(s => s.TotalPrice);
                LblTAmount.Content = totalamount.ToString("#,#", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There are no sales to show.");
                infobox.ShowDialog();
            }
        }

        private void LoadwalkingSales()
        {
            try
            {
                Models.SaleDatabaseHelper saleDatabaseHelper = new SaleDatabaseHelper();
                SalesList = saleDatabaseHelper.GetwalkingSales();
                SalesList.Sort((x, y) => x.SaleDate.CompareTo(y.SaleDate));

                walkingSalesListView.ItemsSource = SalesList;
                walkingSalesListView.Items.Refresh();
                LblTotalwalkinglSales.Content = SalesList.Count;
                float totalamount = SalesList.Sum(s => s.TotalPrice);
                LblwalkingTAmount.Content = totalamount.ToString("#,#", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There are no sales to show.");
                infobox.ShowDialog();
            }
        }
        private void SaleReport_Initialized(object sender, EventArgs e)
        {
            LoadSales();
        }

        private void BtnGridDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.Sale sale = SalesListView.SelectedItem as Entities.Sale;
                ViewSaleDetails vs = new ViewSaleDetails(sale, true);
                vs.walking = false;
                vs.ShowDialog();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There is no selected sale to show its details.");
                infobox.ShowDialog();
            }


        }

        private void BtnGridDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.Sale sale = SalesListView.SelectedItem as Entities.Sale;
                sale.Status = false;
                saleDatabaseHelper.EditSale(sale);
                LoadSales();
                InfoBox infobox = new InfoBox("Refrence no. " +sale.SId+ " deleted successfully.");
                infobox.ShowDialog();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There is no selected sale to delete.");
                infobox.ShowDialog();
            }

        }

        private void SearchDateBetween_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dateFrom = DateFrom.SelectedDate.Value.Date;
                DateTime dateTo = DateTo.SelectedDate.Value.Date;
                Models.SaleDatabaseHelper saleDatabaseHelper = new SaleDatabaseHelper();
                SalesList = saleDatabaseHelper.SalesBetweenDates(dateFrom, dateTo);
                SalesList.Sort((x, y) => x.SaleDate.CompareTo(y.SaleDate));

                SalesListView.ItemsSource = SalesList;
                SalesListView.Items.Refresh();
                LblTotalSales.Content = SalesList.Count;
                float totalamount = SalesList.Sum(s => s.TotalPrice);
                LblTAmount.Content = totalamount.ToString("#,#", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Date Range is not selected.");
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

        private void txtDiscountedAmount_TextChanged(object sender, TextChangedEventArgs e)
        {

           
        }

        private void WalkingSaleReport_Initialized(object sender, EventArgs e)
        {
            LoadwalkingSales();
        }

        private void SearchDatewalkingSales_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.SaleDatabaseHelper saleDatabaseHelper = new SaleDatabaseHelper();
                DateTime date = SearchwalkingSaleDatePicker.SelectedDate.Value.Date;
                SalesList = saleDatabaseHelper.GetDatedwalkingSales(date);
                SalesList.Sort((x, y) => x.SaleDate.CompareTo(y.SaleDate));

                walkingSalesListView.ItemsSource = SalesList;
                walkingSalesListView.Items.Refresh();
                LblTotalwalkinglSales.Content = SalesList.Count;
                float totalamount = SalesList.Sum(s => s.TotalPrice);
                LblwalkingTAmount.Content = totalamount.ToString("#,#", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There are no sales to show.");
                infobox.ShowDialog();
            }
        }

        private void SearchwalkingDateBetween_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dateFrom = walkingDateFrom.SelectedDate.Value.Date;
                DateTime dateTo = walkingDateTo.SelectedDate.Value.Date;
                Models.SaleDatabaseHelper saleDatabaseHelper = new SaleDatabaseHelper();
                SalesList = saleDatabaseHelper.walkingSalesBetweenDates(dateFrom,dateTo);
                SalesList.Sort((x, y) => x.SaleDate.CompareTo(y.SaleDate));

                walkingSalesListView.ItemsSource = SalesList;
                walkingSalesListView.Items.Refresh();
                LblTotalwalkinglSales.Content = SalesList.Count;
                float totalamount = SalesList.Sum(s => s.TotalPrice);
                LblwalkingTAmount.Content = totalamount.ToString("#,#", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There are no sales to show.");
                infobox.ShowDialog();
            }
        }

        private void SearchwalkingCustomerCombo_MouseEnter(object sender, MouseEventArgs e)
        {
           
           
        }

        private void SearchwalkingCustomerSales_Click(object sender, RoutedEventArgs e)
        {
            
           
        }

        private void btnLoadAllwalking_Click(object sender, RoutedEventArgs e)
        {
            LoadwalkingSales();
        }

        private void btnwalkingGridDelete_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnwalkingGridDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 
                Entities.Sale sale = walkingSalesListView.SelectedItem as Entities.Sale;
                ViewSaleDetails vs = new ViewSaleDetails(sale);
                vs.walking = true;
                vs.ShowDialog();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There is no selected sale to show its details.");
                infobox.ShowDialog();
            }
        }

        private void lblCash_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int totalamount = Convert.ToInt32(LblTotalAmount.Content);
              
              
                //totalamount = totalamount - cash;
                //LblDueAmount.Content = totalamount.ToString();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Cash amount should not empty, atleast zero.");
                infobox.ShowDialog();
            }
         
        }

        private void txtDiscountedAmount_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void ProductsWiseSalesCombo_MouseEnter(object sender, MouseEventArgs e)
        {
           
        }

        public void GetProductSales() 
        {
            try
            {
                Models.SaleDatabaseHelper saleDatabaseHelper = new SaleDatabaseHelper();
                Entities.Product product = ProductSalesCombo.SelectedItem as Entities.Product;
                Sale_DetailsList.Clear();
                Sale_DetailsList = sale_DetailsDatabaseHelper.GetProductSales(product.Id);
                Sale_DetailsList.Sort((x, y) => x.SaleDate.CompareTo(y.SaleDate));
                SalesProductListView.ItemsSource = Sale_DetailsList;
                LblProductTotalSales.Content = Sale_DetailsList.Count().ToString();
                float TotalProductAmount = Sale_DetailsList.Sum(s => s.Total);
                LblProductTAmount.Content = TotalProductAmount.ToString("#,#", CultureInfo.InvariantCulture);
                lblTotalQuantityProduct.Content = Sale_DetailsList.Sum(s => s.Quantity);
                SalesProductListView.Items.Refresh();

            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no products sales to show.");
                infobox.ShowDialog();
            }
        }

        private void btnSearchProductWiseSales_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnLoadAllSalesWiseProducts_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CategorySalesCombo_MouseEnter(object sender, MouseEventArgs e)
        {
           
       

      
        }

      

        private void _SaleDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

      

        private void CategoryCombobox_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                categoriesList.Clear();
                categoriesList = categoryDatabaseHelper.GetCategories();
                categoriesList.Sort((x, y) => string.Compare(x.Name, y.Name));
                CategoryCombobox.ItemsSource = categoriesList;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no categories to show.");
                infobox.ShowDialog();
            }
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            New_Category new_Category = new New_Category();
            new_Category.ShowDialog();
        }

        private void CategoryCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Entities.Category _category = CategoryCombobox.SelectedItem as Entities.Category;
                productlist = productDatabaseHelper.GetCategoryProducts(_category.Id);
                ProductCombo.ItemsSource = productlist;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //InfoBox infobox = new InfoBox("Something went wrong,there are no products to show.");
                //infobox.ShowDialog();
            }
        }

       
        private void btnSearchProductWiseSales_Click_1(object sender, RoutedEventArgs e)
        {
           
        }

        

        private void ProductSalesCombo_MouseEnter(object sender, MouseEventArgs e)
        { 
            try
            {

                productlist = productDatabaseHelper.GetProducts();
                productlist.Sort((x, y) => string.Compare(x.Name, y.Name));
                ProductSalesCombo.ItemsSource = productlist;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no products to show.");
                infobox.ShowDialog();
            }

        }

        private void btnSearchDateBetweenWiseSales_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dateFrom = _DateFromSaleDatePicker.SelectedDate.Value.Date;
                DateTime dateTo = _DateToSaleDatePicker.SelectedDate.Value.Date;
                Models.Sale_DetailsDatabaseHelper sale_DetailsDatabaseHelper = new Sale_DetailsDatabaseHelper();
                Sale_DetailsList.Clear();
                Sale_DetailsList = sale_DetailsDatabaseHelper.GetRangeDatedSales(dateFrom, dateTo);
                Sale_DetailsList.Sort((x, y) => x.SaleDate.CompareTo(y.SaleDate));

                SalesProductListView.ItemsSource = Sale_DetailsList;
                LblProductTotalSales.Content = Sale_DetailsList.Count().ToString();
                float TotalProductAmount = Sale_DetailsList.Sum(s => s.Total);
                LblProductTAmount.Content = TotalProductAmount.ToString("#,#", CultureInfo.InvariantCulture);

                lblTotalQuantityProduct.Content = Sale_DetailsList.Sum(s => s.Quantity);
                SalesProductListView.Items.Refresh();

            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no products sales to show.");
                infobox.ShowDialog();
            }
        }

        private void btnSearchProducDateBetweentWiseSales_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                DateTime dateFrom = _DateFromSaleDatePicker.SelectedDate.Value.Date;
                DateTime dateTo = _DateToSaleDatePicker.SelectedDate.Value.Date;
                var product = ProductSalesCombo.SelectedItem as Entities.Product;
                Models.Sale_DetailsDatabaseHelper sale_DetailsDatabaseHelper = new Sale_DetailsDatabaseHelper();
                Sale_DetailsList.Clear();
                Sale_DetailsList = sale_DetailsDatabaseHelper.GetProductRangeDatedSales(product.Id,dateFrom, dateTo);
                Sale_DetailsList.Sort((x, y) => x.SaleDate.CompareTo(y.SaleDate));
                SalesProductListView.ItemsSource = Sale_DetailsList;
                LblProductTotalSales.Content = Sale_DetailsList.Count().ToString();
                float TotalProductAmount = Sale_DetailsList.Sum(s => s.Total);
                LblProductTAmount.Content = TotalProductAmount.ToString("#,#", CultureInfo.InvariantCulture);

                lblTotalQuantityProduct.Content = Sale_DetailsList.Sum(s => s.Quantity);
                SalesProductListView.Items.Refresh();

            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no products sales to show.");
                infobox.ShowDialog();
            }
        }

        private void btnSingleSearchProductWiseSales_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime datedProducts = _SaleDatePicker.SelectedDate.Value.Date;
                Sale_DetailsList.Clear();
                Models.Sale_DetailsDatabaseHelper sale_DetailsDatabaseHelper = new Sale_DetailsDatabaseHelper();
                Sale_DetailsList = sale_DetailsDatabaseHelper.GetDatedSales(datedProducts);
                Sale_DetailsList.Sort((x, y) => x.SaleDate.CompareTo(y.SaleDate));

                SalesProductListView.ItemsSource = Sale_DetailsList;
                LblProductTotalSales.Content = Sale_DetailsList.Count().ToString();
                float TotalProductAmount = Sale_DetailsList.Sum(s => s.Total);
                LblProductTAmount.Content = TotalProductAmount.ToString("#,#", CultureInfo.InvariantCulture);

                lblTotalQuantityProduct.Content = Sale_DetailsList.Sum(s => s.Quantity);

                SalesProductListView.Items.Refresh();

            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no products sales to show.");
                infobox.ShowDialog();
            }
        }

        private void txtPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void ProductCombo_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
              if(ProductCombo.ItemsSource != null && ProductCombo.SelectedItem != null)
                {
                    var product = ProductCombo.SelectedItem as Entities.Product;
                    if (CustomerCombo.SelectedItem != null)
                    {
                        int id = Convert.ToInt32(CustomerCombo.SelectedValue.ToString());

                        Entities.CustomerRate customerRate = customerRateDatabaseHelper.GetcustomerRate(product.Id, id);
                        if (customerRate != null)
                        {
                            txtPrice.Text = customerRate.Rate.ToString();
                        }
                        else
                        {
                            txtPrice.Text = product.Rate.ToString();
                        }
                    }
                    
                    var inventory = inventoryDatabaseHelper.GetSpecificInventory(product.Id);
                    if (inventory != null) 
                    {
                        txtInHand.Content = inventory.Quantity.ToString();
                    }
                }
            }
            catch (Exception)
            {

               InfoBox infobox = new InfoBox("Something went wrong...");
               infobox.ShowDialog();
            }
        }

        private void btnGridDelete_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.Sale deletesale =  SalesListView.SelectedItem as Entities.Sale;
                Models.SaleDatabaseHelper helper = new SaleDatabaseHelper();
                helper.DeleteSale(deletesale);
                InfoBox infobox = new InfoBox("Sale Deleted Successfully...");
                infobox.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                //InfoBox infobox = new InfoBox("Something went wrong..." + ex.ToString());
                //infobox.ShowDialog();
            }
        }

        private void btnwalkingGridDelete_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var deletesale = walkingSalesListView.SelectedItem as Entities.Sale;
                saleDatabaseHelper.DeleteWalkingSale(deletesale);
                InfoBox infobox = new InfoBox("Sale Deleted Successfully...");
                infobox.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //InfoBox infobox = new InfoBox("Something went wrong..." + ex.ToString());
                //infobox.ShowDialog();
            }
        }

        private void SpecificSearchDateBetween_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dateFrom = DateFrom.SelectedDate.Value.Date;
                DateTime dateTo = DateTo.SelectedDate.Value.Date;
                var customer = SearchCustomerCombo.SelectedItem as Entities.Customers;
                Models.SaleDatabaseHelper saleDatabaseHelper = new SaleDatabaseHelper();
                SalesList = saleDatabaseHelper.CustomerSalesBetweenDates(customer.Id,dateFrom, dateTo);
                SalesList.Sort((x, y) => x.SaleDate.CompareTo(y.SaleDate));

                SalesListView.ItemsSource = SalesList;
                SalesListView.Items.Refresh();
                LblTotalSales.Content = SalesList.Count;
                float totalamount = SalesList.Sum(s => s.TotalPrice);
                LblTAmount.Content = totalamount.ToString("#,#", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Date Range is not selected.");
                infobox.ShowDialog();
            }
        }

        private void ProductSalesCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetProductSales();
        }

        private void btnCustomerSales_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.SaleDatabaseHelper saleDatabaseHelper = new SaleDatabaseHelper();
                Entities.Product product = ProductSalesCombo.SelectedItem as Entities.Product;
                Entities.Customers customers = ComboSalesCustomer.SelectedItem as Entities.Customers;
                Sale_DetailsList.Clear();
                Sale_DetailsList = sale_DetailsDatabaseHelper.GetCustomerProductSales(product.Id,customers.Id);
                Sale_DetailsList.Sort((x, y) => x.SaleDate.CompareTo(y.SaleDate));
                SalesProductListView.ItemsSource = Sale_DetailsList;
                LblProductTotalSales.Content = Sale_DetailsList.Count().ToString();
                float TotalProductAmount = Sale_DetailsList.Sum(s => s.Total);
                LblProductTAmount.Content = TotalProductAmount.ToString("#,#", CultureInfo.InvariantCulture);
                lblTotalQuantityProduct.Content = Sale_DetailsList.Sum(s => s.Quantity);
                SalesProductListView.Items.Refresh();

            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no products sales to show.");
                infobox.ShowDialog();
            }
        }

        private void ComboSalesCustomer_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                var customers = customerDatabaseHelper.GetCustomers();
                ComboSalesCustomer.ItemsSource = customers;
                ComboSalesCustomer.Items.Refresh();
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no customers to show.");
                infobox.ShowDialog();
            }
        }

        private void btnSearchByID_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.SaleDatabaseHelper saleDatabaseHelper = new SaleDatabaseHelper();
                string SaleID = txtSaleID.Text;
                SalesList = saleDatabaseHelper.SaleByID(SaleID);
                SalesList.Sort((x, y) => x.SaleDate.CompareTo(y.SaleDate));

                SalesListView.ItemsSource = SalesList;
                SalesListView.Items.Refresh();
                LblTotalSales.Content = SalesList.Count;
                float totalamount = SalesList.Sum(s => s.TotalPrice);
                LblTAmount.Content = totalamount.ToString("#,#", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There are no sales to show.");
                infobox.ShowDialog();
            }

        }

        private void btnProductCustomerRange_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                DateTime dateFrom = _DateFromSaleDatePicker.SelectedDate.Value.Date;
                DateTime dateTo = _DateToSaleDatePicker.SelectedDate.Value.Date;
                var product = ProductSalesCombo.SelectedItem as Entities.Product;
                var customer = ComboSalesCustomer.SelectedItem as Entities.Customers;
                Models.Sale_DetailsDatabaseHelper sale_DetailsDatabaseHelper = new Sale_DetailsDatabaseHelper();
                Sale_DetailsList.Clear();
                Sale_DetailsList = sale_DetailsDatabaseHelper.GetCustomerProductRangeDatedSales(customer.Id, product.Id, dateFrom, dateTo);
                Sale_DetailsList.Sort((x, y) => x.SaleDate.CompareTo(y.SaleDate));
                SalesProductListView.ItemsSource = Sale_DetailsList;
                LblProductTotalSales.Content = Sale_DetailsList.Count().ToString();
                float TotalProductAmount = Sale_DetailsList.Sum(s => s.Total);
                LblProductTAmount.Content = TotalProductAmount.ToString("#,#", CultureInfo.InvariantCulture);

                lblTotalQuantityProduct.Content = Sale_DetailsList.Sum(s => s.Quantity);
                SalesProductListView.Items.Refresh();

            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no products sales to show.");
                infobox.ShowDialog();
            }
        }

        private void SearchCustomerCombo_MouseEnter_1(object sender, MouseEventArgs e)
        {
            try
            {
                customerlist = customerDatabaseHelper.GetCustomers().ToList();
                customerlist.Sort((x, y) => string.Compare(x.Name, y.Name));
                SearchCustomerCombo.ItemsSource = customerlist;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no customers to show.");
                infobox.ShowDialog();
            }

        }

        private void SearchCustomerCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(SearchCustomerCombo.SelectedValue.ToString());
                Models.SaleDatabaseHelper saleDatabaseHelper = new SaleDatabaseHelper();
                SalesList = saleDatabaseHelper.GetCustomerSales(id);
                SalesList.Sort((x, y) => x.SaleDate.CompareTo(y.SaleDate));

                SalesListView.ItemsSource = SalesList;
                SalesListView.Items.Refresh();
                LblTotalSales.Content = SalesList.Count;
                float totalamount = SalesList.Sum(s => s.TotalPrice);
                LblTAmount.Content = totalamount.ToString();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There is no data to show.");
                infobox.ShowDialog();
            }
        }
    }
}
