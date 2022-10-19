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
    
    public partial class Products : UserControl
    {
        private readonly ProductDatabaseHelper helper = new ProductDatabaseHelper();
        public List<Product> productlist = new List<Product>();
        public List<Product> productlistCombo = new List<Product>();
        public Products()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void BtnNewProduct_Click(object sender, RoutedEventArgs e)
        {
            New_Product n = new New_Product();
            n.ShowDialog();
        }

        private void BtnLoadAllProduct_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void BtnSearchProducts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Models.ProductModel> productList = new List<ProductModel>();
                productlist.Clear();
                var _product = ProductCombo.SelectedItem as Entities.Product;
                Entities.Product product = helper.GetProduct(_product.Id);

                int count = 1;
                Models.ProductModel model = new ProductModel
                {
                        ID = product.Id,
                        No = count,
                        Name = product.Name,
                        Category = product.category.Name,
                        Rate = product.Rate
                 };
                productList.Add(model);
                ProductListView.ItemsSource = productList;
                ProductListView.Items.Refresh();
            }
                
            
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no products to show.");
                infobox.ShowDialog();
            }

        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Properties.Configuration.Default.AccessMode == true)
                {
                    InfoBox infobox = new InfoBox("You are not authorized for this action.");
                    infobox.ShowDialog();
                }
                else
                {
                    var product = ProductListView.SelectedItem as Models.ProductModel;
                    Entities.Product _product = helper.GetProduct(product.ID);
                    New_Product pp = new New_Product(_product, true);
                    pp.ShowDialog();
                }
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,please try again.");
                infobox.ShowDialog();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnLoadAllProducts_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                List<Models.ProductModel> productList = new List<ProductModel>();
                productlist.Clear();
                productlist = helper.GetProducts().ToList();
                int count = 1;
                foreach (var item in productlist)
                {
                    
                    Models.ProductModel model = new ProductModel 
                    {
                        ID = item.Id,
                        No = count,
                        Name = item.Name,
                        Category = item.category.Name,
                        Rate = item.Rate
                    };
                    productList.Add(model);
                    count = count+1;
                }
                ProductListView.ItemsSource = productList;
                ProductListView.Items.Refresh();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no products to show.");
                infobox.ShowDialog();
            }
        }
        private void ProductCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                productlistCombo = helper.GetProducts().ToList();
                productlistCombo.Sort((x, y) => string.Compare(x.Name, y.Name));
                ProductCombo.ItemsSource = productlistCombo;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no products to show.");
                infobox.ShowDialog();
            }
        }
    }
}
