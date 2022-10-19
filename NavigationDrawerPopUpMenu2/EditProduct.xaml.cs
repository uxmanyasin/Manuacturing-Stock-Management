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
    
    public partial class EditProduct : Window
    {
        private readonly ProductDatabaseHelper productDatabaseHelper = new ProductDatabaseHelper();
        public List<Product> productlist = new List<Product>();
        public Entities.Sale_Details _Sale_Details = new Sale_Details();
        public Entities.CustomerClaimDetails _Details = new CustomerClaimDetails();
        private readonly Models.CustomerClaimsDatabaseHelper customerClaimsDatabaseHelper = new CustomerClaimsDatabaseHelper();
        private readonly Sale_DetailsDatabaseHelper sale_DetailsDatabaseHelper = new Sale_DetailsDatabaseHelper();
        public bool _indicator;
        public EditProduct(Entities.Sale_Details sale_Details)
        {
            InitializeComponent();
            _Sale_Details = sale_Details;
            lblProduct.Text = _Sale_Details.product.Name;
            txtQuantity.Text = _Sale_Details.Quantity.ToString();
            txtRate.Text = _Sale_Details.Rate.ToString();
        }

        public EditProduct(Entities.CustomerClaimDetails Details, bool indicator )
        {
            InitializeComponent();
            _Details = Details;
            _indicator = indicator;
            lblProduct.Text = _Details.product.Name;
            txtQuantity.Text = _Details.Quantity.ToString();
            txtRate.Text = _Details.Rate.ToString();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();

        }

        private void btnSaveCashOut_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void btnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                if(_indicator == true) 
                {
                    if (ProductCombo.SelectedItem !=null)
                    {
                        Entities.CustomerClaimDetails Claim_Details = new CustomerClaimDetails
                        {
                            Id = _Details.Id,
                            CustomerClaimId = _Details.CustomerClaimId,
                            Rate = float.Parse(txtRate.Text),
                            Quantity = Convert.ToInt32(txtQuantity.Text),
                            Total = (float.Parse(txtQuantity.Text)) * (float.Parse(txtRate.Text)),
                            ProductId = Convert.ToInt32(ProductCombo.SelectedValue.ToString())
                        };
                        customerClaimsDatabaseHelper.EditDetails(Claim_Details);
                        InfoBox infobox = new InfoBox("Return Product edited.");
                        infobox.ShowDialog();
                        this.Close();
                    }
                    else 
                    {
                        Entities.CustomerClaimDetails Claim_Details = new CustomerClaimDetails
                        {
                            Id = _Details.Id,
                            CustomerClaimId = _Details.CustomerClaimId,
                            Rate = float.Parse(txtRate.Text),
                            Quantity = Convert.ToInt32(txtQuantity.Text),
                            Total = (float.Parse(txtQuantity.Text)) * (float.Parse(txtRate.Text)),
                            ProductId = _Details.ProductId
                        };
                        customerClaimsDatabaseHelper.EditDetails(Claim_Details);
                        InfoBox infobox = new InfoBox("Return Product edited.");
                        infobox.ShowDialog();
                        this.Close();
                    }
                    UpdateClaimAmount();
                }
                else 
                {
                    if (ProductCombo.SelectedItem != null)
                    {
                        Entities.Sale_Details Editsale_Details = new Sale_Details
                        {
                            Id = _Sale_Details.Id,
                            SaleDate = _Sale_Details.SaleDate,
                            SaleId = _Sale_Details.SaleId,
                            Rate = float.Parse(txtRate.Text),
                            Quantity = Convert.ToInt32(txtQuantity.Text),
                            Total = (float.Parse(txtQuantity.Text)) * (float.Parse(txtRate.Text)),
                            ProductId = Convert.ToInt32(ProductCombo.SelectedValue.ToString())
                        };
                        sale_DetailsDatabaseHelper.EditSaleDetails(Editsale_Details);
                        InfoBox infobox = new InfoBox("Product Edited.");
                        infobox.ShowDialog();
                        this.Close();
                    }
                    else 
                    {
                        Entities.Sale_Details Editsale_Details = new Sale_Details
                        {
                            Id = _Sale_Details.Id,
                            SaleDate = _Sale_Details.SaleDate,
                            SaleId = _Sale_Details.SaleId,
                            Rate = float.Parse(txtRate.Text),
                            Quantity = Convert.ToInt32(txtQuantity.Text),
                            Total = (float.Parse(txtQuantity.Text)) * (float.Parse(txtRate.Text)),
                            ProductId = _Sale_Details.ProductId
                        };
                        sale_DetailsDatabaseHelper.EditSaleDetails(Editsale_Details);
                        InfoBox infobox = new InfoBox("Product Edited.");
                        infobox.ShowDialog();
                        this.Close();
                    }
                    UpdateSaleAmount();
                }

            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Error: All feilds are required.");
                infobox.ShowDialog();
            }
        }
        private void UpdateSaleAmount()
        {
            try
            {
                Models.Sale_DetailsDatabaseHelper helper = new Sale_DetailsDatabaseHelper();
                var _list = helper.GetDetailsBySaleID(_Sale_Details.SaleId);
                float amount = _list.Sum(s => s.Total);
                int totalQuantity = (int)_list.Sum(s => s.Quantity);
                int totalProducts = _list.Count();
                Models.SaleDatabaseHelper saleDatabaseHelper = new SaleDatabaseHelper();
                var sale = saleDatabaseHelper.GetSale(_Sale_Details.SaleId);
                sale.TotalQuantity = totalQuantity;
                sale.TotalProducts = totalProducts;
                sale.TotalPrice = amount;
                helper.UpdateSaleAmount(sale);
               

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void UpdateClaimAmount()
        {
            try
            {
                Models.CustomerClaimsDatabaseHelper helper = new Models.CustomerClaimsDatabaseHelper();
                var _list = helper.GetClaimDetails(_Details.CustomerClaimId);
                float amount = _list.Sum(s => s.Total);
                var claim = helper.GetSingleClaims(_Details.CustomerClaimId);
                claim.TotalPrice = amount;
                helper.UpdateClaimAmount(claim);
                InfoBox infobox = new InfoBox("Claim Amount Updated.");
                infobox.ShowDialog();

            }
            catch (Exception ex)
            {

                InfoBox infobox = new InfoBox(ex.ToString());
                infobox.ShowDialog();
            }
        }

        private void ProductCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                productlist = productDatabaseHelper.GetProducts().ToList();
                productlist.Sort((x, y) => string.Compare(x.Name, y.Name));
                ProductCombo.ItemsSource = productlist;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no products to show.");
                infobox.ShowDialog();
            }
        }
    }
}
