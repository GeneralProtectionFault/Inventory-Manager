using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Inventory_Manager.Windows
{
    /// <summary>
    /// Interaction logic for ModifyPartWindow.xaml
    /// </summary>
    public partial class ModifyPartWindow : Window
    {
        public ModifyPartWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the part that was selected

            txtModifyPartID.Text = MainWindow.partSelected.PartID.ToString();
            txtModifyPartName.Text = MainWindow.partSelected.Name.ToString();
            txtModifyPartInventory.Text = MainWindow.partSelected.InStock.ToString();
            txtModifyPartPrice.Text = MainWindow.partSelected.Price.ToString();
            txtModifyPartMin.Text = MainWindow.partSelected.Min.ToString();
            txtModifyPartMax.Text = MainWindow.partSelected.Max.ToString();


            if (MainWindow.partSelected.GetType() == typeof(InhousePart))
            {
                Debug.WriteLine("Inhouse part detected");
                radModifyPartInhouse.IsChecked = true;
                radModifyPartOutsourced.IsChecked = false;

                lblModifyPartMachine_Company.Content = "Machine ID";
                txtModifyPartMachine_Company.Text = MainWindow.partSelected.MachineID.ToString();
            }
            else if (MainWindow.partSelected.GetType() == typeof(OutsourcedPart))
            {
                Debug.WriteLine("Outsourced part detected");
                radModifyPartInhouse.IsChecked = false;
                radModifyPartOutsourced.IsChecked = true;

                lblModifyPartMachine_Company.Content = "Company Name";
                txtModifyPartMachine_Company.Text = MainWindow.partSelected.CompanyName;
            }

        }


        private void UpdateLabel()
        {
            if (radModifyPartInhouse.IsChecked == true)
                lblModifyPartMachine_Company.Content = "Machine ID";
            else if (radModifyPartOutsourced.IsChecked == true)
                lblModifyPartMachine_Company.Content = "Company Name";
        }

        private void radModifyPartInhouse_Click(object sender, RoutedEventArgs e)
        {
            UpdateLabel();
        }

        private void radModifyPartOutsourced_Click(object sender, RoutedEventArgs e)
        {
            UpdateLabel();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).Show();
        }

        private void btnModifyPartCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Confirm part modifications as filled out in the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifyPartSave_Click(object sender, RoutedEventArgs e)
        {
            // Make sure number fields are numbers...
            if (!MainWindow.IsNumericValue(txtModifyPartInventory.Text) ||
                !MainWindow.IsDecimalValue(txtModifyPartPrice.Text) ||
                !MainWindow.IsNumericValue(txtModifyPartMin.Text) ||
                !MainWindow.IsNumericValue(txtModifyPartMax.Text))
            {
                MessageBox.Show("Please ensure Inventory, Price, Min & Max values are numbers.");
                return;
            }


            if (Convert.ToInt32(txtModifyPartMax.Text) <= Convert.ToInt32(txtModifyPartMin.Text))
            {
                MessageBox.Show("Max value must be larger than Min value");
                return;
            }

            if (Convert.ToInt32(txtModifyPartMin.Text) > Convert.ToInt32(txtModifyPartInventory.Text) ||
                Convert.ToInt32(txtModifyPartMax.Text) < Convert.ToInt32(txtModifyPartInventory.Text))
            {
                MessageBox.Show("Inventory value must be between Min & Max values.");
                return;
            }

            if (radModifyPartInhouse.IsChecked == true)
            {
                if (!MainWindow.IsNumericValue(txtModifyPartMachine_Company.Text))
                {
                    MessageBox.Show("Machine ID should be a number.");
                    return;
                }
            }




                if (radModifyPartInhouse.IsChecked == true)
            {
                InhousePart newPart = new InhousePart(MainWindow.partSelected.PartID, txtModifyPartName.Text,
                    Convert.ToDecimal(txtModifyPartPrice.Text), Convert.ToInt32(txtModifyPartInventory.Text),
                    Convert.ToInt32(txtModifyPartMin.Text), Convert.ToInt32(txtModifyPartMax.Text), Convert.ToInt32(txtModifyPartMachine_Company.Text));

                MainWindow.viewModel.UpdatePart(MainWindow.partSelected.PartID, newPart);
            }
            else if (radModifyPartOutsourced.IsChecked == true)
            {
                OutsourcedPart newPart = new OutsourcedPart(MainWindow.partSelected.PartID, txtModifyPartName.Text,
                    Convert.ToDecimal(txtModifyPartPrice.Text), Convert.ToInt32(txtModifyPartInventory.Text),
                    Convert.ToInt32(txtModifyPartMin.Text), Convert.ToInt32(txtModifyPartMax.Text),txtModifyPartMachine_Company.Text);

                MainWindow.viewModel.UpdatePart(MainWindow.partSelected.PartID, newPart);
            }

            this.Close();
        }
    }
}
