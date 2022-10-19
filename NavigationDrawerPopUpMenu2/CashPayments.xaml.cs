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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BizBook.Entities;
using BizBook.Models;

namespace BizBook
{
   
    public partial class CashPayments : UserControl
    {
        public List<Customers> customerlist = new List<Customers>();
        private readonly PaymentsDatabaseHelper helper = new PaymentsDatabaseHelper();
        private readonly CustomerDatabaseHelper customerDatabaseHelper = new CustomerDatabaseHelper();
        public List<Entities.Suppliers> supplierList = new List<Entities.Suppliers>();
        private readonly SupplierDatabaseHelper supplierDatabaseHelper = new SupplierDatabaseHelper();

        public CashPayments()
        {
            InitializeComponent();
        }

        private void CustomerCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                customerlist = customerDatabaseHelper.GetCustomers().ToList();
                CustomerCombo.ItemsSource = customerlist;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("ERROR:All Feilds are required.");
                infobox.ShowDialog();
            }
        }

        private void txtDesc_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnSaveCashIN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(CustomerCombo.SelectedValue.ToString());
                Payments payments = new Payments()
                {
                    CustomerID = id,
                    Credit = Convert.ToInt32(txtAmount.Text),
                    Date = DatePicker.SelectedDate.Value.Date,
                    EntryDate = DateTime.Now.Date,
                    Type = "Customer",
                    Description = txtDesc.Text,
                    Debit = 0

                };

                helper.CreatePayment(payments);
                InfoBox infobox = new InfoBox("Transaction successfully added.");
                infobox.ShowDialog();
            }

            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Error: All feilds are required.");
                infobox.ShowDialog();
            }
        }

        private void btnSaveCashOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(SupplierCombo.SelectedValue.ToString());
                Payments payments = new Payments()
                {
                    SupplierID = id,
                    Debit = Convert.ToInt32(txtAmountsupplier.Text),
                    EntryDate = DateTime.Now.Date,
                    Date = DatePickersupplier.SelectedDate.Value.Date,
                    Type = "Supplier",
                    Description = txtDescsupplier.Text,
                    Credit = 0
                };
                helper.CreatePayment(payments);
                InfoBox infobox = new InfoBox("Transaction successfully added.");
                infobox.ShowDialog();
            }
            catch (Exception)
            {
               InfoBox infobox = new InfoBox("ERROR:All Feilds are required.");
               infobox.ShowDialog();
            }
        }

        private void SupplierCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                supplierList = supplierDatabaseHelper.GetSuppliers().ToList();
                SupplierCombo.ItemsSource = supplierList;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("No Supplier Selected.");
                infobox.ShowDialog();
            }
        }
    }
}
