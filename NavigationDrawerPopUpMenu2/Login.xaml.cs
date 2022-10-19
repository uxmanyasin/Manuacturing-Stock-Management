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
    
    public partial class Login : Window
    {
        private readonly UserDatabaseHelper helper = new UserDatabaseHelper();
        public Login()
        {
            InitializeComponent();
            txtUsername.Text = Properties.Configuration.Default.Username;
           
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string user = txtUsername.Text;
                string password = txtPassword.Password;

                Users model = helper.GetUser(user, password);
                if (model == null)
                {
                    InfoBox infobox = new InfoBox("No Internet Connection / Username or Password is incorrect.");
                    infobox.ShowDialog();
                }
                else
                {
                    if(model.Type == "Admin")
                    {
                        Properties.Configuration.Default.Username = txtUsername.Text;
                        Properties.Configuration.Default.Password = txtPassword.Password;
                        Properties.Configuration.Default.AccessMode = false;
                        Properties.Configuration.Default.Save();
                        Home home = new Home();
                        home.Show();
                        this.Close();
                    }
                    else if (model.Type == "Employee") 
                    {
                        Properties.Configuration.Default.Username = txtUsername.Text;
                        Properties.Configuration.Default.Password = txtPassword.Password;
                        Properties.Configuration.Default.AccessMode = true;
                        Properties.Configuration.Default.Save();
                        GeneralUser home = new GeneralUser();
                        home.Show();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
               // InfoBox infobox = new InfoBox("No Internet Connection or there is an error to connect to your server.");
               // infobox.ShowDialog();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Properties.Configuration.Default.Username = txtUsername.Text;
            Properties.Configuration.Default.Save();
            this.Close();
        }
    }
}
