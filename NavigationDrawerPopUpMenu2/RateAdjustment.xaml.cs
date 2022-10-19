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
   
    public partial class RateAdjustment : Window
    {
        private readonly ProductDatabaseHelper helper = new ProductDatabaseHelper();
        private readonly Sale_DetailsDatabaseHelper sale_DetailsDatabaseHelper = new Sale_DetailsDatabaseHelper();
        private readonly SaleDatabaseHelper saleDatabaseHelper = new SaleDatabaseHelper();
        private readonly CustomerDatabaseHelper customerDatabaseHelper = new CustomerDatabaseHelper();
        private readonly PaymentsDatabaseHelper paymentsDatabaseHelper = new PaymentsDatabaseHelper();
        List<Entities.Customers> customersList = new List<Entities.Customers>();
        List<Entities.Sale_Details> sale_DetailsList = new List<Sale_Details>();
        List<Entities.Product> productsList = new List<Product>();

        public RateAdjustment()
        {
            InitializeComponent();
        }

        private void btnAdjustPricing_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime Fromdate = datePicker.SelectedDate.Value.Date;
                Entities.Product _product = ProductCombo.SelectedItem as Entities.Product;
                DateTime ToDate = DateTime.Now.Date;
               
                Entities.Customers _customer = CustomerCombo.SelectedItem as Entities.Customers;

                sale_DetailsList = sale_DetailsDatabaseHelper.GetDatedProductSales(_product.Id, Fromdate, ToDate, _customer.Id);
                
                foreach (var item in sale_DetailsList) 
                {
                    Entities.Sale sale = saleDatabaseHelper.GetSale(item.SaleId);
                    float xSales = sale.TotalPrice; float xSubTotal = item.Total; float Remainder = xSales - xSubTotal;
                    float newSubTotal = item.Quantity * (float.Parse(txtRate.Text));
                    float newTotalPrice = newSubTotal + Remainder;
                    item.Rate = float.Parse(txtRate.Text);
                    item.Total = newSubTotal;
                    sale.TotalPrice = newTotalPrice;
                    saleDatabaseHelper.EditSaleAmount(sale);
                    paymentsDatabaseHelper.EditSaleAmountPayment(sale);
                    sale_DetailsDatabaseHelper.UpdateRates(item);  
                }
                
                var sale_Details1 = sale_DetailsDatabaseHelper.GetDatedProductSales(_product.Id, Fromdate, ToDate, _customer.Id);
               
                InfoBox infobox = new InfoBox("Rate of " + _product.Name + " updated succesfully.");
                infobox.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void ProductCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                    productsList = helper.GetProducts();
                    productsList.Sort((x, y) => string.Compare(x.Name, y.Name));
                    ProductCombo.ItemsSource = productsList;
                    ProductCombo.Items.Refresh();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no products to show.");
                infobox.ShowDialog();
            }
        }

        private void CategoryCombo_MouseEnter(object sender, MouseEventArgs e)
        {
           
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CustomerCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                customersList = customerDatabaseHelper.GetCustomers();
                customersList.Sort((x, y) => string.Compare(x.Name, y.Name));
                CustomerCombo.ItemsSource = customersList;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no customers to show.");
                infobox.ShowDialog();
            }
        }
    }
}
