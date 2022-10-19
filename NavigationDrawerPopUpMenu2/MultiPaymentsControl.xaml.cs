using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BizBook
{
   
    public partial class MultiPaymentsControl : UserControl
    {
        List<Models.DummyModels.PaymentsListModel> PaymentsList = new List<Models.DummyModels.PaymentsListModel>();
        IFormatProvider usFormatProvider = new System.Globalization.CultureInfo("en-US");
        public bool IsCredit { get; set; }

        public MultiPaymentsControl(bool isCredit)
        {
            InitializeComponent();
            IsCredit = isCredit;
        }
        private void CustomerCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (rdbCustomer.IsChecked == true && rdbSupplier.IsChecked == false)
                {
                    Models.CustomerDatabaseHelper helper = new Models.CustomerDatabaseHelper();
                    var list = helper.GetCustomers();
                    CustomerCombo.ItemsSource = list;
                    CustomerCombo.Items.Refresh();
                }
                else if (rdbCustomer.IsChecked == false && rdbSupplier.IsChecked == true)
                {
                    Models.SupplierDatabaseHelper helper = new Models.SupplierDatabaseHelper();
                    var list = helper.GetSuppliers();
                    CustomerCombo.ItemsSource = list;
                    CustomerCombo.Items.Refresh();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = PaymentListView.SelectedItem as Models.DummyModels.PaymentsListModel;
                if (obj != null)
                {
                    PaymentsList.Remove(obj);
                    PaymentListView.Items.Refresh();
                    if (IsCredit is true)
                    {
                        lblTotalAmount.Content = PaymentsList.Sum(s => s.Credit).ToString("#,##0.00", usFormatProvider);
                    }
                    else if (IsCredit is false)
                    {
                        lblTotalAmount.Content = PaymentsList.Sum(s => s.Debit).ToString("#,##0.00", usFormatProvider);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SavePayments_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.PaymentsDatabaseHelper helper = new Models.PaymentsDatabaseHelper();
                if (rdbCustomer.IsChecked == true && rdbSupplier.IsChecked == false)
                {
                    helper.CreateMultiplePayments(PaymentsList, true);
                }
                else if (rdbCustomer.IsChecked == false && rdbSupplier.IsChecked == true)
                {
                    helper.CreateMultiplePayments(PaymentsList, false);
                }
                InfoBox infobox = new InfoBox("Multiple Payments has been added successfully.");
                infobox.ShowDialog();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("something went wrong.");
                infobox.ShowDialog();
            }
        }

        private void btnToList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = 0;
                Models.DummyModels.PaymentsListModel model = new Models.DummyModels.PaymentsListModel();
                if (rdbCustomer.IsChecked == true && rdbSupplier.IsChecked == false)
                {
                    var obj = CustomerCombo.SelectedItem as Entities.Customers;
                    if (obj != null) { id = obj.Id; }
                    if (IsCredit is true)
                    {
                        model.Id = id;
                        model.Credit = float.Parse(txtAmount.Text);
                        model.Debit = 0;
                        model.Desc = txtDesc.Text;
                        model.Date = DatePicker.SelectedDate.Value.Date;
                        model.Type = "Customer";

                    }
                    else if (IsCredit is false)
                    {
                        model.Id = id;
                        model.Credit = 0;
                        model.Debit = float.Parse(txtAmount.Text);
                        model.Desc = txtDesc.Text;
                        model.Date = DatePicker.SelectedDate.Value.Date;
                        model.Type = "Customer";
                    }
                }
                else if (rdbCustomer.IsChecked == false && rdbSupplier.IsChecked == true)
                {
                    var obj = CustomerCombo.SelectedItem as Entities.Suppliers;
                    if (obj != null) { id = obj.Id; }
                    if (IsCredit is true)
                    {
                        model.Id = id;
                        model.Credit = float.Parse(txtAmount.Text);
                        model.Debit = 0;
                        model.Desc = txtDesc.Text;
                        model.Date = DatePicker.SelectedDate.Value.Date;
                        model.Type = "Supplier";
                    }
                    else if (IsCredit is false)
                    {
                        model.Id = id;
                        model.Credit = 0;
                        model.Debit = float.Parse(txtAmount.Text);
                        model.Desc = txtDesc.Text;
                        model.Date = DatePicker.SelectedDate.Value.Date;
                        model.Type = "Supplier";
                    }
                }
                PaymentsList.Add(model);
                PaymentListView.ItemsSource = PaymentsList;
                PaymentListView.Items.Refresh();
                if (IsCredit is true)
                {
                    lblTotalAmount.Content = PaymentsList.Sum(s => s.Credit).ToString("#,##0.00", usFormatProvider);
                }
                else if (IsCredit is false)
                {
                    lblTotalAmount.Content = PaymentsList.Sum(s => s.Debit).ToString("#,##0.00", usFormatProvider);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
