using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Inventory_Manager
{
    public class Product
    {
        public ObservableCollection<Part> AssociatedParts { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }



        public Product(ObservableCollection<Part> parts, int productID, string name, decimal price, int inStock, int min, int max)
        {
            AssociatedParts = parts;
            ProductID = productID;
            Name = name;
            Price = price;
            InStock = inStock;
            Min = min;
            Max = max;
        }
    }
}
