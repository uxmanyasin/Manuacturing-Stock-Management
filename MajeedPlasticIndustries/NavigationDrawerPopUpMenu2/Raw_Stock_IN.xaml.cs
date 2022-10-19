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
  
    public partial class Raw_Stock_IN : Window
    {
        private readonly RawMaterialDatabaseHelper rawMaterialDatabaseHelper = new RawMaterialDatabaseHelper();
        private readonly RawStockDatabaseHelper helper = new RawStockDatabaseHelper();
        List<Entities.RawMaterial> RawMaterialsList = new List<Entities.RawMaterial>();
        Entities.RawStock stock;

        public Raw_Stock_IN()
        {
            InitializeComponent();
        }

      

        private void btnClose_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RawCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                RawMaterialsList = rawMaterialDatabaseHelper.GetRawMaterial();
                RawMaterialsList.Sort((x, y) => string.Compare(x.Name, y.Name));
                RawCombo.ItemsSource = RawMaterialsList;
                RawCombo.Items.Refresh();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no raw materials  to show.");

                infobox.ShowDialog();
            }
        }

        private void btnSaveStockIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                int id = Convert.ToInt32(RawCombo.SelectedValue.ToString());
                stock = helper.GetSingleRawStock(id);
                if (stock == null)
                {
                    stock = new Entities.RawStock()
                    {
                        RawId = Convert.ToInt32(RawCombo.SelectedValue.ToString()),
                        Quantity = Convert.ToInt32(txtQty.Text)

                    };
                    helper.CreateRawStock(stock);
                    InfoBox infobox = new InfoBox(stock.Quantity + " inserted succesfully.");
                    infobox.ShowDialog();
                    this.Close();
                }
                else
                {
                    int qty = Convert.ToInt32(txtQty.Text);
                    float sum = (stock.Quantity + qty);
                    stock.Quantity = sum;
                    helper.RawStock_In(stock);
                    InfoBox infobox = new InfoBox(stock.Quantity + " updated succesfully.");
                    infobox.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception )
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no raw materials  to show.");
                infobox.ShowDialog();
                this.Close();
            }
        }
    }
}
