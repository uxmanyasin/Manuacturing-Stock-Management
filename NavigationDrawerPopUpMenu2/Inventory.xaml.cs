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

    public partial class Inventory : UserControl
    {
        private readonly ProductDatabaseHelper productDatabaseHelper = new ProductDatabaseHelper();
        private readonly InventoryDatabaseHelper inventoryDatabaseHelper = new InventoryDatabaseHelper();
        public List<Product> productslist = new List<Product>();
        public List<Entities.Inventory> inventoryList = new List<Entities.Inventory>();
        public List<Entities.Inventory> ComboinventoryList = new List<Entities.Inventory>();
        public Inventory()
        {
            InitializeComponent();
            LoadInventory();
        }

        private void BtnSearchInventory_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    ComboinventoryList.Clear();
            //    int id = Convert.ToInt32(ProductCombo.SelectedValue.ToString());
            //    ComboinventoryList = inventoryDatabaseHelper.GetSpecificInventoryList(id);
            //    InventoryListView.ItemsSource = ComboinventoryList;
            //    InventoryListView.Items.Refresh();
            //    string TProducts = InventoryListView.Items.Count.ToString();
            //    LbltotalProducts.Content = TProducts;
            //    int InQty = 0;

            //    foreach (var InList in ComboinventoryList)
            //    {
            //        InQty = InList.Quantity + InQty;
            //    }
            //    LblTotalQuantity.Content = InQty;
            //}
            //catch (Exception)
            //{
            //    InfoBox infobox = new InfoBox("Something went wrong,there is inventory to show.");
            //    infobox.ShowDialog();
            //}
        }

        private void BtnLoadInentory_Click(object sender, RoutedEventArgs e)
        {
            LoadInventory();

        }
        private void LoadInventory()
        {
            try
            {
                inventoryList = inventoryDatabaseHelper.GetInventoryList();
                InventoryListView.ItemsSource = inventoryList;
                InventoryListView.Items.Refresh();
                string TProducts = InventoryListView.Items.Count.ToString();
                LbltotalProducts.Content = TProducts;
                float InQty = 0;
                foreach (var InList in inventoryList)
                {
                    InQty = InList.Quantity + InQty;
                }
                LblTotalQuantity.Content = InQty;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there is no inventory to load.");
                infobox.ShowDialog();
            }
        }

        private void ProductCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                productslist = productDatabaseHelper.GetProducts();
                productslist.Sort((x, y) => string.Compare(x.Name, y.Name));
                ProductCombo.ItemsSource = productslist;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there is no product to load.");
                infobox.ShowDialog();
            }
        }
        private void WholeInventory_Initialized(object sender, EventArgs e)
        {
            LoadInventory();
        }

        private void btnStockIn_Click(object sender, RoutedEventArgs e)
        {
            Product_Stock_In product_Stock_In = new Product_Stock_In();
            product_Stock_In.ShowDialog();
        }

        private void ProductCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboinventoryList.Clear();
                int id = Convert.ToInt32(ProductCombo.SelectedValue.ToString());
                ComboinventoryList = inventoryDatabaseHelper.GetSpecificInventoryList(id);
                InventoryListView.ItemsSource = ComboinventoryList;
                InventoryListView.Items.Refresh();
                string TProducts = InventoryListView.Items.Count.ToString();
                LbltotalProducts.Content = TProducts;
                float InQty = 0;

                foreach (var InList in ComboinventoryList)
                {
                    InQty = InList.Quantity + InQty;
                }
                LblTotalQuantity.Content = InQty;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there is inventory to show.");
                infobox.ShowDialog();
            }
        }

        private void btnStockOut_Click(object sender, RoutedEventArgs e)
        {
            Product_StockOut product_StockOut = new Product_StockOut();
            product_StockOut.ShowDialog();
        }
    }
}
