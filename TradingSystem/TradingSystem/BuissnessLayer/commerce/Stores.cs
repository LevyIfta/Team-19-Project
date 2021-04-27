﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.DataLayer;
using TradingSystem.BuissnessLayer.commerce;

namespace TradingSystem.BuissnessLayer
{
    public static class Stores

    {
        public static ICollection<Store> stores = new LinkedList<Store>();
        public static bool addStore(string storeName, Member founder)
        {
            Store newStore = new Store(storeName, founder);
            // check for a name duplicant
            if (stores.Contains(newStore))
                return false;
            stores.Add(newStore);
            // add the new store to DB
            StoreDAL.addStore(newStore.toDataObject());
            return true;

        }

        public static Store searchStore(string storeName)
        {
            foreach (Store store in stores)
                if (store.name.Equals(storeName))
                    return store;
            return null;

        }

        public static void removeStore(Store store)
        {
            stores.Remove(store);
            // update DB TODO
            store.remove();
        }

        public static ICollection<Store> getAllStores()
        {
            return stores;
        }

        public static Dictionary<Store, Product> searchProduct(string productName, string manufacturer)
        {
            Dictionary<Store, Product> result = new Dictionary<Store, Product>();
            foreach (Store store in stores)
                if (store.searchProduct(productName, manufacturer) != null)
                    result.Add(store, store.searchProduct(productName, manufacturer));
            return result;
        }
        public static Dictionary<Store, Product> searchProduct(string productName, string category, string manufacturer, double minPrice, double maxPrice)
        {
            Dictionary<Store, Product> result = new Dictionary<Store, Product>();
            foreach (Store store in stores)
                if (store.searchProduct(productName, manufacturer) != null)
                    result.Add(store, store.searchProduct(productName, category, manufacturer, minPrice, maxPrice));
            return result;
        }

    }
}