using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using BizBook.Entities;
using BizBook.Models;

namespace BizBook
{

    public partial class SplashScreen : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        CompanyDatabaseHelper CompanyDatabaseHelper = new CompanyDatabaseHelper();
        public SplashScreen()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(timer_Tick);

            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            CheckLiscence();
            timer.Stop();
            
        }
        private void CheckLiscence()
        {
            try
            {
                var company = CompanyDatabaseHelper.GetSpecificCompany();
                if (company.Status == false)
                {
                    InfoBox infobox = new InfoBox("Something went wrong,Please contact your service provider.");
                    infobox.ShowDialog();
                }
                else 
                {
                    
                    Login l = new Login();
                    l.Show();
                    this.Close();
                }
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Connection to Server FAILED.");
                infobox.ShowDialog();
                this.Close();
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            //for (int i = 0; i < 100; i++)
            //{
            //    PbStatus.Value++;
            //    Thread.Sleep(100);
            //}
        }
    }
}
