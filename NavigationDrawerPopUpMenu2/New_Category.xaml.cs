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
   
    public partial class New_Category : Window
    {
        private readonly CategoryDatabaseHelper helper = new CategoryDatabaseHelper();
        private Entities.Category _category;
        public bool update { get; set; }
        public New_Category()
        {
            InitializeComponent();
        }

        public New_Category(Entities.Category category, bool indicator)
        {
            InitializeComponent();
            _category = category;
            txtName.Text = _category.Name;
            update = indicator;
            lblTitle.Text = "Update Category";
            btnSaveCategory.Content = "Update";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSaveCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (update == true)
                {

                    _category.Name = txtName.Text;
                    helper.EditCategory(_category);
                    InfoBox info = new InfoBox(_category.Name + " updated succesfully.");
                    info.ShowDialog();
                }
                else
                {
                    _category = new Entities.Category()
                    {
                        Name = txtName.Text


                    };
                    helper.CreateRawMaterial(_category);
                    InfoBox infobox = new InfoBox(_category.Name + " created succesfully.");
                    infobox.ShowDialog();
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
