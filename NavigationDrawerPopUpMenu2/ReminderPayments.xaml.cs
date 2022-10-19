using System;
using System.Collections.Generic;
using System.Data;
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
   
    public partial class ReminderPayments : UserControl
    {
        private readonly DueAmountDatabaseHelper helper = new DueAmountDatabaseHelper();
        public List<Entities.DueAmount> dueAmountsList = new List<Entities.DueAmount>();
        public List<Entities.Customers> customersList = new List<Customers>();
        private readonly CustomerDatabaseHelper customerDatabaseHelper = new CustomerDatabaseHelper();
        public ReminderPayments()
        {
            InitializeComponent();
            loadDueReminders();
        }

        public void loadDueReminders() 
        {
            try
            {
                dueAmountsList.Clear();
                dueAmountsList = helper.GetDues();
                DueListView.ItemsSource = dueAmountsList;
                DueListView.Items.Refresh();
                int totalDues = dueAmountsList.Sum(s => s.PendingAmount);
                LblTotalDues.Content = totalDues;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no raw materials  to show.");
                infobox.ShowDialog();
            }
        }

       

        private void btnLoadAllDues_Click(object sender, RoutedEventArgs e)
        {
            loadDueReminders();
        }

        private void btnSearchDues_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(CustomerCombo.SelectedValue.ToString());
                dueAmountsList.Clear();
                dueAmountsList = helper.GetCustomerDues(id);
                DueListView.ItemsSource = dueAmountsList;
                DueListView.Items.Refresh();
                int totalDues = dueAmountsList.Sum(s => s.PendingAmount);
                LblTotalDues.Content = totalDues;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no dues to show.");
                infobox.ShowDialog();
            }
        }

        private void CustomerCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                customersList.Clear();
                customersList = customerDatabaseHelper.GetCustomers().ToList();
                CustomerCombo.ItemsSource = customersList;
                
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("ERROR:All Feilds are required.");
                infobox.ShowDialog();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.DueAmount dueAmount = DueListView.SelectedItem as Entities.DueAmount;
                New_Cash_IN new_Cash_IN = new New_Cash_IN(dueAmount);
                new_Cash_IN.ShowDialog();

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("ERROR:There is no selected due to paid.");
                infobox.ShowDialog();
            }
        }

        private void btnPartialPayment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.DueAmount dueAmount = DueListView.SelectedItem as Entities.DueAmount;

                helper.PartialPayment(dueAmount);
                loadDueReminders();
                InfoBox infobox = new InfoBox("Dues updated successfuly.");
                infobox.ShowDialog();

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("ERROR:There is no selected due to paid.");
                infobox.ShowDialog();
            }
        }

        private void btnSearchDateBetween_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dateFrom = SearchDateFromPicker.SelectedDate.Value.Date;
                DateTime dateTo = SearchDateToPicker.SelectedDate.Value.Date;
                dueAmountsList.Clear();
                dueAmountsList = helper.DuesBetweenDates(dateFrom, dateTo);
                DueListView.ItemsSource = dueAmountsList;
                DueListView.Items.Refresh();
                int totalDues = dueAmountsList.Sum(s => s.PendingAmount);
                LblTotalDues.Content = totalDues;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no dues to show.");
                infobox.ShowDialog();
            }
        }

        private void btnSearchSingleDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime searchdate = SearchSpecificPicker.SelectedDate.Value.Date;
               
                dueAmountsList.Clear();
                dueAmountsList = helper.GetDatedDues(searchdate);
                DueListView.ItemsSource = dueAmountsList;
                DueListView.Items.Refresh();
                int totalDues = dueAmountsList.Sum(s => s.PendingAmount);
                LblTotalDues.Content = totalDues;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no dues to show.");
                infobox.ShowDialog();
            }
        }
    }
}
