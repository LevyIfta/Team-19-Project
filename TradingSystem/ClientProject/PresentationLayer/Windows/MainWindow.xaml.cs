﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_Trial2.PresentationLayer.DataContext;
using ClientProject;
using ClientProject.PresentationLayer.Windows;

namespace WPF_Trial2.PresentationLayer.Windows
{
    class UserDataName : ANotifyPropChange
    {
        private String Username;

        public String userhello
        {
            get { return Username; }
            set
            {
                Username = value;
                OnPropertyChanged();
            }
        }

        private String Storename;

        public String storename
        {
            get { return Storename; }
            set
            {
                Storename = value;
                OnPropertyChanged();
            }
        }

        private String Storemsg;

        public String storemsg
        {
            get { return Storemsg; }
            set
            {
                Storemsg = value;
                OnPropertyChanged();
            }
        }

        private String Searchmsg;

        public String searchmsg
        {
            get { return Searchmsg; }
            set
            {
                Searchmsg = value;
                OnPropertyChanged();
            }
        }
    }
    public partial class MainWindow : Window
    {
        static Controller controler = Controller.GetController();
        UserDataName user = new UserDataName();
        public string username = WindowManager.username;
        string msgFailed = "";

        private class searchString : INotifyPropertyChanged
        {
            private String search;
            public String Search
            {
                get
                {
                    return search;
                }
                set
                {
                    search = value;
                    OnPropertyChanged(search);
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string search)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(search));
                }
            }
        }

        

        //private UserDataContext userDataContext;
        private List<StoreDataContext> storesDataContexts;
        private searchString searchStr;
        public List<string> alarms;
        private enum SearchOption
        {
            PRODUCT,
            STORE
        }

        private SearchOption searchOption;
        public MainWindow()
        {
            //this.userDataContext = new UserDataContext();
            this.alarms = new List<string>();
            this.DataContext = user;
            user.userhello = "hello, " + username;
            //string ans = controler.getUserName();
            //if (ans != null)
            //user.userhello = ans;
            //this.UserNameHello = ans;
            this.storesDataContexts = new List<StoreDataContext>();
            this.searchStr = new searchString();
            InitializeComponent();
            this.byStore.IsChecked = true;


            //testing purpose
            this.storesDataContexts.Add(new StoreDataContext("a"));
            this.storesDataContexts.Add(new StoreDataContext("b"));
            this.storesDataContexts.Add(new StoreDataContext("c"));
        }

        //---search bar grayed text---
        private void byStoreChecked(object sender, RoutedEventArgs e)
        {
            this.searchBarText.Text = " Store name:";
        }
        private void byProductChecked(object sender, RoutedEventArgs e)
        {
            this.searchBarText.Text = " Product name:";
        }
        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.searchBarText.Visibility = Visibility.Hidden;
        }
        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //if (this.searchBarBox.Text.Length < 1)
            {
                //this.searchBarText.Visibility = Visibility.Visible;
            }
            
        }

       
        //used by login window
        public void setLoggedInUser(UserDataContext udc)
        {
            //this.userDataContext = udc;
        }


        private void Search(object sender, RoutedEventArgs e)
        {
            //this.button1.Content = this.searchStr.Search;
            string searchVal = this.SearchValue.Text;
            
            switch (this.searchOption)
            {
                case SearchOption.PRODUCT:
                    searchByProduct(searchVal);
                    break;
                case SearchOption.STORE:
                    searchStore(searchVal);
                    break;
                default:
                    break;
            }
        }
        private void searchStore(string storeName)
        {
            string store = controler.SearchStore(storeName);
            if (store != null && !store.Equals(""))
            {
                // show in list and then click on the name open window
                StoreWindow storeWindow = new StoreWindow(store);
                storeWindow.Show();
                App.Current.MainWindow = storeWindow;
            }
            else
            {
                this.user.searchmsg = "insert name";
                
            }
        }
        private void searchByStore(string storeName)
        {
            ICollection<string> stores = controler.SearchStores(storeName);

            if (stores != null)
                foreach (string store in stores)
                {
                    var button = new Button() { Content = store };
                    button.Click += openStoreWindow;
                    this.ContentMain.Children.Add(button);
                }

        }
        private void openStoreWindow(object sender, RoutedEventArgs e)
        {
            string storeName = (e.Source as Button).Content.ToString();
            controler.SearchStores(storeName);
        }
        private void searchByProduct(string searchVal)
        {
            throw new NotImplementedException();
        }

        private void ByProduct_Checked(object sender, RoutedEventArgs e)
        {
            this.searchOption = SearchOption.PRODUCT;
        }

        private void ByStore_Checked(object sender, RoutedEventArgs e)
        {
            this.searchOption = SearchOption.STORE;
        }

        private void OpenStore(object sender, RoutedEventArgs e)
        {
            Store loginWindow = new Store();
            loginWindow.Show();
            App.Current.MainWindow = loginWindow;
        }

        private void login_register_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            App.Current.MainWindow = loginWindow;
            this.Close();
        }
        private void logout(object sender, RoutedEventArgs e)
        {
            Main1 m1 = new Main1();
            m1.Show();
            App.Current.MainWindow = m1;
            this.Close();
            /*
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            App.Current.MainWindow = loginWindow;
            this.Close();
            */
            //loginWindow.ShowDialog();
        }
        private void logoutf(object sender, RoutedEventArgs e)
        {
            bool ans = controler.Logoutfunc();
            if (!ans)
            {
                user.userhello = "Failed logout";
            }
            else
            {
                user.userhello = "hello, guest";
            }
            //loginWindow.ShowDialog();
        }


        public void getAlarm(string title, string des)
        {
            this.alarms.Add(des);
        }

       
    }
}
