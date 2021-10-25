using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public BindingList<Car> BndCarList { get; set; } = new BindingList<Car>();
        dbHelperSingl dbHelper;
        bool Mode;

        public MainWindow(bool mode)
        {
            InitializeComponent();

            dbHelper = dbHelperSingl.GetInstance();

            dbHelper.GetCarsFromDB(BndCarList);

           DataContext = this;

            Mode = mode;

            if (Mode == true)
            {
                mainGrid.Children.Remove(Menu);
                carsListView.Margin = new Thickness(0);
                carsListView.KeyDown -= carsListView_KeyDown;
            }

            //ContextMenu mnu = new ContextMenu();
            //dbHelper.InsertIntoCarsQuery()
        }
        //private void carsListView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    ContextMenu RightClickCtxMenu = new ContextMenu();

        //    Button ButtonAddCar = new Button();
        //    ButtonAddCar.BorderBrush = Brushes.Black;
        //    ButtonAddCar.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFB8EAFF");
        //    ButtonAddCar.Background = (Brush)new BrushConverter().ConvertFromString("#41B1E1");
        //    ButtonAddCar.MouseEnter += ButtonAddCar_MouseEnter;
        //    ButtonAddCar.MouseLeave += ButtonAddCar_MouseLeave;
        //    ButtonAddCar.MouseDown += ButtonAddCar_MouseDown;
        //    ButtonAddCar.Content = "Добавить машину";

        //    RightClickCtxMenu.Items.Add(ButtonAddCar);

        //    Button ButtonRemoveCar = new Button();
        //    ButtonRemoveCar.BorderBrush = Brushes.Black;
        //    ButtonRemoveCar.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFB8EAFF");
        //    ButtonRemoveCar.Background = (Brush)new BrushConverter().ConvertFromString("#41B1E1");
        //    ButtonRemoveCar.MouseEnter += ButtonRemoveCar_MouseEnter;
        //    ButtonRemoveCar.MouseLeave += ButtonRemoveCar_MouseLeave;
        //    ButtonRemoveCar.MouseDown += ButtonRemoveCar_MouseDown;
        //    ButtonRemoveCar.Content = "Удалить машину";

        //    RightClickCtxMenu.Items.Add(ButtonRemoveCar);

        //    Button ButtonChangeCar = new Button();
        //    ButtonChangeCar.BorderBrush = Brushes.Black;
        //    ButtonChangeCar.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFB8EAFF");
        //    ButtonChangeCar.Background = (Brush)new BrushConverter().ConvertFromString("#41B1E1");
        //    ButtonChangeCar.MouseEnter += ButtonChangeCar_MouseEnter;
        //    ButtonChangeCar.MouseLeave += ButtonChangeCar_MouseLeave;
        //    ButtonChangeCar.MouseDown += ButtonChangeCar_MouseDown;
        //    ButtonChangeCar.Content = "Изменить машину";

        //    RightClickCtxMenu.Items.Add(ButtonChangeCar);

        //    carsListView.ContextMenu = RightClickCtxMenu;
        //}

        //private void ButtonAddCar_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    Button button = sender as Button;
        //    button.Foreground = Brushes.Black;
        //}

        //private void ButtonAddCar_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    Button button = sender as Button;
        //    button.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFB8EAFF");
        //}

        //private void ButtonAddCar_MouseDown(object sender, MouseButtonEventArgs e)
        //{

        //}

        //private void ButtonRemoveCar_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    Button button = sender as Button;
        //    button.Foreground = Brushes.Black;
        //}

        //private void ButtonRemoveCar_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    Button button = sender as Button;
        //    button.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFB8EAFF");
        //}

        //private void ButtonRemoveCar_MouseDown(object sender, MouseButtonEventArgs e)
        //{

        //}

        //private void ButtonChangeCar_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    Button button = sender as Button;
        //    button.Foreground = Brushes.Black;
        //}

        //private void ButtonChangeCar_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    Button button = sender as Button;
        //    button.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFB8EAFF");
        //}

        //private void ButtonChangeCar_MouseDown(object sender, MouseButtonEventArgs e)
        //{

        //}

        private void AddCarMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window2 window2 = new Window2(0, this);
            window2.Title = "Добавление машини";
            window2.ShowDialog();
        }

        private void ChangeCarMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (carsListView.SelectedItem != null)
            {
                Window2 window2 = new Window2(1, this);
                window2.Title = "Изменение машини";
                window2.ShowDialog();
            }
            else
                MessageBox.Show("Не выбран автомобиль", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void RemoveCarMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (carsListView.SelectedItem != null)
            {
                Car carToRemove = carsListView.SelectedItem as Car;

                dbHelper.DeleteFromCars((carToRemove).Id);
                BndCarList.Remove(carToRemove);
            }
            else
                MessageBox.Show("Не выбран автомобиль", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void carsListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (carsListView.SelectedItem != null)
            {
                if (e.Key == Key.Delete)
                {
                    var res = MessageBox.Show("Вы действительно хотите удалить элемент?", "Подтвердите действие", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        Car carToRemove = carsListView.SelectedItem as Car;

                        dbHelper.DeleteFromCars((carToRemove).Id);
                        BndCarList.Remove(carToRemove);
                    }
                    else
                        return;
                }
            }
            else
                MessageBox.Show("Не выбран автомобиль", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void carsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Window2 window2 = new Window2(2, this);
            window2.Show();
        }
    }
}
