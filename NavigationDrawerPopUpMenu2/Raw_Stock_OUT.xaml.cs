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
   
    public partial class Raw_Stock_OUT : Window
    {
        private readonly RawStockDatabaseHelper helper = new RawStockDatabaseHelper();
        private readonly RawMaterialDatabaseHelper rawMaterialDatabaseHelper = new RawMaterialDatabaseHelper();
        List<Entities.RawMaterial> rawMaterialsList = new List<Entities.RawMaterial>();
        Entities.RawStock stock;
        public Raw_Stock_OUT()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      

        private void RawMaterialCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                rawMaterialsList = rawMaterialDatabaseHelper.GetRawMaterial();
                rawMaterialsList.Sort((x, y) => string.Compare(x.Name, y.Name));
                RawMaterialCombo.ItemsSource = rawMaterialsList;
                RawMaterialCombo.Items.Refresh();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no raw materials  to show.");

                infobox.ShowDialog();
            }
        }

        private void btnSaveRawStockOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(RawMaterialCombo.SelectedValue.ToString());
                stock = helper.GetSingleRawStock(id);
                if (stock == null)
                {
                    InfoBox infobox = new InfoBox("There is no raw material to stock-out.");
                    infobox.ShowDialog();
                    this.Close();
                }
                else
                {
                    int qty = Convert.ToInt32(txtQuantity.Text);
                    float sum = (stock.Quantity - qty);
                    stock.Quantity = sum;
                    helper.RawStock_In(stock);
                    InfoBox infobox = new InfoBox(stock.Quantity + " updated succesfully.");
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
