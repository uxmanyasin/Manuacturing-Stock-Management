using BizBook.Entities;
using BizBook.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
   
    public partial class Purchase : UserControl
    {
        private readonly SupplierDatabaseHelper supplierDatabaseHelper = new SupplierDatabaseHelper();
        private readonly RawMaterialDatabaseHelper rawMaterialDatabaseHelper = new RawMaterialDatabaseHelper();
        private readonly PurchaseDatabaseHelper purchaseDatabaseHelper = new PurchaseDatabaseHelper();
        //  private readonly InventoryDatabaseHelper inventoryDatabaseHelper = new InventoryDatabaseHelper();
        private readonly SupplierRateDatabaseHelper supplierRateDatabaseHelper = new SupplierRateDatabaseHelper();
        public List<Entities.Suppliers> suppliersList = new List<Suppliers>();
        public List<Entities.RawMaterial> rawMaterialList = new List<Entities.RawMaterial>();
        public List<Entities.Purchase> purchaseList = new List<Entities.Purchase>();
        
        public ObservableCollection<SaleDetailsViewModel> SaleDetailsList = new ObservableCollection<SaleDetailsViewModel>();
        public Purchase()
        {
            InitializeComponent();
            bool Access = Properties.Configuration.Default.AccessMode;
            if (Access == true)
            {
                PurchaseReport.IsEnabled = false;
            }
        }
        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            New_RawMaterial n = new New_RawMaterial();
            n.ShowDialog();
        }

        private void BtnAddSupplier_Click(object sender, RoutedEventArgs e)
        {
            New_Supplier s = new New_Supplier();
            s.ShowDialog();
        }

        private void SupplierCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                suppliersList = supplierDatabaseHelper.GetSuppliers().ToList();
                suppliersList.Sort((x, y) => string.Compare(x.Name, y.Name));
                SupplierCombo.ItemsSource = suppliersList;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no suppliers to show.");
                infobox.ShowDialog();
            }
        }

        private void ProductCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                rawMaterialList = rawMaterialDatabaseHelper.GetRawMaterial().ToList();
                rawMaterialList.Sort((x, y) => string.Compare(x.Name, y.Name));
                ProductCombo.ItemsSource = rawMaterialList;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no products to show.");
                infobox.ShowDialog();
            }
        }

        private void ProductCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(ProductCombo.SelectedValue.ToString());
                Entities.RawMaterial rawMaterial = rawMaterialDatabaseHelper.GetSingleRawMaterial(id);
               
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Error:Please select raw material first.");
                infobox.ShowDialog();
            }
        }

        private void TxtQty_TextChanged(object sender, TextChangedEventArgs e)
        {
            //try
            //{
            //    int rate = Convert.ToInt32(txtPrice.Text.ToString());
            //    int qty = 0;
            //    qty = Convert.ToInt32(TxtQty.Text);
            //    int ans = (rate * qty);
            //    LblTotalAmount.Content = ans;
            //}
            //catch (Exception)
            //{
            //    InfoBox infobox = new InfoBox("Please enter quantity greater than 0.");
            //    infobox.ShowDialog();
            //}
        }

        private void SupplierCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void BtnToList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.RawMaterial p = ProductCombo.SelectedItem as Entities.RawMaterial;
                var pp = SaleDetailsList.Where(s => s.ProductId == p.Id).FirstOrDefault();
                if (pp == null)
                {
                    SaleDetailsViewModel model = new SaleDetailsViewModel()
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        Quantity = float.Parse(TxtQty.Text),
                        Total = (float.Parse(TxtQty.Text) * float.Parse(txtPrice.Text)),
                        Price = float.Parse(txtPrice.Text)
                    };
                    SaleDetailsList.Add(model);
                }
                else
                {
                    pp.Quantity = pp.Quantity + Convert.ToInt32(TxtQty.Text);
                    pp.Total = pp.Total + (float.Parse(TxtQty.Text) * float.Parse(txtPrice.Text));
                }

                if (SupplierCombo.SelectedItem != null)
                {
                    var supplier = SupplierCombo.SelectedItem as Entities.Suppliers;
                    Entities.SupplierRate supplierRate = supplierRateDatabaseHelper.GetSupplierRate(p.Id, supplier.Id);
                    float rate =float.Parse(txtPrice.Text);
                    if (rate > 0)
                    {
                        if (supplierRate == null)
                        {
                            Entities.SupplierRate model = new Entities.SupplierRate
                            {
                                SupplierId = supplier.Id,
                                RawId = p.Id,
                                Rate = float.Parse(txtPrice.Text)
                            };
                            supplierRateDatabaseHelper.CreateSupplierRate(model);

                        }
                        else
                        {
                            Entities.SupplierRate Editmodel = new Entities.SupplierRate
                            {
                                SupplierId = supplier.Id,
                                RawId = p.Id,
                                Rate =float.Parse(txtPrice.Text)
                            };

                            supplierRateDatabaseHelper.EditSupplierRate(Editmodel);
                        }
                    }
                }
                ProductCartGrid.ItemsSource = SaleDetailsList;
                ProductCartGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //InfoBox infobox = new InfoBox("Error:Please select product and enter quantity to add it your list.");
                //infobox.ShowDialog();
            }
        }

        private void PurchaseDone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                float totalQuantity = SaleDetailsList.Sum(s => s.Quantity);
                float totalAmount = SaleDetailsList.Sum(s => s.Total);
                float totalProducts = SaleDetailsList.Count;

                LblTotaluantity.Content = totalQuantity.ToString();
                LblTAmount.Content = totalAmount.ToString();
                LblTotalProducts.Content = totalProducts.ToString();
               
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Error:There is no product in product list.Minimum 1 product is required.");
                infobox.ShowDialog();
            }
        }

        private void BtnSavePurchase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(SupplierCombo.SelectedValue.ToString());
                string guid = System.Guid.NewGuid().ToString();
                guid = guid.Substring(0, 5);
                float amount = float.Parse(LblTAmount.Content.ToString());
                float freight = 0;
                Entities.Purchase purchase = new Entities.Purchase()
                {
                    PId = guid,

                    TotalProducts = Convert.ToInt32(LblTotalProducts.Content.ToString()),
                    TotalPrice = amount + freight,
                    TotalQuantity = Convert.ToInt32(LblTotaluantity.Content.ToString()),
                    PurchaseDate = PurchaseDatePicker.SelectedDate.Value,
                    EntryDate = DateTime.Now.Date,
                    Status = true,
                    GatePass = txtGatePass.Text,
                    SupplierId = id,
                    Cash = 0,
                    Freight = 0,
                    PurchaseAmount = float.Parse(LblTAmount.Content.ToString())
                  
                };
                purchaseDatabaseHelper.CreatePurchase(purchase, SaleDetailsList);
                InfoBox infobox = new InfoBox("New Purchase created successfully.");
                SaleDetailsList.Clear();
                infobox.ShowDialog();
              
                txtPrice.Text = "";
                //txtCash.Text = "";
                //txtFreight.Text = "";
                TxtQty.Text = "";
                LblAmount.Content = "";
                LblTAmount.Content = "";
                LblTotalProducts.Content = "";
                LblTotaluantity.Content = "";
                ProductCombo.SelectedItem = null;
                ProductCartGrid.ItemsSource = null;
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.ToString());
                InfoBox infobox = new InfoBox("ERROR:All Feilds are required.");
                infobox.ShowDialog();
            }

        }

        private void BtnLoadAll_Click(object sender, RoutedEventArgs e)
        {
            LoadPurchases();
        }

        private void SupplierComboSearch_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                suppliersList = supplierDatabaseHelper.GetSuppliers().ToList();
                SupplierComboSearch.ItemsSource = suppliersList;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no suppliers to show.");
                infobox.ShowDialog();
            }
        }

        private void SearchSupplierPurchase_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void SearchByDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                purchaseList.Clear();
                DateTime date = Datepicker.SelectedDate.Value.Date;
                purchaseList = purchaseDatabaseHelper.GetDatedPurchase(date).ToList();
                purchaseList.Sort((x, y) => x.PurchaseDate.CompareTo(y.PurchaseDate));
                PurchaseListView.ItemsSource = purchaseList;
                LblTotalPurchases.Content = purchaseList.Count;
                float TAmount = purchaseList.Sum(s => s.TotalPrice);
                LblAmount.Content = TAmount.ToString();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no purchases to show.");
                infobox.ShowDialog();
            }
        }

        private void PurchaseReport_Initialized(object sender, EventArgs e)
        {
            
        }

        private void LoadPurchases()
        {
            try
            {
                purchaseList.Clear();
                purchaseList = purchaseDatabaseHelper.GetPurchases().ToList();
                PurchaseListView.ItemsSource = purchaseList;
                LblTotalPurchases.Content = purchaseList.Count;
                float TAmount = purchaseList.Sum(s => s.TotalPrice);
                LblAmount.Content = TAmount.ToString();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no purchases to show.");
                infobox.ShowDialog();
            }
        }

        private void btnGridDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Entities.Purchase purchase = PurchaseListView.SelectedItem as Entities.Purchase;
                ViewPurchaseDetails vp = new ViewPurchaseDetails(purchase);
                vp.ShowDialog();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there is no selected purchase to show.");
                infobox.ShowDialog();
            }
        }

        private void SearchDateBetween_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                purchaseList.Clear();
                DateTime datefrom = datepickerFrom.SelectedDate.Value.Date;
                DateTime dateto = datepickerTo.SelectedDate.Value.Date;
               
                purchaseList = purchaseDatabaseHelper.GetDateBetweenPurchase(datefrom,dateto);
                purchaseList.Sort((x, y) => x.PurchaseDate.CompareTo(y.PurchaseDate));
                PurchaseListView.ItemsSource = purchaseList;
                PurchaseListView.Items.Refresh();
                LblTotalPurchases.Content = purchaseList.Count;
                float TAmount = purchaseList.Sum(s => s.TotalPrice);
                LblAmount.Content = TAmount.ToString();

            }
            catch (Exception)
            {
                Info_Dialogue infobox = new Info_Dialogue("Something went wrong,there are no purchases to show.");
                infobox.ShowDialog();
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaleDetailsViewModel model = ProductCartGrid.SelectedItem as SaleDetailsViewModel;
                SaleDetailsList.Remove(model);
                txtPrice.Text = "0";
              
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("There is no selected product to remove.");
                infobox.ShowDialog();
            }
        }

        private void ProductCombo_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ProductCombo.ItemsSource != null && ProductCombo.SelectedItem != null)
                {
                    var product = ProductCombo.SelectedItem as Entities.RawMaterial;
                    if (SupplierCombo.SelectedItem != null)
                    {
                        var supplier = SupplierCombo.SelectedItem as Entities.Suppliers;

                        Entities.SupplierRate model = supplierRateDatabaseHelper.GetSupplierRate(product.Id,supplier.Id);
                        if (model != null) 
                        {
                            txtPrice.Text = model.Rate.ToString();

                        }
                        else
                        {
                            txtPrice.Text = product.Rate.ToString();
                        }
                    }

                   
                }
               
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong...");
                infobox.ShowDialog();
            }
        }

        private void btnDeletePurhase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var purhase = PurchaseListView.SelectedItem as Entities.Purchase;
                purchaseDatabaseHelper.DeletePurchase(purhase);
                InfoBox infobox = new InfoBox("Purchase Deleted Successfully...");
                infobox.ShowDialog();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong...");
                infobox.ShowDialog();
            }
        }

        private void SpecificSearchDateBetween_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                purchaseList.Clear();
                DateTime datefrom = datepickerFrom.SelectedDate.Value.Date;
                DateTime dateto = datepickerTo.SelectedDate.Value.Date;
                var supplier = SupplierComboSearch.SelectedItem as Entities.Suppliers;
                purchaseList = purchaseDatabaseHelper.GetDateBetweenSupplierPurchase(supplier.Id,datefrom, dateto);
                purchaseList.Sort((x, y) => x.PurchaseDate.CompareTo(y.PurchaseDate));
                PurchaseListView.ItemsSource = purchaseList;
                PurchaseListView.Items.Refresh();
                LblTotalPurchases.Content = purchaseList.Count;
                float TAmount = purchaseList.Sum(s => s.TotalPrice);
                LblAmount.Content = TAmount.ToString();

            }
            catch (Exception)
            {
                Info_Dialogue infobox = new Info_Dialogue("Something went wrong,there are no purchases to show.");
                infobox.ShowDialog();
            }
        }

        private void btnSearchByID_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                purchaseList.Clear();
                string id = txtPurchaseID.Text;
                purchaseList = purchaseDatabaseHelper.GetPurchaseByID(id).ToList();
                PurchaseListView.ItemsSource = purchaseList;
                LblTotalPurchases.Content = purchaseList.Count;
                float TAmount = purchaseList.Sum(s => s.TotalPrice);
                LblAmount.Content = TAmount.ToString();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no purchases to show.");
                infobox.ShowDialog();
            }
        }

        private void SupplierComboSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                purchaseList.Clear();
                int id = Convert.ToInt32(SupplierComboSearch.SelectedValue.ToString());
                purchaseList = purchaseDatabaseHelper.GetSupplierPurchase(id).ToList();
                purchaseList.Sort((x, y) => x.PurchaseDate.CompareTo(y.PurchaseDate));
                PurchaseListView.ItemsSource = purchaseList;
                LblTotalPurchases.Content = purchaseList.Count;
                float TAmount = purchaseList.Sum(s => s.TotalPrice);
                LblAmount.Content = TAmount.ToString();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no purchases to show.");
                infobox.ShowDialog();
            }
        }
    }
}
