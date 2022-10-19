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
    
    public partial class Expense : UserControl
    {
        private readonly PaymentsDatabaseHelper paymenthelper = new PaymentsDatabaseHelper();
        private readonly ExpenseDatabaseHelper helper = new ExpenseDatabaseHelper();
        public List<Entities.Payments> ExpenseList = new List<Entities.Payments>();

        public Expense()
        {
            InitializeComponent();

            LoadExpense();
        }

        private void Save_Expense_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var x = ((ComboBoxItem)CategoryCombo.SelectedItem).Content as string;

                Entities.Payments payments = new Payments()
                {
                    Type = "Expense",
                    Description = x,
                    Debit = Convert.ToInt32(txtAmount.Text),
                    Date = ExpenseDatePicker.SelectedDate.Value,
                    EntryDate = DateTime.Now.Date
                };
                paymenthelper.CreatePayment(payments);
                InfoBox infobox = new InfoBox("New Expense added successfully.");
                infobox.ShowDialog();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("ERROR:All Feilds are required.");
                infobox.ShowDialog();
            }

        }

        private void Load_Expense_Click(object sender, RoutedEventArgs e)
        {
            LoadExpense();
        }

        private void BtnSearchCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ExpenseList.Clear();
                var category = ((ComboBoxItem)SearchCategoryCombo.SelectedItem).Content as string;
                ExpenseList = paymenthelper.GetSpecificExpensePayments(category);
                ExpenseListView.ItemsSource = ExpenseList;
                float? TotalExpense = 0;
                foreach (var data in ExpenseList)
                {
                    TotalExpense = (data.Debit + TotalExpense);
                    LblTotalExpense.Content = TotalExpense.ToString();
                }
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong.");
                infobox.ShowDialog();
            }
        }
        private void BtnGridDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.Payments exp = ExpenseListView.SelectedItem as Entities.Payments;
                paymenthelper.DeletePayment(exp);
                InfoBox infobox = new InfoBox("Expense deleted successfully.");
                infobox.ShowDialog(); 
                LoadExpense();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there is no expense to delete.");
                infobox.ShowDialog();
            }
        }

        public void LoadExpense()
        {
            try
            {
                ExpenseList.Clear();
                ExpenseList = paymenthelper.GetExpensePayments().ToList();
                ExpenseListView.ItemsSource = ExpenseList;
                float? TotalExpense = 0;
                foreach (var data in ExpenseList)
                {


                    TotalExpense = ((float?)(data.Debit + TotalExpense));
                    LblTotalExpense.Content = TotalExpense.ToString();
                }
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there is no expense to load.");
                infobox.ShowDialog();
            }
        }
    }
}
