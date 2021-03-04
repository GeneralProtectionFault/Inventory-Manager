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
    /// Interaction logic for AddPartWindow.xaml
    /// </summary>
    public partial class AddPartWindow : Window
    {
        public AddPartWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        /// <summary>
        /// Add a part to the collection, using the data entered
        /// Generate a unique ID that is not visible to the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPartSave_Click(object sender, RoutedEventArgs e)
        {
            var maxPartID = 0;

            // Get the existing ID(s)
            foreach (var item in MainWindow.viewModel.AllParts)
            {
                if (item.PartID > maxPartID)
                    maxPartID = item.PartID;
            }

            maxPartID++;

            // Make sure number fields are numbers...
            if (!MainWindow.IsNumericValue(txtAddPartInventory.Text) ||
                !MainWindow.IsDecimalValue(txtAddPartPrice.Text) ||
                !MainWindow.IsNumericValue(txtAddPartMin.Text) ||
                !MainWindow.IsNumericValue(txtAddPartMax.Text))
            {
                MessageBox.Show("Please ensure Inventory, Price, Min & Max values are numbers.");
                return;
            }


            if (radAddPartInhouse.IsChecked == true)
            {
                if (!MainWindow.IsNumericValue(txtAddPartMachine_Company.Text))
                {
                    MessageBox.Show("Machine ID should be a number.");
                    return;
                }

                if (Convert.ToInt32(txtAddPartMax.Text) <= Convert.ToInt32(txtAddPartMin.Text))
                {
                    MessageBox.Show("Max value must be larger than Min value");
                    return;
                }

                if (Convert.ToInt32(txtAddPartMin.Text) > Convert.ToInt32(txtAddPartInventory.Text) ||
                    Convert.ToInt32(txtAddPartMax.Text) < Convert.ToInt32(txtAddPartInventory.Text))
                {
                    MessageBox.Show("Inventory value must be between Min & Max values.");
                    return;
                }



                MainWindow.viewModel.AddPart(new InhousePart(maxPartID, txtAddPartName.Text, Convert.ToDecimal(txtAddPartPrice.Text),
                    Convert.ToInt32(txtAddPartInventory.Text), Convert.ToInt32(txtAddPartMin.Text), Convert.ToInt32(txtAddPartMax.Text), 
                    Convert.ToInt32(txtAddPartMachine_Company.Text)));
            }
            else
            {
                MainWindow.viewModel.AddPart(new OutsourcedPart(maxPartID, txtAddPartName.Text, Convert.ToDecimal(txtAddPartPrice.Text),
                    Convert.ToInt32(txtAddPartInventory.Text), Convert.ToInt32(txtAddPartMin.Text), Convert.ToInt32(txtAddPartMax.Text),
                    txtAddPartMachine_Company.Text));
            }

            (App.Current.MainWindow as MainWindow).Show();
            this.Close();
        }






        private void UpdateLabel()
        {
            if (radAddPartInhouse.IsChecked == true)
                lblAddPartMachine_Company.Content = "Machine ID";
            else if (radAddPartOutsourced.IsChecked == true)
                lblAddPartMachine_Company.Content = "Company Name";
        }


        private void radAddPartInhouse_Click(object sender, RoutedEventArgs e)
        {
            UpdateLabel();
        }

        private void radAddPartOutsourced_Click(object sender, RoutedEventArgs e)
        {
            UpdateLabel();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).Show();
        }

        private void btnAddPartCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

