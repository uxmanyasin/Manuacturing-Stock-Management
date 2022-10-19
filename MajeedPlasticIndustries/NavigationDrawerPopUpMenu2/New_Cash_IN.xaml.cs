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
using System.Windows.Threading;
using BizBook.Entities;
using BizBook.Models;

namespace BizBook
{
    
    public partial class New_Cash_IN : Window
    {
       
        private readonly PaymentsDatabaseHelper helper = new PaymentsDatabaseHelper();
        private readonly DueAmountDatabaseHelper amountDatabaseHelper = new DueAmountDatabaseHelper();
        Entities.DueAmount _dueAmount;
        public New_Cash_IN(Entities.DueAmount dueAmount)
        {
            InitializeComponent();
            _dueAmount = dueAmount;
            txtDueAmount.Text = _dueAmount.PendingAmount.ToString();
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       
        private void BtnSaveCashIN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? id = _dueAmount.CustomerId;
                int due = _dueAmount.PendingAmount;
                int payment = Convert.ToInt32(txtAmount.Text);
                payment = due - payment;
                if (payment == 0)
                {
                    amountDatabaseHelper.StatusChange(_dueAmount);
                    InfoBox infoboxx = new InfoBox("Due amount is cleared,Transaction updated successfuly.");

                    infoboxx.ShowDialog();
                    infoboxx.Close();
                    this.Close();

                }
                else if(payment < 0)
                {
                    InfoBox _infobox = new InfoBox("Payment amount should not less than due amount.");
                    _infobox.ShowDialog();
                }
                else
                {
                    Entities.DueAmount dueAmount = _dueAmount;
                    dueAmount.PendingAmount = payment;
                    Payments payments = new Payments()
                    {
                        CustomerID = id,
                        Credit = float.Parse(txtAmount.Text),
                        Date = DatePicker.SelectedDate.Value.Date,
                        EntryDate = DateTime.Now.Date,
                        Type = "Customer",
                        Description = txtDescription.Text,
                        Debit = 0

                    };
                    amountDatabaseHelper.PartialPayment(dueAmount);
                    helper.CreatePayment(payments);
                    InfoBox infobox1 = new InfoBox("Transaction successfully updated.");

                    infobox1.ShowDialog();
                    infobox1.Close();
                    this.Close();
                }
            
            }

            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Error: Something went wrong please try again.");
                infobox.ShowDialog();
            }
        }
        
    }
}
