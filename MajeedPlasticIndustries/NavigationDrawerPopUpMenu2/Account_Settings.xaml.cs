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
    
    public partial class Account_Settings : Window
    {
        private readonly UserDatabaseHelper helper = new UserDatabaseHelper();
        public Account_Settings()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = PtxtUsername.Text;
                string oldpassword = PtxtOldPassword.Password;
                Users users = new Users()
                {
                    UserName = PtxtUsername.Text,
                    Password = PtxtNewPassword.Password
                };
                helper.EditPassword(users, username, oldpassword);
                InfoBox infobox = new InfoBox("Password changed successfully.");
                infobox.ShowDialog();
                PtxtUsername.Clear();
                PtxtNewPassword.Clear();
                PtxtOldPassword.Clear();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("ERROR:All Feilds are required.");
                infobox.ShowDialog();
            }
        }

        private void btnChangeUsername_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string oldusername = UtxtOldUsername.Text;
                string password = UtxtPassword.Password;
                Users users = new Users()
                {
                    UserName = NewtxtUsername.Text,
                    Password = UtxtPassword.Password
                };
                helper.EditUsername(users, oldusername, password);
                InfoBox infobox = new InfoBox("Username changed successfully.");
                infobox.ShowDialog();
                UtxtOldUsername.Clear();
                UtxtPassword.Clear();
                NewtxtUsername.Clear();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("ERROR:All Feilds are required.");
                infobox.ShowDialog();
            }
        }
    }
}
