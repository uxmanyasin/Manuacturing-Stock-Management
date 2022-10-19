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
using BizBook;
using BizBook.Reporting;
using System.Collections.ObjectModel;
using CrystalDecisions.CrystalReports.Engine;

namespace BizBook
{
   
    public partial class ViewSaleDetails : Window
    {
        private readonly Sale_DetailsDatabaseHelper sale_DetailsDatabaseHelper = new Sale_DetailsDatabaseHelper();
        private readonly SaleDatabaseHelper saleDatabaseHelper = new SaleDatabaseHelper();
        private readonly PaymentsDatabaseHelper paymentsDatabaseHelper = new PaymentsDatabaseHelper();
        private readonly DueAmountDatabaseHelper dueAmountDatabaseHelper = new DueAmountDatabaseHelper(); 
        public List<Entities.Sale_Details> Sale_DetailsList = new List<Sale_Details>();
        public ObservableCollection<SaleDetailsViewModel> saleDetails = new ObservableCollection<SaleDetailsViewModel>();

        public Entities.Sale Sale = new Entities.Sale();
        public bool update { get; set; }
        public bool walking;
        public bool CloseCheck = false;

        public ViewSaleDetails(Entities.Sale sale , bool indicater)
        {
            InitializeComponent();
            Sale = sale;
            LblTitle.Content = Sale.customers.Name;
            LblContact.Content = Sale.customers.Contact;
            LblAddress.Content = Sale.customers.Address;
            LblDate.Content = Sale.SaleDate.ToString();
            LblTotalAmount.Content = Sale.TotalPrice.ToString();
            lblGatePass.Text = "Gate Pass:" + Sale.GatePass;
            string id = Sale.SId;
            Sale_DetailsList = sale_DetailsDatabaseHelper.GetSingleSaleDetails(id).ToList();
            SalesListView.ItemsSource = Sale_DetailsList;
           
        }
        public ViewSaleDetails(Entities.Sale sale)
        {
            InitializeComponent();
            Sale = sale;
            LblTitle.Content = Sale.CustomerName;
            LblDate.Content = Sale.SaleDate.ToString();
            LblTotalAmount.Content = Sale.TotalPrice.ToString();
            string id = Sale.SId;
            lblGatePass.Text = Sale.GatePass;
            Sale_DetailsList = sale_DetailsDatabaseHelper.GetSingleSaleDetails(id).ToList();
            SalesListView.ItemsSource = Sale_DetailsList;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        { 
          this.Close();
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (walking == false) {
                try
                {

                    ObservableCollection<Reporting.SalePrintDetailsModel> _saleDetails = new ObservableCollection<SalePrintDetailsModel>();
                    foreach (var salelist_ in Sale_DetailsList)
                    {
                        Reporting.SalePrintDetailsModel model = new Reporting.SalePrintDetailsModel()
                        {
                            ProductName = salelist_.product.Name,
                            Quantity = salelist_.Quantity,
                            Total = salelist_.Total.ToString(),
                            ProductId = salelist_.ProductId,
                            Price = salelist_.Rate.ToString()
                        };
                        _saleDetails.Add(model);
                    }
                    SalesReportView salesReportView = new SalesReportView(Sale, _saleDetails);
                    salesReportView.ShowDialog();
                    ReportDocument report = new ReportDocument();
                }
                catch (Exception)
                {
                    InfoBox infobox = new InfoBox("ERROR:There is no data to generate report.");
                    infobox.ShowDialog();
                }
            }
            else if ( walking == true) 
            {
                try
                {
                    ObservableCollection<Reporting.SalePrintDetailsModel> _saleDetails = new ObservableCollection<SalePrintDetailsModel>();
                  
                    foreach (var salelist_ in Sale_DetailsList)
                    {
                        Reporting.SalePrintDetailsModel model = new Reporting.SalePrintDetailsModel()
                        {
                            ProductName = salelist_.product.Name,
                            Quantity = salelist_.Quantity,
                            Total = salelist_.Total.ToString(),
                            ProductId = salelist_.ProductId,
                            Price = salelist_.Rate.ToString()
                        };
                        _saleDetails.Add(model);
                    }
                    SalesReportView salesReportView = new SalesReportView(Sale, _saleDetails, true);
                    salesReportView.ShowDialog();
                    ReportDocument report = new ReportDocument();
                }
                catch (Exception)
                {
                    InfoBox infobox = new InfoBox("ERROR:There is no data to generate report.");
                    infobox.ShowDialog();
                }
            }
        }

        private void btnEditDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.Sale_Details sale = SalesListView.SelectedItem as Entities.Sale_Details;
                EditProduct editProduct = new EditProduct(sale);
                editProduct.ShowDialog();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There is no selected sale to show its details.");
                infobox.ShowDialog();
            }
        }

        private void btnDeleteDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.Sale_Details sale = SalesListView.SelectedItem as Entities.Sale_Details;
                sale_DetailsDatabaseHelper.DeleteSaleDetails(sale);
                SalesListView.Items.Refresh();
                UpdateAmount();
                InfoBox infobox = new InfoBox("Selected Product is deleted.");
                infobox.ShowDialog();
                string id = Sale.SId;
                Sale_DetailsList.Clear();
                Sale_DetailsList = sale_DetailsDatabaseHelper.GetSingleSaleDetails(id).ToList();
                SalesListView.ItemsSource = Sale_DetailsList;

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There is no selected sale_detail to delete.");
                infobox.ShowDialog();
            }
        }
        private void UpdateAmount() 
        {
            try
            {
                Models.Sale_DetailsDatabaseHelper helper = new Sale_DetailsDatabaseHelper();
                var _list = helper.GetDetailsBySaleID(Sale.SId);
                float amount = _list.Sum(s => s.Total);
                int totalQuantity = (int)_list.Sum(s => s.Quantity);
                int totalProducts = _list.Count();
                Sale.TotalPrice = amount;
                Sale.TotalProducts = totalProducts;
                Sale.TotalQuantity = totalQuantity;
                saleDatabaseHelper.EditSaleAmount(Sale);
                paymentsDatabaseHelper.EditSaleAmountPayment(Sale);
               
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void btnUpdateDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.Sale_DetailsDatabaseHelper helper = new Sale_DetailsDatabaseHelper();
                Sale.SaleDate = DatePicker.SelectedDate.Value.Date;
                saleDatabaseHelper.EditSaleDate(Sale);
                paymentsDatabaseHelper.EditSaleDate(Sale);
                saleDatabaseHelper.EditSaleDetailsDate(Sale);
                InfoBox infobox = new InfoBox("Sale Date is updated.");
                infobox.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnUpdateGatePass_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.Sale_DetailsDatabaseHelper helper = new Sale_DetailsDatabaseHelper();
                Sale.GatePass = lblGatePass.Text;
                saleDatabaseHelper.EditGatePass(Sale);
              
                
                InfoBox infobox = new InfoBox("Gate Pass is updated.");
                infobox.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}
