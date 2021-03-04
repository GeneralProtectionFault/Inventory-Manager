using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Inventory_Manager
{
    class Inventory : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }




        private ObservableCollection<Product> _Products = new ObservableCollection<Product>();
        public ObservableCollection<Product> Products
        { 
            get { return _Products; }
            set
            {
                _Products = value;
                OnPropertyChanged("Products");
            }
        
        }

        private ObservableCollection<Part> _AllParts = new ObservableCollection<Part>();
        public ObservableCollection<Part> AllParts
        {
            get { return _AllParts; }
            set
            {
                _AllParts = value;
                OnPropertyChanged("AllParts");
            }
        }


        private ObservableCollection<Part> _AssociatedParts = new ObservableCollection<Part>();
        public ObservableCollection<Part> AssociatedParts
        {
            get { return _AssociatedParts; }
            set
            {
                _AssociatedParts = value;
                OnPropertyChanged("AssociatedParts");
            }
        }
        





        internal void AddProduct(Product product)
        {
            Products.Add(product);
        }

        internal bool RemoveProduct(int productID)
        {
            // Check?
            Products.Remove(Products.Where(x => x.ProductID == productID).FirstOrDefault());
            return true;
        }

        internal Product LookupProduct(int productID)
        {
            return Products.Where(x => x.ProductID == productID).FirstOrDefault();
        }

        internal void UpdateProduct(int productID, Product product)
        {
            Products.Remove(Products.Where(x => x.ProductID == productID).FirstOrDefault());
            Products.Add(product);
        }

        internal void AddPart(Part part)
        {
            AllParts.Add(part);
        }

        internal bool DeletePart(Part part)
        {
            // Check if part exists - return false if not?
            AllParts.Remove(part);
            return true;
        }

        internal Part LookupPart(int partID)
        {
            return AllParts.Where(x => x.PartID == partID).FirstOrDefault();
        }

        internal void UpdatePart(int partID, Part part)
        {
            AllParts.Remove(AllParts.Where(x => x.PartID == partID).FirstOrDefault());
            AllParts.Add(part);
        }


    }
}
