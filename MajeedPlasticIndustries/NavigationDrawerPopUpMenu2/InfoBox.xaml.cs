﻿using System;
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
using System.Windows.Threading;

namespace BizBook
{
   
    public partial class InfoBox : Window
    {
        DispatcherTimer timer = new DispatcherTimer();

        string _msg = "";
        public InfoBox(string msg)
        {
            InitializeComponent();
            timer.Tick += new EventHandler(timer_Tick);

            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Start();
            _msg = msg;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            
            timer.Stop();
            this.Close();

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            msg.Text = _msg;
        }
    }
}
