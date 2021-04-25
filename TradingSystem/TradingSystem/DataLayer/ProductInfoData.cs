﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.DataLayer
{
    class ProductInfoData
    {
        public string name { get; set; }
        public string category { get; set; }
        public string manufacturer { get; set; }

        public ProductInfoData(string name, string category, string manufacturer)
        {
            this.name = name;
            this.category = category;
            this.manufacturer = manufacturer;
        }

        public bool Equals(ProductInfoData other)
        {
            return this.name.Equals(other.name) & this.category.Equals(other.category) & this.manufacturer.Equals(other.manufacturer);
        }
    }
}
