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
   
    public partial class RawMaterial : UserControl
    {
        private readonly RawMaterialDatabaseHelper helper = new RawMaterialDatabaseHelper();
        List<Entities.RawMaterial> RawMaterialList = new List<Entities.RawMaterial>();
        public RawMaterial()
        {
            InitializeComponent();
            LoadRawMaterial();
        }

        private void LoadRawMaterial() 
        {
            try 
            {
                RawMaterialList = helper.GetRawMaterial();
                List<Models.DummyModels.RawModel> rawList = new List<Models.DummyModels.RawModel>();
                int count = 1;
                foreach (var item in RawMaterialList)
                {

                    Models.DummyModels.RawModel model = new Models.DummyModels.RawModel
                    {
                        ID = item.Id,
                        No = count,
                        Name = item.Name,
                        Rate = item.Rate
                    };
                    rawList.Add(model);
                    count = count + 1;
                }
                RawMaterialListView.ItemsSource = rawList;
                RawMaterialListView.Items.Refresh();

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no Raw material to show.");
                infobox.ShowDialog();
            }
        }

        private void btnNewRawMaterial_Click(object sender, RoutedEventArgs e)
        {
            New_RawMaterial new_Raw = new New_RawMaterial();
            new_Raw.ShowDialog();
        }

        private void btnUpdateRawMaterial_Click(object sender, RoutedEventArgs e)
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
                    var raw = RawMaterialListView.SelectedItem as Models.DummyModels.RawModel;
                    var RawModel = helper.GetSingleRawMaterial(raw.ID);
                    New_RawMaterial pp = new New_RawMaterial(RawModel, true);
                    pp.ShowDialog();
                }
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,please try again.");
                infobox.ShowDialog();
            }
        }

        private void btnLoadAllRawMaterial_Click(object sender, RoutedEventArgs e)
        {
            LoadRawMaterial();
        }

        private void RawMaterialCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                RawMaterialList = helper.GetRawMaterial();
                RawMaterialList.Sort((x, y) => string.Compare(x.Name, y.Name));
                RawMaterialCombo.ItemsSource = RawMaterialList;

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no Raw material to show.");
                infobox.ShowDialog();
            }
        }

        private void btnSearchRawMaterial_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                RawMaterialList.Clear();
                var raw = RawMaterialCombo.SelectedItem as Entities.RawMaterial;
                Entities.RawMaterial rawMaterial = helper.GetSingleRawMaterial(raw.Id);
                List<Models.DummyModels.RawModel> rawList = new List<Models.DummyModels.RawModel>();
            
                int count = 1;
                Models.DummyModels.RawModel model = new Models.DummyModels.RawModel
                {
                    ID = rawMaterial.Id,
                    No = count,
                    Name = rawMaterial.Name,
                    Rate = rawMaterial.Rate
                };
                rawList.Add(model);
                RawMaterialListView.ItemsSource = rawList;
                RawMaterialListView.Items.Refresh();

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no Raw material to show.");
                infobox.ShowDialog();
            }

        }
    }
}
