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
   
    public partial class Configuration_Settings : Window
    {
        private readonly CompanyDatabaseHelper helper = new CompanyDatabaseHelper();
        int companyID;
        public Configuration_Settings()
        {
            InitializeComponent();
            var company = helper.GetSpecificCompany();
            txtCompanyName.Text = company.Name;
            txtOwner.Text = company.OwnerName;
            txtTelephone.Text = company.Telephone;
            txtAddress.Text = company.Address;
            companyID = company.Id;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtncSaveConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Company model = new Company()
                {
                    OwnerName = txtOwner.Text,
                    Name = txtCompanyName.Text,
                    Address = txtAddress.Text,
                    Telephone = txtTelephone.Text,
                    Id = companyID

                };
                 helper.EditCompany(model);

                Properties.Configuration.Default.Owner_Name = model.OwnerName;
                Properties.Configuration.Default.Telephone = model.Telephone;
                Properties.Configuration.Default.Address = model.Address;
                Properties.Configuration.Default.Company = model.Name;
                Properties.Configuration.Default.Save();
                InfoBox infobox = new InfoBox("Company details saved successfully.");
                infobox.ShowDialog();
            }

            catch (Exception)
            {
                InfoBox infobox = new InfoBox("ERROR:All Feilds are required.");
                infobox.ShowDialog();
            }
        }
    }
}
