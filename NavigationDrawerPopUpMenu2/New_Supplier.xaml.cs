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
    
    public partial class New_Supplier : Window
    {
        
        private readonly SupplierDatabaseHelper supplierDatabaseHelper = new SupplierDatabaseHelper();
        public bool update { get; set; }
        private Suppliers Suppliers;
        public New_Supplier()
        {
            InitializeComponent();
        }

        public New_Supplier(Suppliers suppliers,bool indicater)
        {
            InitializeComponent();
            update = indicater;
            Suppliers = suppliers;
            txtName.Text = Suppliers.Name;
            txtContact.Text = Suppliers.Contact;
            txtAddress.Text = Suppliers.Address;
            txtCreditBalance.Text = Suppliers.CreditBalance.ToString();
            txtDebitBalance.Text = Suppliers.DebitBalance.ToString();
            lblOpenBalanceDate.Text = Suppliers.Date.ToString();
            btnNewSupplier.Content = "Update";
            LblTitle.Text = "Update Supplier";
        }

        private void BtnNewSupplier_Click(object sender, RoutedEventArgs e)
        {
            try{
                if (update == true)
                {
                    Suppliers.Name = txtName.Text;
                    Suppliers.Contact = txtContact.Text;
                    Suppliers.Address = txtAddress.Text;
                    Suppliers.CreditBalance = float.Parse(txtCreditBalance.Text);
                    Suppliers.DebitBalance = float.Parse(txtDebitBalance.Text);
                    DateTime date = DatePicker.SelectedDate.Value.Date;
                    supplierDatabaseHelper.EditSupplier(Suppliers, date);
                    InfoBox infobox = new InfoBox ("Supplier updated successfully.");
                    infobox.ShowDialog();
                    this.Close();
                }
                else
                {
                    Suppliers suppliers = new Suppliers()
                    {
                        Name = txtName.Text,
                        Contact = txtContact.Text,
                        Address = txtAddress.Text,
                        Date = DateTime.Now.ToShortDateString(),
                        CreditBalance = float.Parse(txtCreditBalance.Text),
                        DebitBalance = float.Parse(txtDebitBalance.Text)
                    };
                    Suppliers model = supplierDatabaseHelper.CreateSupplier(suppliers , DatePicker.SelectedDate.Value.Date);
                    InfoBox infobox = new InfoBox("Supplier created successfully.");
                    infobox.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //InfoBox infobox = new InfoBox("ERROR:All Feilds are required.");
                //infobox.ShowDialog();
            }
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
