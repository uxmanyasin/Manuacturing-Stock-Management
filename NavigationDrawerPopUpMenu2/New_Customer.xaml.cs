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
    
    public partial class New_Customer : Window
    {
        private readonly CustomerDatabaseHelper helper = new CustomerDatabaseHelper();
        private Customers Customers;
        private PaymentsDatabaseHelper PaymentsDatabaseHelper = new PaymentsDatabaseHelper();
        public bool update { get; set; }
        public New_Customer()
        {
            InitializeComponent();
        }

       
        public New_Customer(Customers customers,bool indicater)
        {
            InitializeComponent();
            Customers = customers;
            update = indicater;
            txtName.Text = Customers.Name;
            txtContact.Text = Customers.Contact;
            txtAddress.Text = Customers.Address;
            txtcreditlimit.Text = customers.CreditLimit.ToString();
            txtCreditBalance.Text = customers.CreditBalance.ToString();
            txtDebitBalance.Text = customers.DebitBalance.ToString();
            lblBalanceDate.Text = Customers.Date.ToString();
            btnNewCustomer.Content = "Update";
            LblTitle.Text = "Update Customer";
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (update == true)
                {
                    Customers.Name = txtName.Text;
                    Customers.Contact = txtContact.Text;
                    Customers.Address = txtAddress.Text;
                    Customers.CreditBalance = float.Parse(txtCreditBalance.Text);
                    Customers.DebitBalance = float.Parse(txtDebitBalance.Text);
                    Customers.CreditLimit = Convert.ToInt32(txtcreditlimit.Text);
                    Customers.CreditStatus = 1;
                    DateTime date = DatePicker.SelectedDate.Value.Date;
                    helper.EditCustomer(Customers, date);
                    InfoBox infobox = new InfoBox("Updated successfuly.");
                    infobox.ShowDialog();
                    this.Close();
                }
                else
                {
                    Entities.Customers customers = new Customers()
                    {
                        Name = txtName.Text,
                        Contact = txtContact.Text,
                        Address = txtAddress.Text,
                        Date = DateTime.Now.Date.ToString(),
                        CreditLimit = Convert.ToInt32(txtcreditlimit.Text),
                        CreditBalance = float.Parse(txtCreditBalance.Text),
                        DebitBalance = float.Parse(txtDebitBalance.Text),
                        CreditStatus = 1
                    };
                   
                    Customers model = helper.CreateCustomer(customers,DatePicker.SelectedDate.Value.Date);
                    InfoBox infobox = new InfoBox("New Customer Added Successfuly.");
                    infobox.ShowDialog();
                    this.Close();
                }

            }
            catch (Exception ex)
            {

                InfoBox infobox = new InfoBox("ERROR:All Feilds are required.");
                infobox.ShowDialog();
            }
        }

        private void txtAddress_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
