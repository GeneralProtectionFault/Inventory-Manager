using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventory_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static Inventory viewModel = new Inventory();
        internal static dynamic partSelected;
        internal static Product productSelected;

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = viewModel;




            // Test to add a PART
            viewModel.AddPart(new OutsourcedPart(1, "Doohickey", (decimal)19.95, 2, 0, 1000, "ACME"));
            viewModel.AddPart(new InhousePart(2, "Wingnut", (decimal)3.99, 4, 0, 22, 829991));
            viewModel.AddPart(new InhousePart(3, "ScrewLoose", (decimal)9.95, 20, 0, 9999, 923847));

            // Test to add a PRODUCT
            ObservableCollection<Part> associated = new ObservableCollection<Part>();
            associated.Add(viewModel.LookupPart(2));
            Product newProduct = new Product(associated, 2, "Illudium Q-38 Space Modulator", (decimal)9999.99, 1, 0, 1);
            viewModel.AddProduct(newProduct);
        }


        public static bool IsNumericValue (string input)
        {
            return int.TryParse(input, out _);
        }

        public static bool IsDecimalValue (string input)
        {
            return decimal.TryParse(input, out _);
        }


        /// <summary>
        /// Pass in a datagrid.  Will return the Part object highlighted
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static Part GetSelectedGridItem(DataGrid grid)
        {
            int partID = -1;

            foreach (var item in grid.SelectedCells)
            {
                // Debug.WriteLine(item.Column.Header);
                if (item.Column.Header.ToString() == "PartID")
                {
                    var cellContent = item.Column.GetCellContent(item.Item);
                    var cell = (DataGridCell)cellContent.Parent;
                    TextBlock tb = cell.Content as TextBlock;
                    partID = Convert.ToInt32(tb.Text);
                }
            }

            if (partID == -1)
            {
                MessageBox.Show("Please select an item from the grid to modify.");
                return null;
            }

            return MainWindow.viewModel.LookupPart(partID);
        }

        public static Product GetSelectedProductGridItem(DataGrid grid)
        {
            int productID = -1;

            foreach (var item in grid.SelectedCells)
            {
                // Debug.WriteLine(item.Column.Header);
                if (item.Column.Header.ToString() == "ProductID")
                {
                    var cellContent = item.Column.GetCellContent(item.Item);
                    var cell = (DataGridCell)cellContent.Parent;
                    TextBlock tb = cell.Content as TextBlock;
                    productID = Convert.ToInt32(tb.Text);
                }
            }

            if (productID == -1)
            {
                MessageBox.Show("Please select an item from the grid to modify.");
                return null;
            }

            return MainWindow.viewModel.LookupProduct(productID);
        }


       



        private void dataProducts_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            // Hide the associated parts column--not to be shown
            switch (e.Column.Header.ToString())
            {
                case "AssociatedParts":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
            }

        }

        
        /// <summary>
        /// Bring up the Add Part Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPart_Click(object sender, RoutedEventArgs e)
        {
            var addPartWindow = new Windows.AddPartWindow();
            addPartWindow.Show();
            this.Hide();
        }

        private void dataParts_Loaded(object sender, RoutedEventArgs e)
        {
            // Automatically fill the space of the data grid
            foreach (var item in dataParts.Columns.AsParallel())
            {
                item.MinWidth = item.ActualWidth;
                item.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void dataProducts_Loaded(object sender, RoutedEventArgs e)
        {
            // Automatically fill the space of the data grid
            foreach (var item in dataProducts.Columns.AsParallel())
            {
                item.MinWidth = item.ActualWidth;
                item.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        
        /// <summary>
        /// Whatever part (if any) is selected in the DataGrid, launch the ModifyPartWindow w/ that info for....modifying...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifyPart_Click(object sender, RoutedEventArgs e)
        {
            // Get part by id, then determine the dynamic field & type by the COLLECTION
            partSelected = GetSelectedGridItem(dataParts);

            if (partSelected == null)
                return;

            viewModel.AssociatedParts.Clear();

            var modifyPartWindow = new Windows.ModifyPartWindow();
            modifyPartWindow.Show();
            this.Hide();
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AssociatedParts.Clear();
            var addProductWindow = new Windows.AddProductWindow();
            addProductWindow.Show();
            this.Hide();
        }

        private void btnModifyProduct_Click(object sender, RoutedEventArgs e)
        {
            productSelected = GetSelectedProductGridItem(dataProducts);

            if (productSelected == null)
                return;

            // viewModel.AssociatedParts = productSelected.AssociatedParts;

            var modifyProductWindow = new Windows.ModifyProductWindow();
            modifyProductWindow.Show();
            this.Hide();
        }

        private void btnDeletePart_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this part?", "Confirm DELETE", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
                return;


            partSelected = GetSelectedGridItem(dataParts);

            // Ensure no product has this particular part associated with it
            foreach (var product in viewModel.Products)
            {
                foreach (var part in product.AssociatedParts)
                {
                    if (part == partSelected)
                    {
                        MessageBox.Show("Cannot delete this part.  It is associated with 1 or more existing products.");
                        return;
                    }
                }
            }

            if (partSelected == null)
                return;

            viewModel.DeletePart(partSelected);
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this product?", "Confirm DELETE", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
                return;


            productSelected = GetSelectedProductGridItem(dataProducts);

            if (productSelected.AssociatedParts.Count > 0)
            {
                MessageBox.Show("Cannot delete this product.  It has 1 or more parts associated with it.");
                return;
            }




            if (productSelected == null)
                return;

            viewModel.RemoveProduct(productSelected.ProductID);
        }

        private void btnPartSearch_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dataParts.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)dataParts.ItemContainerGenerator.ContainerFromIndex(i);
                var cell = WPFExtensions.GetCell(dataParts, row, 1);
                TextBlock tb = (cell.Content as TextBlock);

                if (tb.Text.ToUpper().Contains(txtPartSearch.Text.ToUpper()))
                    row.Background = new SolidColorBrush(Colors.Yellow);
                else
                    row.Background = new SolidColorBrush(Colors.White);
            } 
        }

        private void btnProductSearch_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dataProducts.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)dataProducts.ItemContainerGenerator.ContainerFromIndex(i);
                var cell = WPFExtensions.GetCell(dataProducts, row, 2);
                TextBlock tb = (cell.Content as TextBlock);

                if (tb.Text.ToUpper().Contains(txtProductSearch.Text.ToUpper()))
                    row.Background = new SolidColorBrush(Colors.Yellow);
                else
                    row.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
