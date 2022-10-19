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
using BizBook.Models;

namespace BizBook
{
    
    public partial class Product_Stock_In : Window
    {
        private readonly ProductDatabaseHelper productDatabaseHelper = new ProductDatabaseHelper();
        private readonly InventoryDatabaseHelper helper = new InventoryDatabaseHelper();
        List<Entities.Product> productList = new List<Entities.Product>();
        Entities.Inventory inventory = new Entities.Inventory();
        public Product_Stock_In()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ProductCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                productList.Clear();
                productList = productDatabaseHelper.GetProducts();
                productList.Sort((x, y) => string.Compare(x.Name, y.Name));
                ProductCombo.ItemsSource = productList;
                ProductCombo.Items.Refresh();
              
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Error: There are no products to show.");
                infobox.ShowDialog();
            }
        }

        private void btnProductStockIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                int id = Convert.ToInt32(ProductCombo.SelectedValue.ToString());
                inventory = helper.GetSpecificInventory(id);
                if (inventory == null)
                {
                    inventory = new Entities.Inventory()
                    {
                        ProductId = Convert.ToInt32(ProductCombo.SelectedValue.ToString()),
                        Quantity = Convert.ToInt32(txtQty.Text)

                    };
                    helper.CreateInventory(inventory);
                    InfoBox infobox = new InfoBox( inventory.Quantity + " inserted succesfully.");
                    infobox.ShowDialog();
                    this.Close();
                }
                else
                {
                    int qty = Convert.ToInt32(txtQty.Text);
                    float sum = (inventory.Quantity + qty);
                    inventory.Quantity = sum;
                    helper.ProductStock_In(inventory);
                    InfoBox infobox = new InfoBox(inventory.Quantity + " updated succesfully.");
                    infobox.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,you are out of stock for this quantity.");

                infobox.ShowDialog();
                this.Close();
            }
        }
    }
}
