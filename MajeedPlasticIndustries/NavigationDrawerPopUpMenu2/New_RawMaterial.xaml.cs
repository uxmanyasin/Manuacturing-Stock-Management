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
   
    public partial class New_RawMaterial : Window
    {
        private readonly RawMaterialDatabaseHelper helper = new RawMaterialDatabaseHelper();
       
        private Entities.RawMaterial _RawMaterial;
        public bool update { get; set; }
        public New_RawMaterial()
        {
            InitializeComponent();
        }

        public New_RawMaterial(Entities.RawMaterial rawMaterial, bool indicator)
        {
            InitializeComponent();
            _RawMaterial = rawMaterial;
            txtName.Text = _RawMaterial.Name;
            txtRate.Text = _RawMaterial.Rate.ToString();
            update = indicator;
            lblTitle.Text = "Update Raw Material";
            btnSaveRawMaterial.Content = "Update";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSaveRawMaterial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (update == true)
                {

                    _RawMaterial.Name = txtName.Text;
                    _RawMaterial.Rate = float.Parse(txtRate.Text);
                    helper.EditRawMaterial(_RawMaterial);
                    InfoBox info = new InfoBox(_RawMaterial.Name + " updated succesfully.");
                    info.ShowDialog();
                }
                else
                {
                    _RawMaterial = new Entities.RawMaterial()
                    {
                        Name = txtName.Text,
                        Rate = float.Parse(txtRate.Text)
                        
                    };
                    helper.CreateRawMaterial(_RawMaterial);
                    InfoBox infobox = new InfoBox(_RawMaterial.Name + " created succesfully.");
                    infobox.ShowDialog();
                    Raw_Stock_IN raw_Stock_IN = new Raw_Stock_IN();
                    raw_Stock_IN.ShowDialog();
                    this.Close();
                }
            }

            catch (Exception)
            {
                InfoBox infobox = new InfoBox("ERROR:All Feilds are required.");
                infobox.ShowDialog();
            }
        }
    }
}
