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
  
    public partial class New_CashINAdvance : Window
    {
        public List<Entities.Customers> customerList = new List<Entities.Customers>();
        public List<Entities.Suppliers> supplierList = new List<Suppliers>();
        private readonly SupplierDatabaseHelper supplierDatabaseHelper = new SupplierDatabaseHelper();
        private readonly CustomerDatabaseHelper customerDatabaseHelper = new CustomerDatabaseHelper();
        private readonly PaymentsDatabaseHelper paymentsDatabaseHelper = new PaymentsDatabaseHelper();
        public Entities.Payments _UpdatePayments = new Payments();
        public bool CustomerCheck;
        public New_CashINAdvance()
        {
            InitializeComponent();
        }

        public New_CashINAdvance(Entities.Payments payments,bool Customer)
        {
            InitializeComponent();
            _UpdatePayments = payments;
            CustomerCheck = Customer;
            lblTitle.Text = "Update Cash In";
            btnSaveCashIn.Content = "Update Info";
            if (payments.CustomerID != null)
            {
                CustomerCombo.Text = payments.customers.Name;
                txtDesc.Text = payments.Description;
                TxtAmount.Text = payments.Credit.ToString();
                SupplierCombo.IsEnabled = false;
                lblDate.Text = payments.Date.ToString();
            }
            else if (payments.SupplierID != null)
            {
                SupplierCombo.Text = payments.Suppliers.Name;
                txtDesc.Text = payments.Description;
                TxtAmount.Text = payments.Credit.ToString();
                CustomerCombo.IsEnabled = false;
                lblDate.Text = payments.Date.ToString();
            }
        }

        private void CustomerCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
               
                customerList = customerDatabaseHelper.GetCustomers().ToList();
                customerList.Sort((x, y) => string.Compare(x.Name, y.Name));
                CustomerCombo.ItemsSource = customerList;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("No Customer Selected.");
                infobox.ShowDialog();
            }
        }

        private void btnSaveCashIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lblTitle.Text == "Update Cash In")
                {
                    if (CustomerCheck == true) 
                    {
                        if (CustomerCombo.SelectedItem != null)
                        {
                            int id = Convert.ToInt32(CustomerCombo.SelectedValue.ToString());
                            _UpdatePayments.CustomerID = id;
                        }
                        if (DatePicker.SelectedDate.HasValue)
                        {
                            _UpdatePayments.Date = DatePicker.SelectedDate.Value.Date;
                        }
                        _UpdatePayments.Credit = Convert.ToInt32(TxtAmount.Text);
                        _UpdatePayments.Description = txtDesc.Text;
                        paymentsDatabaseHelper.EditPayment(_UpdatePayments);
                        InfoBox infobox = new InfoBox(" Customer Transaction successfully updated.");
                        infobox.ShowDialog();
                    }

                   else if (CustomerCheck == false)
                    {
                        if (SupplierCombo.SelectedItem != null)
                        {
                            int id = Convert.ToInt32(SupplierCombo.SelectedValue.ToString());
                            _UpdatePayments.SupplierID = id;
                        }
                        if (DatePicker.SelectedDate.HasValue)
                        {
                            _UpdatePayments.Date = DatePicker.SelectedDate.Value.Date;
                        }
                        _UpdatePayments.Credit = Convert.ToInt32(TxtAmount.Text);
                        _UpdatePayments.Description = txtDesc.Text;
                        paymentsDatabaseHelper.EditPayment(_UpdatePayments);
                        InfoBox infobox = new InfoBox(" Supplier Transaction successfully updated.");
                        infobox.ShowDialog();
                    }

                    this.Close();
                }
                else 
                {
                    if (CustomerCombo.SelectedItem != null)
                    {
                        int id = Convert.ToInt32(CustomerCombo.SelectedValue.ToString());
                        Payments payments = new Payments()
                        {
                            CustomerID = id,
                            Credit = Convert.ToInt32(TxtAmount.Text),
                            EntryDate = DateTime.Now.Date,
                            Date = DatePicker.SelectedDate.Value.Date,
                            Type = "Customer",
                            Description = txtDesc.Text,
                            Debit = 0
                        };
                        paymentsDatabaseHelper.CreatePayment(payments);
                        InfoBox infobox = new InfoBox(" Customer Transaction successfully added.");
                        infobox.ShowDialog();
                    }
                    else if (SupplierCombo.SelectedItem != null)
                    {
                        int id = Convert.ToInt32(SupplierCombo.SelectedValue.ToString());
                        Payments payments = new Payments()
                        {
                            SupplierID = id,
                            Credit = float.Parse(TxtAmount.Text),
                            EntryDate = DateTime.Now.Date,
                            Date = DatePicker.SelectedDate.Value.Date,
                            Type = "Supplier",
                            Description = txtDesc.Text,
                            Debit = 0
                        };
                        paymentsDatabaseHelper.CreatePayment(payments);
                        InfoBox infobox = new InfoBox(" Supplier Transaction successfully added.");
                        infobox.ShowDialog();
                    }
                }

                CustomerCombo.ItemsSource = null;
                SupplierCombo.ItemsSource = null;
                txtDesc.Clear();
                TxtAmount.Clear();
              
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong...Please try again.");
                infobox.ShowDialog();
                this.Close();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SupplierCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
               
                supplierList = supplierDatabaseHelper.GetSuppliers();
                supplierList.Sort((x, y) => string.Compare(x.Name, y.Name));
                SupplierCombo.ItemsSource = supplierList;

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("No Supplier Selected.");
                infobox.ShowDialog();
            }
        }

        private void SupplierCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CustomerCombo.ItemsSource = null;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CustomerCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SupplierCombo.ItemsSource = null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
