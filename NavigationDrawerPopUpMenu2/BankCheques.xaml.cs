using BizBook.Entities;
using BizBook.Models;
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

namespace BizBook
{
   
    public partial class BankCheques : UserControl
    {
        private readonly BankChequeDatabaseHelper bankChequeDatabaseHelper = new BankChequeDatabaseHelper();
        private readonly CustomerDatabaseHelper customerDatabaseHelper = new CustomerDatabaseHelper();
        private readonly PaymentsDatabaseHelper paymentsDatabaseHelper = new PaymentsDatabaseHelper();
        public bool Access;
        List<Entities.Customers> customersList = new List<Customers>();
        List<Entities.BankCheque> bankChequesList = new List<BankCheque>();
       
        public BankCheques()
        {
            InitializeComponent();
            LoadPendingCheques();
             Access = Properties.Configuration.Default.AccessMode;
            if (Access == true)
            {
                btnLoadAllCheques.IsEnabled = false;
                btnLoadCompletedCheques.IsEnabled = false;
            }
        }

        private void LoadPendingCheques() 
        {
            try
            {
                ChequesListView.IsEnabled = true;
                bankChequesList.Clear();
                bankChequesList = bankChequeDatabaseHelper.GetPendingCheques().ToList();
                float totalAmount = bankChequesList.Sum(s => s.Amount);
                int TotalCheques = bankChequesList.Count();
                lblTotalCheques.Content = TotalCheques.ToString();
                lblTotalAmount.Content = totalAmount.ToString();
                ChequesListView.ItemsSource = bankChequesList;
                ChequesListView.Items.Refresh();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no Cheques to show.");
                infobox.ShowDialog();
            }
        }


        private void LoadCompletedCheques()
        {
            try
            {
                if (Access == true)
                {
                    InfoBox infobox = new InfoBox("You are not authorized for this action.");
                    infobox.ShowDialog();
                }
                else
                {
                    ChequesListView.IsEnabled = false;
                    bankChequesList.Clear();
                    bankChequesList = bankChequeDatabaseHelper.GetComlpetedCheques().ToList();
                    float totalAmount = bankChequesList.Sum(s => s.Amount);
                    int TotalCheques = bankChequesList.Count();
                    lblTotalCheques.Content = TotalCheques.ToString();
                    lblTotalAmount.Content = totalAmount.ToString();
                    ChequesListView.ItemsSource = bankChequesList;
                    ChequesListView.Items.Refresh();
                }
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no Cheques to show.");
                infobox.ShowDialog();
            }
        }


        private void LoadAllCheques()
        {
            try
            {
                if (Access == true)
                {
                    InfoBox infobox = new InfoBox("You are not authorized for this action.");
                    infobox.ShowDialog();
                }
                else
                {
                    ChequesListView.IsEnabled = false;
                    bankChequesList.Clear();
                    bankChequesList = bankChequeDatabaseHelper.GetCheques().ToList();
                    float totalAmount = bankChequesList.Sum(s => s.Amount);
                    int TotalCheques = bankChequesList.Count();
                    lblTotalCheques.Content = TotalCheques.ToString();
                    lblTotalAmount.Content = totalAmount.ToString();
                    ChequesListView.ItemsSource = bankChequesList;
                    ChequesListView.Items.Refresh();
                }
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no Cheques to show.");
                infobox.ShowDialog();
            }
        }

        private void btnSearchSingleDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Access == false) 
                {
                    ChequesListView.IsEnabled = false;
                    string date = SearchSpecificPicker.SelectedDate.Value.Date.ToString();
                    float totalAmount = bankChequesList.Sum(s => s.Amount);
                    int TotalCheques = bankChequesList.Count();
                    lblTotalCheques.Content = TotalCheques.ToString();
                    lblTotalAmount.Content = totalAmount.ToString();
                    bankChequesList.Clear();
                    bankChequesList = bankChequeDatabaseHelper.GetDatedCheques(date);
                    ChequesListView.ItemsSource = bankChequesList;
                    ChequesListView.Items.Refresh();
                }
                else if (Access == true) 
                {
                    InfoBox infobox = new InfoBox("You are not authorized for this action.");
                    infobox.ShowDialog();
                }
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no Cheques to show.");
                infobox.ShowDialog();
            }
        }

        private void CustomerCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
               
                  customersList = customerDatabaseHelper.GetCustomers();
                    CustomerCombo.ItemsSource = customersList;
               
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no custoemrs to show.");
                infobox.ShowDialog();
            }
        }

        private void btnSearchDues_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                    Entities.BankCheque _bankCheque = ChequesListView.SelectedItem as BankCheque;
                    if (_bankCheque.Status == 0)
                    {
                        bankChequeDatabaseHelper.CashInCheque(_bankCheque);
                        InfoBox infobox = new InfoBox(_bankCheque.Amount + " PKR has been added to your company account.");
                        infobox.ShowDialog();
                    }
                    else
                    {
                        InfoBox infobox = new InfoBox("This cheque is already cashed in to your comapny.");
                        infobox.ShowDialog();
                    }
              
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,please try again.");
                infobox.ShowDialog();
            }
        }

        private void btnLoadAllCheques_Click(object sender, RoutedEventArgs e)
        {
            LoadAllCheques();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {

        }

        private void btnLoadPendingCheques_Click(object sender, RoutedEventArgs e)
        {
            LoadPendingCheques();
        }

        private void btnLoadCompletedCheques_Click(object sender, RoutedEventArgs e)
        {
            LoadCompletedCheques();
        }

        private void btnNewCheque_Click(object sender, RoutedEventArgs e)
        {
            ReceiveCheque receiveCheque = new ReceiveCheque();
            receiveCheque.ShowDialog();
        }

        private void CustomerCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Access == true)
                {
                    InfoBox infobox = new InfoBox("You are not authorized for this action.");
                    infobox.ShowDialog();
                }
                else
                {
                    ChequesListView.IsEnabled = true;
                    bankChequesList.Clear();
                    int id = Convert.ToInt32(CustomerCombo.SelectedValue.ToString());
                    bankChequesList = bankChequeDatabaseHelper.GetCustomerCheques(id);
                    float totalAmount = bankChequesList.Sum(s => s.Amount);
                    int TotalCheques = bankChequesList.Count();
                    lblTotalCheques.Content = TotalCheques.ToString();
                    lblTotalAmount.Content = totalAmount.ToString();
                    ChequesListView.ItemsSource = bankChequesList;
                    ChequesListView.Items.Refresh();
                }
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there is no selected customer to show cheques.");
                infobox.ShowDialog();
            }
        }

        private void btnEditCheque_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Access == true)
                {
                    InfoBox infobox = new InfoBox("You are not authorized for this action.");
                    infobox.ShowDialog();
                }
                else
                {
                    Entities.BankCheque _bankCheque = ChequesListView.SelectedItem as BankCheque;
                    ReceiveCheque receiveCheque = new ReceiveCheque(_bankCheque);
                    receiveCheque.ShowDialog();
                }
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There is no Cheque selected to edit.");
                infobox.ShowDialog();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Access == true)
                {
                    InfoBox infobox = new InfoBox("You are not authorized for this action.");
                    infobox.ShowDialog();
                }
                else
                {
                    Entities.BankCheque _bankCheque = ChequesListView.SelectedItem as BankCheque;
                    if (_bankCheque.Status == 0)
                    {
                        bankChequeDatabaseHelper.DeleteCheque(_bankCheque);
                        InfoBox infobox = new InfoBox("Selected cheque deleted.");
                        infobox.ShowDialog();
                    }
                    else
                    {
                        InfoBox infobox = new InfoBox("Selected cheque cannot be deleted because it is cashed in to your company.");
                        infobox.ShowDialog();
                    }
                }
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There is no Cheque selected to delete.");
                infobox.ShowDialog();
            }
        }
    }
}
