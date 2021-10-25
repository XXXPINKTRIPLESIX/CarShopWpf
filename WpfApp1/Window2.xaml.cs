using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : MetroWindow
    {
        public int Mode { get; set; }
        MainWindow Parent_ { get; set; }
        public Window2(int mode, MainWindow mainWindow)
        {
            InitializeComponent();
            Mode = mode;
            Parent_ = mainWindow;

            if (Mode == 0)
            {
                window2.Title = "Добавление машины";

                ChngOrAddButton.Content = "Добавить";
                ChngOrAddButton.Click += ChngOrAddButton_AddClick;
            }
            else if (Mode == 1)
            {
                window2.Title = "Изменение машины";

                ChngOrAddButton.Content = "Изменить";
                ChngOrAddButton.Click += ChngOrAddButton_ChangeClick;

                Car selectedCar = (Parent_.carsListView.SelectedItem as Car);

                CarNameTxtBox.Text = selectedCar.Name;
                CategoryTxtBox.Text = selectedCar.Category;
                PriceTxtBox.Text = selectedCar.Price.ToString();
                FullNameOfImageTxtBox.Text = selectedCar.Image;

                ImageOnWindow2.Source = new BitmapImage(new Uri(selectedCar.Image));
            }
            else if (Mode == 2)
            {
                window2.Title = "Просмотр машины";

                btns.Children.Remove(ChngOrAddButton);
                CancelButton.Margin = new Thickness(162, 15, 0, 10);

                Car selectedCar = (Parent_.carsListView.SelectedItem as Car);

                CarNameTxtBox.Text = selectedCar.Name;
                CategoryTxtBox.Text = selectedCar.Category;
                PriceTxtBox.Text = selectedCar.Price.ToString();
                FullNameOfImageTxtBox.Text = selectedCar.Image;

                ImageOnWindow2.Source = new BitmapImage(new Uri(selectedCar.Image));

                CarNameTxtBox.IsReadOnly = true;
                CategoryTxtBox.IsReadOnly = true;
                PriceTxtBox.IsReadOnly = true;
                FullNameOfImageTxtBox.IsReadOnly = true;
            }
        }

        private void OpenFileDialogButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName == "")
            {
                System.Windows.MessageBox.Show("Файл не был выбран.");
            }
            else if (openFileDialog.FileName != "")
            {
                try
                {
                    ImageOnWindow2.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    FullNameOfImageTxtBox.Text = openFileDialog.FileName;
                }
                catch
                {
                    System.Windows.MessageBox.Show("Неверный формат файла.");
                }
            }
        }

        private void ChngOrAddButton_AddClick(object sender, RoutedEventArgs e)
        {
            decimal price = 0;

            try
            {
                price = Decimal.Parse(PriceTxtBox.Text);
            }
            catch (FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Неверная цена", "Ошибка", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Car car = new Car
            {
                Image = FullNameOfImageTxtBox.Text,
                Name = CarNameTxtBox.Text,
                Category = CategoryTxtBox.Text,
                Price = price
            };

            dbHelperSingl.GetInstance().InsertIntoCarsQuery(car);

            car.Id = dbHelperSingl.GetInstance().GetCarIdAfterAdd(car);

            Parent_.BndCarList.Add(car);

            window2.Close();
        }

        private void ChngOrAddButton_ChangeClick(object sender, RoutedEventArgs e)
        {
            Car selectedCar = (Parent_.carsListView.SelectedItem as Car);

            decimal price = 0;

            try
            {
                price = Decimal.Parse(PriceTxtBox.Text);
            }
            catch (FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Неверная цена", "Ошибка", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Car car = new Car
            {
                Id = selectedCar.Id,
                Image = FullNameOfImageTxtBox.Text,
                Name = CarNameTxtBox.Text,
                Category = CategoryTxtBox.Text,
                Price = price
            };

            dbHelperSingl.GetInstance().UpdateCarsQuery(car);

            int idx = Parent_.BndCarList.IndexOf(Parent_.carsListView.SelectedItem as Car);

            Parent_.BndCarList[idx] = car;

            window2.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            window2.Close();
        }
    }
}
