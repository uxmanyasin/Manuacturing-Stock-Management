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

    public partial class New_Product : Window
    {
        private readonly ProductDatabaseHelper helper = new ProductDatabaseHelper();
        private readonly CategoryDatabaseHelper categoryDatabaseHelper = new CategoryDatabaseHelper();
        List<Entities.Category> categoryList = new List<Category>();
        private Product Product;
        public bool update { get; set; }
        public New_Product()
        {
            InitializeComponent();
        }
        public New_Product(Product product,bool indicater)
        {
            InitializeComponent();
            update = indicater;
            Product = product;
            txtName.Text = Product.Name;
            lblCategory.Text = product.category.Name;
            txtPrice.Text = Product.Rate.ToString();
            lblTitle.Text = "Update Product";
            btnSaveProduct.Content = "Update";
        }

        private void BtnNewProduct_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (update == true)
                {
                    if (CategoryCombo.SelectedItem != null)
                    {

                        Product.Name = txtName.Text;
                        Product.Rate = (float)Convert.ToInt32(txtPrice.Text);
                        Product.CategoryId = Convert.ToInt32(CategoryCombo.SelectedValue.ToString());
                        helper.EditProduct(Product);
                        InfoBox infobox = new InfoBox(Product.Name + " updated succesfully.");
                        infobox.ShowDialog();
                    }
                    else 
                    {
                        Product.Name = txtName.Text;
                        Product.Rate = (float)Convert.ToInt32(txtPrice.Text);
                        helper.EditProduct(Product);
                        InfoBox infobox = new InfoBox(Product.Name + " updated succesfully.");
                        infobox.ShowDialog();

                    }
                }
                else
                {
                    Product = new Product()
                    {
                        Name = txtName.Text,
                        Rate = float.Parse(txtPrice.Text),
                        CategoryId = Convert.ToInt32(CategoryCombo.SelectedValue.ToString())
                        
                    };
                    helper.CreateProduct(Product);
                    InfoBox infobox = new InfoBox(Product.Name + " created succesfully.");
                    infobox.ShowDialog();
                    Product_Stock_In _In = new Product_Stock_In();
                    _In.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception )
            {

                InfoBox infobox = new InfoBox("ERROR:All Feilds are required.");
                infobox.ShowDialog();
            }
        }

        private void btnNewCategory_Click(object sender, RoutedEventArgs e)
        {
            New_Category new_Category = new New_Category();
            new_Category.ShowDialog();
        }

        private void CategoryCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                categoryList = categoryDatabaseHelper.GetCategories();
                categoryList.Sort((x, y) => string.Compare(x.Name, y.Name));
                CategoryCombo.ItemsSource = categoryList;
                CategoryCombo.Items.Refresh();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no categories to show.");
                infobox.ShowDialog();
            }
        }
    }
}
