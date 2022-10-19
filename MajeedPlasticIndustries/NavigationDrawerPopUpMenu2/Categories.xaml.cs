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
using BizBook.Models;
namespace BizBook
{
    
    public partial class Categories : UserControl
    {
        private readonly CategoryDatabaseHelper helper = new CategoryDatabaseHelper();
        List<Entities.Category> CategoryList = new List<Entities.Category>();
        public Categories()
        {
            InitializeComponent();
            LoadAll();
        }

        private void LoadAll() 
        {
            try
            {
                CategoryList = helper.GetCategories();
                List<Models.DummyModels.CategoryModel> categoryList = new List<Models.DummyModels.CategoryModel>();
                int count = 1;
                foreach (var item in CategoryList)
                {
                    Models.DummyModels.CategoryModel model = new Models.DummyModels.CategoryModel 
                    {
                        ID = item.Id,
                        No = count,
                        Name = item.Name
                    };
                    categoryList.Add(model);
                    count = count + 1;
                }
                CategoryListView.ItemsSource = categoryList;
                CategoryListView.Items.Refresh();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no categories to show.");
                infobox.ShowDialog();
            }
        }

        private void btnNewCategory_Click(object sender, RoutedEventArgs e)
        {
            New_Category nc = new New_Category();
            nc.ShowDialog();
        }

        private void btnLoadAllCategories_Click(object sender, RoutedEventArgs e)
        {
            LoadAll();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
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
                    var model = CategoryListView.SelectedItem as Models.DummyModels.CategoryModel;
                    var category = helper.GetSingleCategory(model.ID);

                    New_Category pp = new New_Category(category, true);
                    pp.ShowDialog();
                }
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,please try again.");
                infobox.ShowDialog();
            }
        }
    }
}
