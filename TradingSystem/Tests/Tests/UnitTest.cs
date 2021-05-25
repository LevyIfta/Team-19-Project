using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using Tests.Bridge;
using TradingSystem.BuissnessLayer;
using System.Linq;
using TradingSystem.BuissnessLayer.commerce;
using TradingSystem.ServiceLayer;
using System;
using System.Threading;
using System.Timers;

namespace Tests
{
    [TestClass]
    public class UserAccessUnitTest
    {
        [ClassInitialize]
        public static void classInit(TestContext context)
        {
            /*
            bridge = Driver.getBridge();
            bridge.register("user1", "Password1");
            bridge.register("user2", "Password2");
            */
        }

        [TestMethod]
        public void loginTestGood()
        {
            // , fake1 = "fake1", fake2 = "fake2" ' , fakePass1 = "111", fakePass2 = "789"
            // init usernames and passes
            UserServices.logout("AbD1");
            UserServices.logout("QwEr2");
            string username1 = "AbD1", username2 = "QwEr2";
            string pass1 = "123Xx456", pass2 = "465Ss789";
            // register
            UserController.register(username1, pass1);
            UserController.register(username2, pass2);
            // try to login twice
            aUser u = UserServices.getUser(username1);
            //string[] usr = UserServices.login(username1, pass1);
            //Assert.IsTrue(UserController.login(username1, pass1));
            string[] usr2 = UserServices.login(username2, pass2);
            Assert.IsTrue(usr2[0].Equals("true"));
            UserServices.logout("QwEr2");
            UserServices.logout("AbD1");

        }

        [TestMethod]
        public void loginTestBad()
        {
            // init usernames and passes
            string username1 = "AbD1", username2 = "QwEr2", fake1 = "fake1", fake2 = "fake2";
            string pass1 = "123Xx456", pass2 = "465Ss789", fakePass1 = "111", fakePass2 = "789";
            // register
            UserServices.register(username1, pass1);
            UserServices.register(username2, pass2);
            // try to login twice
            string[] u1 = UserServices.login(username1, pass1);
            string[] u2 = UserServices.login(username1, pass1);
            Assert.IsTrue(u2[0].Equals("false"));
            // try to login with fake user
            string[] u3 = UserServices.login(fake1, fakePass1);
            Assert.IsTrue(u3[0].Equals("false"), "managed to login as fake user");
            UserController.logout();
            // fake pass
            string[] u4 = UserServices.login(username2, fakePass2);
            Assert.IsTrue(u4[0].Equals("false"), "managed to login with wrong password");
            UserController.logout();

            string[] u5 = UserServices.login(fake1, pass1);
            Assert.IsTrue(u5[0].Equals("false"), "managed to login with fake username");
            UserController.logout();
        }


        [TestMethod]
        public void registerTestGood()
        {
            string[] u1 = UserServices.register("newUser1", "newPassword1");
            Assert.IsTrue(u1[0].Equals("true"), "faild to register as a valid user");
            string[] u2 = UserServices.login("newUser1", "newPassword1");
            Assert.IsTrue(u1[0].Equals("true"), "user was not properly saved");
            UserController.logout();
            string[] u3 = UserServices.login("newUser1", "fakePassword1");
            Assert.IsTrue(u3[0].Equals("false"), "wrong password found as exist");
            UserController.logout();

            string[] u4 = UserServices.register("newUser2", "newPassword1");
            Assert.IsTrue(u4[0].Equals("true"), "faild to register as a valid user(existing password)");
            string[] u5 = UserServices.login("newUser2", "newPassword1");
            Assert.IsTrue(u5[0].Equals("true"), "user was not properly saved (existing password)");
            UserController.logout();
        }

        [TestMethod]
        public void registerTestBad()
        {
            UserServices.register("user1", "Password1");
            string[] u1 = UserServices.register("user1", "Password1");
            Assert.IsFalse(u1[0].Equals("true"), "managed to register as an alread existing user (same password)");
            string[] u2 = UserServices.register("user1", "Password2");
            Assert.IsFalse(u2[0].Equals("true"), "managed to register with an existig username (different passwords)");
            string[] u3 = UserServices.login("user1", "Password2");
            Assert.IsTrue(u3[0].Equals("false"), "exisint user with different passworrd considerd to exist");
            UserController.logout();

            string[] u4 = UserServices.register("badbadBADUSERwithBadWord!@$%$$$$_8", "OKpassword");
            Assert.IsFalse(u4[0].Equals("true"), "managed to register with bad username");
            string[] u5 = UserServices.login("badbadBADUSERwithBadWord!@$%$$$$_8", "OKpassword");
            Assert.IsFalse(u5[0].Equals("true"), "invlid user saved (bad username)");
            UserController.logout();

            string[] u6 = UserServices.register("okUser", "badpassword^^^^^^^^^^^^^^^^^^^^^6^^#@#$%^");
            Assert.IsFalse(u6[0].Equals("true"), "managed to register with bad password");
            string[] u7 = UserServices.login("okUser", "badpassword^^^^^^^^^^^^^^^^^^^^^6^^#@#$%^");
            Assert.IsFalse(u7[0].Equals("true"), "invlid user saved (bad password)");
            UserController.logout();

            string[] u8 = UserServices.register("badbadBADUSERwithBadWord!@$%$$$$_8", "badpassword^^^#@#$%^");
            Assert.IsFalse(u8[0].Equals("true"), "managed to register with bad username and bad password");
            string[] u9 = UserServices.login("badbadBADUSERwithBadWord!@$%$$$$_8", "badpassword^^^#@#$%^");
            Assert.IsFalse(u8[0].Equals("true"), "invlid user saved (bad username and bad passsword)");
            UserController.logout();
        }

        [TestMethod]
        public void logoutTest()
        {
            string username = "newUser1_", pass = "newPas00s";
            UserServices.register(username, pass);
            string[] u = UserServices.login(username, pass);
            string[] u1 = UserServices.login(username, pass);
            Assert.IsFalse(u1[0].Equals("true")); // user should be logged in
            UserServices.logout(username);
            string[] u3 = UserServices.login(username, pass);
            Assert.IsTrue(u3[0].Equals("true"));
            UserController.logout();
        }

    }

    [TestClass]
    public class StoreTests
    {
        private static ProductInfo product1;
        private static ProductInfo product2;
        private static string store1Name;
        private static string store2Name;
        [ClassInitialize]
        public static void classInit(TestContext context)
        {
            string username = "ShopOwner11", pass = "123xX321";
            //bool ownerReg = 
            UserServices.register(username, pass);
            // create 2 stores
            // login
            UserServices.login(username, pass);
            Member owner = (Member)UserServices.getUser(username);
            // init names
            store1Name = "store1_";
            store2Name = "store2_";
            // establish the stroes
            owner.EstablishStore(store1Name);
            owner.EstablishStore(store2Name);
            // init product infos
            product1 = ProductInfo.getProductInfo("Batman Costume", "clothing", "FairytaleLand");
            product2 = ProductInfo.getProductInfo("Wireless keyboard", "computer accessories", "Logitech");
            // add some produts
            /*

            ProductInfo newInfo1 = ProductInfo.getProductInfo("item1", "cat", "man");

            ProductInfo newInfo2 = ProductInfo.getProductInfo("item2", "cat2", "man2");


            prod1 = newInfo1;
            prod2 = newInfo2;


            Product items1 = new Product(prod1, 2, 5), items2 = new Product(prod2, 2, 5);


            ShoppingBasket basket = new ShoppingBasket(bridge.getStore("Store1"), (Member)bridge.getUser());
            basket.products.Add(items1);
            basket.products.Add(items2);

            bridge.addInventory(basket);
            
            */

        }



        [TestMethod]
        public void createStoreTestGood()
        {
            string storeOwnerName = "StoreOwner_", storeOwnerPass = "123Xx456";
            string storeName = null;
            bool ownerReg = UserController.register(storeOwnerName, storeOwnerPass);
            UserServices.login(storeOwnerName, storeOwnerPass);
            aUser storeOwner = UserServices.getUser(storeOwnerName);
            // try to create a store  with an empty name
            Assert.IsFalse(Stores.addStore(storeName, (Member)storeOwner), "managed to create a store with a null name");
            Assert.IsNull(Stores.searchStore(storeName), "found a store with a null name");
            UserController.logout();
        }
        [TestMethod]
        public void createStoreTestBad()
        {
            string storeOwnerName = "StoreOwner", storeOwnerPass = "123Xx456";
            string storeName = "createStoreTestBad_store1";
            bool ownerReg = UserController.register(storeOwnerName, storeOwnerPass);
            UserServices.login(storeOwnerName, storeOwnerPass);
            aUser storeOwner = UserServices.getUser(storeOwnerName);
            Stores.addStore(storeName, (Member)storeOwner);
            Store store = Stores.searchStore(storeName);

            Assert.IsFalse(Stores.addStore(storeName, (Member)storeOwner), "manage to open a store with an existing name");
            Assert.AreEqual(store.founder.getUserName(), storeOwnerName, "store founder changed");
            UserController.logout();
        }

        [TestMethod]
        public void searchStoreTest()
        {
            string storeOwnerName = "StoreOwner_1", storeOwnerPass = "123Xx456";
            string storeName = "searchStoreTestGood_store1";
            bool ownerReg = UserController.register(storeOwnerName, storeOwnerPass);
            UserServices.login(storeOwnerName, storeOwnerPass);
            aUser storeOwner = UserServices.getUser(storeOwnerName);
            Stores.addStore(storeName, (Member)storeOwner);
            Store store = Stores.searchStore(storeName);

            Assert.IsNotNull(store, "could not find an existing store: " + storeName);
            Assert.IsNull(Stores.searchStore("this store does not exist"));
            UserController.logout();
        }

        [TestMethod]
        public void parallelPurchase()
        {
            // register twice and login from two different users
            bool user1reg = UserController.register("AliKB", "123xX456");
            bool user2reg = UserController.register("Bader", "456xX789");
            // login
            UserServices.login("AliKB", "123xX456");
            aUser user1 = UserServices.getUser("AliKB");
            UserServices.login("Bader", "456xX789");
            aUser user2 = UserServices.getUser("Bader");
            // establish a new store
            string storeName = "Ali Shop";
            Stores.addStore(storeName, (Member)user1);
            Store aliShop = Stores.searchStore(storeName);
            // add products to the strore
            aliShop.addProduct("Bamba", "Food", "Osem");
            // set the price of the product
            aliShop.editPrice("Bamba", "Osem", 3);
            // supply 
            aliShop.supply("Bamba", "Osem", 20);
            // try to buy more than 20 bamba in total
            user1.getCart().getBasket(aliShop).addProduct(new Product(ProductInfo.getProductInfo("Bamba", "Food", "Osem"), 12, 0));
            user2.getCart().getBasket(aliShop).addProduct(new Product(ProductInfo.getProductInfo("Bamba", "Food", "Osem"), 12, 0));
            // purchase

            ICollection<Receipt> receipts1 = new LinkedList<Receipt>();
            ICollection<Receipt> receipts2 = null;
            bool flag = true;

            Thread purchase1 = new Thread(() =>
            {
                Thread.Sleep(2000);
                receipts1 = user1.purchase(new CreditCard());
            }),
                purchase2 = new Thread(() =>
                {
                    Thread.Sleep(2000);
                    receipts2 = user2.purchase(new CreditCard());
                }),
                assertThread = new Thread(() =>
                {
                    Thread.Sleep(3000);
                    flag &= (receipts1 != null && (receipts1.Count > 0 & receipts2 == null)) | (receipts2 != null && (receipts2.Count > 0 & receipts1 == null));
                  
                    // check for amount
                    flag &= aliShop.searchProduct("Bamba", "Osem").amount == 8;
                });

            purchase1.Start();
            purchase2.Start();
            assertThread.Start();

            Thread.Sleep(3500);
            Assert.IsTrue(flag);
            
            /*
            System.Timers.Timer purchase1, purchase2, assert;
            // Create a timer with a two second interval.
            purchase1 = new System.Timers.Timer(2000);
            purchase2 = new System.Timers.Timer(2000);
            assert = new System.Timers.Timer(3000);

            // Hook up the Elapsed event for the timer. 
            purchase1.Elapsed += (s, e) => receipts1 = user1.purchase(new CreditCard());
            purchase2.Elapsed += (s, e) => receipts2 = user2.purchase(new CreditCard());
            assert.Elapsed += (s, e) =>
            {
                Assert.IsTrue((receipts1 != null && (receipts1.Count > 0 & receipts2 == null)) | (receipts2 != null && (receipts2.Count > 0 & receipts1 == null)));
                // check for amount
                Assert.IsTrue(aliShop.searchProduct("Bamba", "Osem").amount == 8);
            };

            purchase1.AutoReset = false;
            purchase2.AutoReset = false;

            purchase1.Enabled = true;
            purchase2.Enabled = true;

            purchase1.Start();
            purchase2.Start();
            assert.Start();

            Thread.Sleep(3500);*/
            //receipts1 = user1.purchase(new CreditCard());
            //receipts2 = user2.purchase(new CreditCard());

            //Assert.IsTrue((receipts1 != null && (receipts1.Count > 0 & receipts2 == null)) | (receipts2 != null && (receipts2.Count > 0 & receipts1 == null)));
            // check for amount
            //Assert.IsTrue(aliShop.searchProduct("Bamba", "Osem").amount == 8);
            UserServices.logout("AliKB");
        }

        [TestMethod]
        public void purchaseTestGood()
        {
            // register
            string username1 = "StoreOwner1";
            string pass1 = "123xX456";
            // register and login
            bool ownerReg = UserController.register(username1, pass1);
            UserServices.login(username1, pass1);
            aUser storeOwner = UserServices.getUser(username1);
            // establish a new store
            string storeName = "Ali Shop2_purchaseTestGood";
            Stores.addStore(storeName, (Member)storeOwner);
            Store aliShop = Stores.searchStore(storeName);
            // add products to the strore
            aliShop.addProduct("Bamba", "Food", "Osem");
            // set the price of the product
            aliShop.editPrice("Bamba", "Osem", 3);
            // supply 
            aliShop.supply("Bamba", "Osem", 20);
            // register and login
            string username = "AliKSB";
            string pass = "123xX456";

            bool user1reg = UserController.register(username, pass);
            UserServices.login(username, pass);
            aUser user1 = UserServices.getUser(username);
            // try to buy 12 bamba - less that overall
            user1.getCart().getBasket(aliShop).addProduct(new Product(ProductInfo.getProductInfo("Bamba", "Food", "Osem"), 12, 0));
            // purchase
            ICollection<Receipt> receipts1 = user1.purchase(new CreditCard());
            // check for the amounts
            Assert.AreEqual(aliShop.searchProduct("Bamba", "Osem").amount, 8);
        }

        [TestMethod]
        public void purchaseTestBad()
        {
            // register
            string ownerUsername = "AliBB", ownerPass = "123Xx123";
            bool reg = UserController.register(ownerUsername, ownerPass);
            string storeName = "Ali Shop3";

            UserServices.login(ownerUsername, ownerPass);
            aUser owner = UserServices.getUser(ownerUsername);
            // establish a new store
            Stores.addStore(storeName, (Member)owner);

            Store aliShop = Stores.searchStore(storeName);
            // add products to the strore
            aliShop.addProduct("Bamba", "Food", "Osem");
            // set the price of the product
            aliShop.editPrice("Bamba", "Osem", 3);
            // supply 
            aliShop.supply("Bamba", "Osem", 20);
            // register and login
            string username = "AliKSBa";
            string pass = "123xX456";

            bool user1reg = UserController.register(username, pass);
            UserServices.login(username, pass);
            aUser user1 = UserServices.getUser(username);
            // try to buy 22 bamba - more that overall
            user1.getCart().getBasket(aliShop).addProduct(new Product(ProductInfo.getProductInfo("Bamba", "Food", "Osem"), 22, 0));
            // purchase
            ICollection<Receipt> receipts1 = user1.purchase(new CreditCard());
            // check for the amounts
            Assert.AreEqual(aliShop.searchProduct("Bamba", "Osem").amount, 20);
        }

        [TestMethod]
        public void addProductTestGood()
        {
            // add new ProductInfo
            ProductInfo p = ProductInfo.getProductInfo("productX2", "categoryX2", "ManufacturerX2");
            ProductInfo p1 = ProductInfo.getProductInfo("productY2", "categoryY2", "ManufacturerY2");
            ProductInfo p2 = ProductInfo.getProductInfo("productZ2", "categoryZ2", "ManufacturerZ2");
            // add products to stores
            Stores.searchStore(store1Name).addProduct(p1.name, p1.category, p1.manufacturer);
            Stores.searchStore(store1Name).addProduct(p2.name, p2.category, p2.manufacturer);
            Stores.searchStore(store2Name).addProduct(p1.name, p1.category, p1.manufacturer);
            // assert that the products exist
            Assert.IsTrue(Stores.searchStore(store1Name).isProductExist(p1.name, p1.manufacturer));
            Assert.IsTrue(Stores.searchStore(store1Name).isProductExist(p2.name, p2.manufacturer));
            Assert.IsTrue(Stores.searchStore(store2Name).isProductExist(p1.name, p1.manufacturer));
            Assert.IsFalse(Stores.searchStore(store2Name).isProductExist(p2.name, p2.manufacturer));
            // clean the stores
            Stores.searchStore(store1Name).removeProduct(p1.name, p1.manufacturer);
            Stores.searchStore(store1Name).removeProduct(p2.name, p2.manufacturer);
            Stores.searchStore(store2Name).removeProduct(p1.name, p1.manufacturer);
            // assert that the products were removed
            Assert.IsFalse(Stores.searchStore(store1Name).isProductExist(p1.name, p1.manufacturer));
            Assert.IsFalse(Stores.searchStore(store1Name).isProductExist(p2.name, p2.manufacturer));
            Assert.IsFalse(Stores.searchStore(store2Name).isProductExist(p1.name, p1.manufacturer));
        }

        [TestMethod]
        public void supplyTestGood()
        {
            // add the products to the stores
            Stores.searchStore(store1Name).addProduct(product1.name, product1.category, product1.manufacturer);
            Stores.searchStore(store1Name).addProduct(product2.name, product2.category, product2.manufacturer);
            Stores.searchStore(store2Name).addProduct(product1.name, product1.category, product1.manufacturer);
            Stores.searchStore(store2Name).addProduct(product2.name, product2.category, product2.manufacturer);
            // supply store1
            Stores.searchStore(store1Name).supply(product1.name, product1.manufacturer, 25);
            Stores.searchStore(store1Name).supply(product2.name, product2.manufacturer, 30);
            // supply store2
            Stores.searchStore(store2Name).supply(product1.name, product1.manufacturer, 10);
            Stores.searchStore(store2Name).supply(product2.name, product2.manufacturer, 40);
            // check amounts
            // store1
            Assert.AreEqual(Stores.searchStore(store1Name).searchProduct(product1.name, product1.manufacturer).amount, 25);
            Assert.AreEqual(Stores.searchStore(store1Name).searchProduct(product2.name, product2.manufacturer).amount, 30);
            // store2
            Assert.AreEqual(Stores.searchStore(store2Name).searchProduct(product1.name, product1.manufacturer).amount, 10);
            Assert.AreEqual(Stores.searchStore(store2Name).searchProduct(product2.name, product2.manufacturer).amount, 40);
        }

        [TestMethod]
        public void supplyTestBad()
        {
            // add new ProductInfo
            ProductInfo p = ProductInfo.getProductInfo("productX", "categoryX", "ManufacturerX");
            ProductInfo p1 = ProductInfo.getProductInfo("productY", "categoryY", "ManufacturerY");
            ProductInfo p2 = ProductInfo.getProductInfo("productZ", "categoryZ", "ManufacturerZ");
            // add the products to the stores
            Stores.searchStore(store1Name).addProduct(p1.name, p1.category, p1.manufacturer);
            Stores.searchStore(store1Name).addProduct(p2.name, p2.category, p2.manufacturer);
            Stores.searchStore(store2Name).addProduct(p1.name, p1.category, p1.manufacturer);
            Stores.searchStore(store2Name).addProduct(p2.name, p2.category, p2.manufacturer);
            // supply stores with illegal amoounts
            Assert.IsFalse(Stores.searchStore(store1Name).supply(p1.name, p1.manufacturer, -5));
            Assert.IsFalse(Stores.searchStore(store1Name).supply(p2.name, p2.manufacturer, -3));
            Assert.IsFalse(Stores.searchStore(store2Name).supply(p1.name, p1.manufacturer, -9));
            Assert.IsFalse(Stores.searchStore(store2Name).supply(p2.name, p2.manufacturer, -6));
            // supply stores with illegal product info
            Assert.IsFalse(Stores.searchStore(store1Name).supply(p.name, p.manufacturer, 30));
            Assert.IsFalse(Stores.searchStore(store2Name).supply(p.name, p.manufacturer, 40));
            Assert.IsFalse(Stores.searchStore(store1Name).supply(p1.name, p.manufacturer, 30)); // wrong manufacturer
            Assert.IsFalse(Stores.searchStore(store2Name).supply(p.name, p1.manufacturer, 40)); // wrong name
            // check amounts
            // store1
            // wrong amounts
            Assert.AreEqual(Stores.searchStore(store1Name).searchProduct(p1.name, p1.manufacturer).amount, 0);
            Assert.AreEqual(Stores.searchStore(store1Name).searchProduct(p2.name, p2.manufacturer).amount, 0);
            // wrong info
            Assert.IsNull(Stores.searchStore(store1Name).searchProduct(p.name, p.manufacturer)); // wrong name & manufacturer
            Assert.IsNull(Stores.searchStore(store1Name).searchProduct(p1.name, p.manufacturer)); // wrong manufacturer
            // store2
            // wrong amounts
            Assert.AreEqual(Stores.searchStore(store2Name).searchProduct(p1.name, p1.manufacturer).amount, 0);
            Assert.AreEqual(Stores.searchStore(store2Name).searchProduct(p2.name, p2.manufacturer).amount, 0);
            // wrong info
            Assert.IsNull(Stores.searchStore(store2Name).searchProduct(p.name, p.manufacturer)); // wrong name & manufacturer
            Assert.IsNull(Stores.searchStore(store2Name).searchProduct(p.name, p1.manufacturer)); // wrong name
        }

    }

    [TestClass]
    public class BasketTests
    {
        private static ProductInfo prod1, prod2;


        [ClassInitialize]
        public static void classInit(TestContext context)
        {
            /*
            bridge = Driver.getBridge();
            bridge.register("basket1", "Basket1");
            bridge.login("basket1", "Basket1");
            bridge.openStore("basketStore1");
            bridge.openStore("basketStore2");
            ProductInfo newInfo1 = ProductInfo.getProductInfo("item1", "cat", "man");
            prod1 = newInfo1;
            ProductInfo newInfo2 =  ProductInfo.getProductInfo("item2", "ca1", "man1");
            prod2 = newInfo2;

            Product items1 = new Product(newInfo1, 2, 5), items2 = new Product(newInfo2, 2, 5);


            ShoppingBasket basket = new ShoppingBasket(bridge.getStore("basketStore1"), (Member)bridge.getUser());
            basket.products.Add(items1);
            basket.products.Add(items2);
            bridge.addProducts(basket);

            bridge.logout();
            */
        }


        [TestMethod]
        public void saveProductTest()
        {
            string username = "AlIbAd";
            string pass = "123xX4";
            string storeName = "new_store_1";

            UserServices.register(username, pass);
            string[] user = UserServices.login(username, pass);
            // open a new store
            Stores.addStore(storeName, (Member)UserServices.getUser(username));
            //setup
            ProductInfo newInfo3 = ProductInfo.getProductInfo("item3", "cat", "man");

            ProductInfo newInfo4 = ProductInfo.getProductInfo("item4", "cat", "man");

            Product items3 = new Product(newInfo3, 5, 5), items4 = new Product(newInfo4, 2, 5);


            ShoppingBasket basket = new ShoppingBasket(Stores.searchStore(storeName), (Member)UserServices.getUser(username));
            basket.addProduct(items3);
            basket.addProduct(items4);

            Assert.IsTrue(basket.products.Contains(items3), "failed to save one of the items");
            Assert.IsTrue(basket.products.Contains(items4), "failed to save one of the items");
            Assert.IsFalse(basket.products.Contains(null), "saved a null item");
            //bridge.logout();
        }

        /*
        [TestMethod]
        public void removeProductTest()
        {
            //setup
   
            Product items1 = new Product(prod1, 1, 5), items2 = new Product(prod2, 2, 5);
            
            ShoppingBasket basket = new ShoppingBasket(bridge.getStore("basketStore1"), (Member)bridge.getUser());
            basket.products.Add(items1);
            basket.products.Add(items2);
            
            bridge.removeProducts(basket);

            ShoppingBasket saveBasket = bridge.getBasket("basketStore");
            Assert.IsTrue(saveBasket.products.Contains(items1), "removed an item with non-0 amount");
            Assert.AreSame(bridge.getProductAmount(saveBasket, prod1), 1, "didnt change the amount properly");
            Assert.IsFalse(saveBasket.products.Contains(items2), "failed to delete an item with amount of 0");
            Assert.IsFalse(saveBasket.products.Contains(null), "saved a null item");


            //bad
            bridge.removeProducts(basket); //2nd removal
            Assert.IsTrue(saveBasket.products.Contains(items1), "illigal removal worked");
            Assert.AreSame(bridge.getProductAmount(saveBasket, prod1), 1, "illigal removal worked");
            Assert.IsFalse(saveBasket.products.Contains(items2), "illigal removal worked");
            Assert.IsFalse(saveBasket.products.Contains(null), "saved a null item (illgal removal)");

        }
        */

    }


    

    /*
    [TestClass]
    public class StoresTest
    {
        private static Bridge.Bridge bridge;
        private static ProductInfo product1;
        private static ProductInfo product2;
        private static string store1Name;
        private static string store2Name;
        [ClassInitialize]
        public static void classInit(TestContext context)
        {
            bridge = Driver.getBridge();
            // establish stores
            store1Name = "store1";
            store2Name = "store2";
            bridge.openStore(store1Name);
            bridge.openStore(store2Name);
            // add some produts
            product1 = ProductInfo.getProductInfo("Batman Costume", "clothing", "FairytaleLand");
            product2 = ProductInfo.getProductInfo("Wireless keyboard", "computer accessories", "Logitech");
        }

        [ClassCleanup]
        public static void classCleanUp()
        {
            bridge.logout();
        }
        
        
    }
    */

    [TestClass]
    public class ZReciptTests
    {
        aUser user1, user2;
        string username1 = "AliKB", username2 = "Bader", pass1 = "123xX456", pass2 = "456xX789";
        string storeName;
        private static ProductInfo prod1, prod2;
        private static bool isInit = false;
        [ClassInitialize]
        public static void classInit(TestContext context)
        {
            UserServices.register("recipt1", "Recipt1");
            UserServices.register("recipt2", "Recipt2");
            UserServices.login("recipt2", "Recipt2");
            aUser u = UserServices.getUser("recipt2");
            u.EstablishStore("StoreRecipt1");
            u.EstablishStore("StoreRecipt2");
            ProductInfo newInfo1 = ProductInfo.getProductInfo("item1", "cat", "man");

            ProductInfo newInfo2 = ProductInfo.getProductInfo("item2", "cat2", "man2");


            prod1 = newInfo1;
            prod2 = newInfo2;


            Product items1 = new Product(prod1, 2, 5), items2 = new Product(prod2, 2, 5);


            ShoppingBasket basket = new ShoppingBasket(Stores.searchStore("StoreRecipt1"), (Member)u);
            basket.products.Add(items1);
            basket.products.Add(items2);
            /*
            bridge.addInventory(basket);


            bridge.logout();
            bridge.login("recipt1", "Recipt1");
            bridge.addProducts(basket);
            bridge.purchase("Credit");

    */

            isInit = true;
        }

        [TestMethod]
        public void TestAll()
        {
            // this fuction initializes all the needed arguments 
            // and runs all tests
            // register twice and login from two different users
            bool user1reg = UserController.register(username1, pass1);
            bool user2reg = UserController.register(username2, pass2);
            // login
            UserServices.login(username1, pass1);
            user1 = UserServices.getUser(username1);
            UserServices.login(username2, pass2);
            user2 = UserServices.getUser(username2);
            // establish a new store
            storeName = "Makolet";
            Stores.addStore(storeName, (Member)user1);
            Store aliShop = Stores.searchStore(storeName);
            // add products to the strore
            aliShop.addProduct("Bamba", "Food", "Osem");
            // set the price of the product
            aliShop.editPrice("Bamba", "Osem", 3);
            // supply 
            aliShop.supply("Bamba", "Osem", 30);
            // add products to shopping carts
            user1.getCart().getBasket(aliShop).addProduct(new Product(ProductInfo.getProductInfo("Bamba", "Food", "Osem"), 12, 0));
            user2.getCart().getBasket(aliShop).addProduct(new Product(ProductInfo.getProductInfo("Bamba", "Food", "Osem"), 18, 0));
            // purchase
            ICollection<Receipt> receipts1 = user1.purchase(new CreditCard());
            ICollection<Receipt> receipts2 = user2.purchase(new CreditCard());
            // test 
            //userReciptTestGood();
            //userReciptTestBad();
            //storeReciptsTest();
        }


        public void userReciptTestGood()
        {
            bool user1HasReceipt = false, user2HasReceipt = false;
            // check if the receipts that the user holds contain the receipt from the previous purchase
            ICollection<Receipt> u1Receipts = ((Member)UserServices.getUser(user1.getUserName())).reciepts;
            ICollection<Receipt> u2Receipts = ((Member)user2).reciepts;
            // check for first user
            foreach (Receipt receipt in u1Receipts)
            {
                Assert.AreEqual(receipt.username, username1, "the username is wrong");
                if (receipt.store.name.Equals(storeName) & receipt.actualProducts.Count == 1)
                {
                    LinkedList<Receipt> rAsList = new LinkedList<Receipt>(u1Receipts);
                    LinkedList<Product> products = new LinkedList<Product>(rAsList.First.Value.actualProducts);
                    if (products.First.Value.info.Equals(ProductInfo.getProductInfo("Bamba", "Food", "Osem")) & products.First.Value.amount == 12)
                    {
                        user1HasReceipt = true;
                    }
                }
            }
            // check for user 2
            foreach (Receipt receipt in u2Receipts)
            {
                Assert.AreEqual(receipt.username, username2, "the username is wrong");
                if (receipt.store.name.Equals(storeName) & receipt.actualProducts.Count == 1)
                {
                    LinkedList<Receipt> rAsList = new LinkedList<Receipt>(u2Receipts);
                    LinkedList<Product> products = new LinkedList<Product>(rAsList.First.Value.actualProducts);
                    if (products.First.Value.info.Equals(ProductInfo.getProductInfo("Bamba", "Food", "Osem")) & products.First.Value.amount == 18)
                    {
                        user2HasReceipt = true;
                    }
                }
            }

            Assert.IsTrue(user1HasReceipt);
            //Assert.IsTrue(user2HasReceipt);
        }


        public void userReciptTestBad()
        {
            // check if one of the users got the wrong receipt
            ICollection<Receipt> u1Receipts = ((Member)user1).reciepts;
            ICollection<Receipt> u2Receipts = ((Member)user2).reciepts;

            foreach (Receipt receipt in u1Receipts)
                Assert.AreNotEqual(receipt.username, username2, "user1 got user2's receipt");

            foreach (Receipt receipt in u2Receipts)
                Assert.AreNotEqual(receipt.username, username1, "user2 got user1's receipt");
        }

        public void storeReciptsTest()
        {
            bool user1HasReceipt = false, user2HasReceipt = false;

            ICollection<Receipt> u1Receipts = ((Member)user1).reciepts;
            ICollection<Receipt> u2Receipts = ((Member)user2).reciepts;

            Store store = Stores.searchStore(storeName);
            ICollection<Receipt> storeReceipts = store.getAllReceipts();
            // check if the store has both receipts
            foreach (Receipt receipt in storeReceipts)
            {
                Assert.AreEqual(receipt.store.name, storeName, "wrong store name. expected: " + store.name + ", actual: " + receipt.store.name);
                Assert.AreEqual(receipt.actualProducts.Count, 1);
                Assert.IsTrue(receipt.username.Equals(username1) | receipt.username.Equals(username2));

                if (receipt.username.Equals(username1))
                {
                    LinkedList<Receipt> rAsList = new LinkedList<Receipt>(u1Receipts);
                    LinkedList<Product> products = new LinkedList<Product>(rAsList.First.Value.actualProducts);
                    if (products.First.Value.info.Equals(ProductInfo.getProductInfo("Bamba", "Food", "Osem"))
                        & products.First.Value.amount == 12)
                    {
                        user1HasReceipt = true;
                    }
                }

                if (receipt.username.Equals(username2))
                {
                    LinkedList<Receipt> rAsList = new LinkedList<Receipt>(u1Receipts);
                    LinkedList<Product> products = new LinkedList<Product>(rAsList.First.Value.actualProducts);
                    if (products.First.Value.info.Equals(ProductInfo.getProductInfo("Bamba", "Food", "Osem"))
                        & products.First.Value.amount == 18)
                    {
                        user2HasReceipt = true;
                    }
                }
            }

            Assert.IsTrue(user1HasReceipt);
            Assert.IsTrue(user2HasReceipt);
        }

        /*
        public void storeReciptTestBad()
        {
            bridge.logout();
            bridge.login("recipt2", "Recipt2");


            Receipt reciept = bridge.GetRecieptByUser("StoreRecipt2", "recipt1", new System.DateTime());
            Assert.IsNull(reciept, "managed to get recpit from wrong store");
            //clean up
            bridge.logout();
            bridge.login("recipt1", "Recipt1");

        }
        */
    }
}

