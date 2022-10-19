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
 
    public partial class RawStock : UserControl
    {
        private readonly RawStockDatabaseHelper rawStockDatabaseHelper = new RawStockDatabaseHelper();
        private readonly RawMaterialDatabaseHelper helper = new RawMaterialDatabaseHelper();
        List<Entities.RawMaterial> rawMaterials = new List<Entities.RawMaterial>();
        List<Entities.RawStock> rawList = new List<Entities.RawStock>();
        public RawStock()
        {
            InitializeComponent();
            GetRawStock();
        }
        private void GetRawStock() 
        {
            try
            {
                rawList.Clear();
                rawList = rawStockDatabaseHelper.GetRawStocks();
                InventoryListView.ItemsSource = rawList;
                InventoryListView.Items.Refresh();
                int totalqty = rawList.Count;
                LbltotalProducts.Content = totalqty.ToString();
                float totalquantity = rawList.Sum(s => s.Quantity);
                LblTotalQuantity.Content = totalquantity;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no raw in stocks to show.");
                infobox.ShowDialog();
            }
        }
        private void btnStockIn_Click(object sender, RoutedEventArgs e)
        {
            Raw_Stock_IN raw_Stock_IN = new Raw_Stock_IN();
            raw_Stock_IN.ShowDialog();
        }

        private void btnStockOut_Click(object sender, RoutedEventArgs e)
        {
            Raw_Stock_OUT raw_Stock_OUT = new Raw_Stock_OUT();
            raw_Stock_OUT.ShowDialog();
        }

        private void BtnLoadInentory_Click(object sender, RoutedEventArgs e)
        {
            GetRawStock();
        }

        private void RawCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                rawMaterials.Clear();
                rawMaterials.Sort((x, y) => string.Compare(x.Name, y.Name));
                RawCombo.ItemsSource = helper.GetRawMaterial();
               
                RawCombo.Items.Refresh();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no raw materials to show.");
                infobox.ShowDialog();
            }
        }

        private void RawCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int id =Convert.ToInt32(RawCombo.SelectedValue.ToString());
                rawList.Clear();
                Entities.RawStock rawStock = rawStockDatabaseHelper.GetSingleRawStock(id);
                rawList.Add(rawStock);
                InventoryListView.ItemsSource = rawList;
                InventoryListView.Items.Refresh();
                float totalquantity = rawList.Sum(s => s.Quantity);
                LblTotalQuantity.Content = totalquantity;
                int totalqty = rawList.Count;
                LbltotalProducts.Content = totalqty.ToString();
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no raw in stocks to show.");

                infobox.ShowDialog();
            }
        }
    }
}
