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
    
    public partial class ReceiveCheque : Window
    {
        private readonly CustomerDatabaseHelper customerDatabaseHelper = new CustomerDatabaseHelper();
        private readonly BankChequeDatabaseHelper bankChequeDatabaseHelper = new BankChequeDatabaseHelper();
        public List<Entities.Customers> customersList = new List<Customers>();
        public Entities.BankCheque _BankCheque = new BankCheque();

        public ReceiveCheque()
        {
            InitializeComponent();
        }

        public ReceiveCheque(BankCheque bankCheque)
        {
            InitializeComponent();
            _BankCheque = bankCheque;
            txtAmount.Text = _BankCheque.Amount.ToString();
            txtDescription.Text = _BankCheque.Description;
            txtChequeNumber.Text = _BankCheque.ChequeNumber;
            lblTitle.Text = "Update Cheque";
            btnSave.Content = "Update";

        }

        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CustomerCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                customersList = customerDatabaseHelper.GetCustomers().ToList();
                customersList.Sort((x, y) => string.Compare(x.Name, y.Name));
                CustomerCombo.ItemsSource = customersList;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("No Customer Selected.");
                infobox.ShowDialog();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(lblTitle.Text == "Update Cheque") 
                {
                    int id = Convert.ToInt32(CustomerCombo.SelectedValue.ToString());
                    Entities.BankCheque _cheque = new BankCheque
                    {
                        CustomerID = id,
                        Id = _BankCheque.Id,
                        Amount = Convert.ToInt32(txtAmount.Text),
                        CashDate = DatePicker.SelectedDate.Value.ToString(),
                        Description = txtDescription.Text,
                        ChequeNumber = txtChequeNumber.Text,
                        Status = 0

                    };

                    bankChequeDatabaseHelper.EditCheque(_cheque);
                    InfoBox infobox = new InfoBox("Cheque Updated...");
                    infobox.ShowDialog();
                   

                }
                else 
                {
                    int id = Convert.ToInt32(CustomerCombo.SelectedValue.ToString());
                    Entities.BankCheque _cheque = new BankCheque
                    {
                        CustomerID = id,
                        Amount = Convert.ToInt32(txtAmount.Text),
                        CashDate = DatePicker.SelectedDate.Value.ToString(),
                        Description = txtDescription.Text,
                        ChequeNumber = txtChequeNumber.Text,
                        Status = 0

                    };
                    bankChequeDatabaseHelper.CreateCheque(_cheque);
                    InfoBox infobox = new InfoBox("Cheque Received...");
                    infobox.ShowDialog();
                    CustomerCombo.ItemsSource = null;
                    txtAmount.Clear();
                    txtChequeNumber.Clear();
                    txtDescription.Clear();
                }
            }
            catch (Exception)
            {
                
                InfoBox infobox = new InfoBox("ERROR:All Feilds are required.");
                infobox.ShowDialog();
            }

        }
    }
}
