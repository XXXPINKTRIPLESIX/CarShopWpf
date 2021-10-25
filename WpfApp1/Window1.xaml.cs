using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Configuration;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : MetroWindow
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void ButtonEnter_MouseEnter(object sender, MouseEventArgs e)
        {
            ButtonEnter.Foreground = Brushes.Black;
        }

        private void ButtonEnter_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonEnter.Foreground = Brushes.White;
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            dbHelperSingl dbHelper = dbHelperSingl.GetInstance();

            if (dbHelper.Auth(TextBoxLogin.Text, PassBoxPass.Password) == false)
                wrongAuthLabel.Visibility = Visibility.Visible;
            else
            {
                if (dbHelper.IsUser(TextBoxLogin.Text, PassBoxPass.Password) == true)
                {
                    wrongAuthLabel.Visibility = Visibility.Hidden;
                    MainWindow mainWindow = new MainWindow(true);
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    wrongAuthLabel.Visibility = Visibility.Hidden;
                    MainWindow mainWindow = new MainWindow(false);
                    mainWindow.Show();
                    Close();
                }
            }
        }
    }
}
