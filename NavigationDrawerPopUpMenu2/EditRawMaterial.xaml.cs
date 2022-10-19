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

namespace BizBook
{
   
    public partial class EditRawMaterial : Window
    {
        private readonly Models.PurchaseDatabaseHelper purchaseDatabaseHelper = new Models.PurchaseDatabaseHelper();
        private readonly Models.RawMaterialDatabaseHelper rawMaterialDatabaseHelper = new Models.RawMaterialDatabaseHelper();
        private readonly Models.SupplierClaimsDatabaseHelper supplierClaimsDatabaseHelper = new Models.SupplierClaimsDatabaseHelper();
        List<Entities.RawMaterial> rawMaterialsList = new List<Entities.RawMaterial>();
        public Entities.Purchase_Details _Details;
        public Entities.SupplierClaimDetails SupplierClaimDetails = new Entities.SupplierClaimDetails();
        public bool indicater;

        public EditRawMaterial(Entities.Purchase_Details purchase_Details)
        {
            InitializeComponent();
            _Details = purchase_Details;
            txtQuantity.Text = _Details.Quantity.ToString();
            lblRaw.Text = _Details.rawmaterial.Name;
            txtRate.Text = _Details.Rate.ToString();
        }

        public EditRawMaterial(Entities.SupplierClaimDetails Details,bool indicate)
        {
            InitializeComponent();
            SupplierClaimDetails = Details;
            indicater = indicate;
            lblRaw.Text = SupplierClaimDetails.RawMaterial.Name;
            txtQuantity.Text = SupplierClaimDetails.Quantity.ToString();
            txtRate.Text = SupplierClaimDetails.Rate.ToString();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RawMaterialCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                rawMaterialsList.Clear();
                rawMaterialsList = rawMaterialDatabaseHelper.GetRawMaterial();
                rawMaterialsList.Sort((x, y) => string.Compare(x.Name, y.Name));
                RawMaterialCombo.ItemsSource = rawMaterialsList;
                RawMaterialCombo.Items.Refresh();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnEditRawMaterial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (indicater == true) 
                {
                    if (RawMaterialCombo.SelectedItem != null) 
                    {
                        var raw = RawMaterialCombo.SelectedItem as Entities.RawMaterial;
                        SupplierClaimDetails.RawId = raw.Id;
                        SupplierClaimDetails.Id = SupplierClaimDetails.Id;
                        SupplierClaimDetails.Quantity = Convert.ToInt32(txtQuantity.Text);
                        SupplierClaimDetails.Rate = float.Parse(txtRate.Text);
                        SupplierClaimDetails.Total = (float.Parse(txtRate.Text) * float.Parse(txtQuantity.Text));
                        supplierClaimsDatabaseHelper.EditDetails(SupplierClaimDetails);
                        InfoBox infobox = new InfoBox("Return Raw Material edited.");
                        infobox.ShowDialog();
                        this.Close();
                    }
                    else 
                    {
                        SupplierClaimDetails.RawId = SupplierClaimDetails.RawId;
                        SupplierClaimDetails.Id = SupplierClaimDetails.Id;
                        SupplierClaimDetails.Quantity = Convert.ToInt32(txtQuantity.Text);
                        SupplierClaimDetails.Rate = float.Parse(txtRate.Text);
                        SupplierClaimDetails.Total = (float.Parse(txtRate.Text) * float.Parse(txtQuantity.Text));
                        supplierClaimsDatabaseHelper.EditDetails(SupplierClaimDetails);
                        InfoBox infobox = new InfoBox("Return Raw Material edited.");
                        infobox.ShowDialog();
                        this.Close();
                    }
                }
                else 
                {
                    if (RawMaterialCombo.SelectedItem != null) 
                    {
                        var raw = RawMaterialCombo.SelectedItem as Entities.RawMaterial;
                        _Details.RawId = raw.Id;
                        _Details.Quantity = Convert.ToInt32(txtQuantity.Text);
                        _Details.Rate = float.Parse(txtRate.Text);
                        _Details.Total = (float.Parse(txtRate.Text) * float.Parse(txtQuantity.Text));
                        purchaseDatabaseHelper.EditPurchaseDetail(_Details);
                        InfoBox infobox = new InfoBox("Raw Material edited.");
                        infobox.ShowDialog();
                        this.Close();
                    }
                    else 
                    {
                        _Details.RawId = _Details.RawId;
                        _Details.Quantity = Convert.ToInt32(txtQuantity.Text);
                        _Details.Rate = float.Parse(txtRate.Text);
                        _Details.Total = (float.Parse(txtRate.Text) * float.Parse(txtQuantity.Text));
                        purchaseDatabaseHelper.EditPurchaseDetail(_Details);
                        InfoBox infobox = new InfoBox("Raw Material edited.");
                        infobox.ShowDialog();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                InfoBox infobox = new InfoBox("Error: All feilds are required.");
                infobox.ShowDialog();
            }
        }
    }
}
