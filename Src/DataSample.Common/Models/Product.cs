﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataSample.Common.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string QuantityPerUnit { get; set; }

        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public string CategoryName { get; set; }

        public string SupplierName { get; set; }
    }
}
