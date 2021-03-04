using System;
using System.Collections.Generic;
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
    /// Interaction logic for ModifyProductWindow.xaml
    /// </summary>
    public partial class ModifyProductWindow : Window
    {
        public ModifyProductWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = MainWindow.viewModel;
        }

        private void btnModifyProductCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtModifyProductID.Text = MainWindow.productSelected.ProductID.ToString();
            txtModifyProductName.Text = MainWindow.productSelected.Name;
            txtModifyProductInventory.Text = MainWindow.productSelected.InStock.ToString();
            txtModifyProductPrice.Text = MainWindow.productSelected.Price.ToString();
            txtModifyProductMin.Text = MainWindow.productSelected.Min.ToString();
            txtModifyProductMax.Text = MainWindow.productSelected.Max.ToString();

            MainWindow.viewModel.AssociatedParts = MainWindow.productSelected.AssociatedParts;
        }

        private void btnModifyProductSave_Click(object sender, RoutedEventArgs e)
        {
            // Make sure number fields are numbers...
            if (!MainWindow.IsNumericValue(txtModifyProductInventory.Text) ||
                !MainWindow.IsDecimalValue(txtModifyProductPrice.Text) ||
                !MainWindow.IsNumericValue(txtModifyProductMin.Text) ||
                !MainWindow.IsNumericValue(txtModifyProductMax.Text))
            {
                MessageBox.Show("Please ensure Inventory, Price, Min & Max values are numbers.");
                return;
            }


            if (Convert.ToInt32(txtModifyProductMax.Text) <= Convert.ToInt32(txtModifyProductMin.Text))
            {
                MessageBox.Show("Max value must be larger than Min value");
                return;
            }

            if (Convert.ToInt32(txtModifyProductMin.Text) > Convert.ToInt32(txtModifyProductInventory.Text) ||
                Convert.ToInt32(txtModifyProductMax.Text) < Convert.ToInt32(txtModifyProductInventory.Text))
            {
                MessageBox.Show("Inventory value must be between Min & Max values.");
                return;
            }


            Product newProduct = new Product(MainWindow.viewModel.AssociatedParts, MainWindow.productSelected.ProductID,
            txtModifyProductName.Text, Convert.ToDecimal(txtModifyProductPrice.Text), Convert.ToInt32(txtModifyProductInventory.Text),
            Convert.ToInt32(txtModifyProductMin.Text), Convert.ToInt32(txtModifyProductMax.Text));

            MainWindow.viewModel.UpdateProduct(MainWindow.productSelected.ProductID, newProduct);

            //MainWindow.viewModel.AssociatedParts.Clear();
            this.Close();
        }

        private void btnModifyProductSearchParts_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dataModCandidateParts.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)dataModCandidateParts.ItemContainerGenerator.ContainerFromIndex(i);
                var cell = WPFExtensions.GetCell(dataModCandidateParts, row, 1);
                TextBlock tb = (cell.Content as TextBlock);

                if (tb.Text.ToUpper().Contains(txtModifyProductSearch.Text.ToUpper()))
                    row.Background = new SolidColorBrush(Colors.Yellow);
                else
                    row.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void btnModifyProductAddPart_Click(object sender, RoutedEventArgs e)
        {
            Part partToAdd = MainWindow.GetSelectedGridItem(dataModCandidateParts);

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

        private void btnModifyProductDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to remove this part from the product?", "Confirm DELETE", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
                return;

            Part partToDelete = MainWindow.GetSelectedGridItem(dataModAssignedParts);

            if (partToDelete == null)
                return;

            MainWindow.viewModel.AssociatedParts.Remove(partToDelete);
        }
    }
}
