using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Inventory_Manager.Windows
{
    /// <summary>
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = MainWindow.viewModel;
        }


        private void dataCandidateParts_Loaded(object sender, RoutedEventArgs e)
        {
            // Automatically fill the space of the data grid
            foreach (var item in dataCandidateParts.Columns.AsParallel())
            {
                item.MinWidth = item.ActualWidth;
                item.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void dataAssignedParts_Loaded(object sender, RoutedEventArgs e)
        {
            // Automatically fill the space of the data grid
            foreach (var item in dataAssignedParts.Columns.AsParallel())
            {
                item.MinWidth = item.ActualWidth;
                item.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).Show();
        }

        private void btnAddProductCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.viewModel.AssociatedParts.Clear();
            this.Close();
        }


        /// <summary>
        /// Take part selected from the Candidate parts list and add it to the Associated parts list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddProductAddPart_Click(object sender, RoutedEventArgs e)
        {
            Part partToAdd = MainWindow.GetSelectedGridItem(dataCandidateParts);

            if (partToAdd == null)
                return;

            foreach (var item in MainWindow.viewModel.AssociatedParts)
            {
                if (item == partToAdd)
                {
                    MessageBox.Show("Part has already been added!");
                    return;
                }
            }

            MainWindow.viewModel.AssociatedParts.Add(partToAdd);
        }

        private void btnAddProductDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to remove this part from the product?", "Confirm DELETE", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
                return;


            Part partToDelete = MainWindow.GetSelectedGridItem(dataAssignedParts);

            if (partToDelete == null)
                return;

            MainWindow.viewModel.AssociatedParts.Remove(partToDelete);
        }



        /// <summary>
        /// Actually saves the new product to the collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddProductSave_Click(object sender, RoutedEventArgs e)
        {
            //Checks first
            // Make sure number fields are numbers...
            if (!MainWindow.IsNumericValue(txtAddProductInventory.Text) ||
                !MainWindow.IsDecimalValue(txtAddProductPrice.Text) ||
                !MainWindow.IsNumericValue(txtAddProductMin.Text) ||
                !MainWindow.IsNumericValue(txtAddProductMax.Text))
            {
                MessageBox.Show("Please ensure Inventory, Price, Min & Max values are numbers.");
                return;
            }


            if (Convert.ToInt32(txtAddProductMax.Text) <= Convert.ToInt32(txtAddProductMin.Text))
            {
                MessageBox.Show("Max value must be larger than Min value");
                return;
            }

            if (Convert.ToInt32(txtAddProductMin.Text) > Convert.ToInt32(txtAddProductInventory.Text) ||
                Convert.ToInt32(txtAddProductMax.Text) < Convert.ToInt32(txtAddProductInventory.Text))
            {
                MessageBox.Show("Inventory value must be between Min & Max values.");
                return;
            }



            int newProductID = 0;

            // Get new unique ID
            foreach (var item in MainWindow.viewModel.Products)
            {
                if (item.ProductID > newProductID)
                    newProductID = item.ProductID;
            }

            newProductID++;

            Product newProduct = new Product(MainWindow.viewModel.AssociatedParts,
                newProductID, txtAddProductName.Text, Convert.ToDecimal(txtAddProductPrice.Text), Convert.ToInt32(txtAddProductInventory.Text),
                Convert.ToInt32(txtAddProductMin.Text), Convert.ToInt32(txtAddProductMax.Text));

            MainWindow.viewModel.AddProduct(newProduct);
            //MainWindow.viewModel.AssociatedParts.Clear();

            this.Close();
        }



        /// <summary>
        /// Search the Parts collection for matching Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddProductSearchParts_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dataCandidateParts.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)dataCandidateParts.ItemContainerGenerator.ContainerFromIndex(i);
                var cell = WPFExtensions.GetCell(dataCandidateParts, row, 1);
                TextBlock tb = (cell.Content as TextBlock);

                if (tb.Text.ToUpper().Contains(txtAddProductSearch.Text.ToUpper()))
                    row.Background = new SolidColorBrush(Colors.Yellow);
                else
                    row.Background = new SolidColorBrush(Colors.White);
            }
        }
    }
}
