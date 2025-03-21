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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientWeb.View.StoreManagement
{
    /// <summary>
    /// Interaction logic for ManageFeedBacks.xaml
    /// </summary>
    public partial class ManageFeedBacks : Page
    {
        private string storeName;
        private string username;

        public ManageFeedBacks()
        {
            InitializeComponent();
        }

        public ManageFeedBacks(string username, string storeName)
        {
            this.username = username;
            this.storeName = storeName;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Store page = new Store(storeName);
            NavigationService.Navigate(page);
        }
    }
}
