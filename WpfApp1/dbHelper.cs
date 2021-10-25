using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    class dbHelperSingl : IDisposable
    {
        private static dbHelperSingl instance;
        private SqlConnection connection;


        private dbHelperSingl()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            Connection(connectionString);
        }

        private void Connection(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
            ;
        }

        public static dbHelperSingl GetInstance()
        {
            if (instance is null)
                instance = new dbHelperSingl();
            return instance;
        }

        public void Dispose()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public bool Auth(string login, string pass)
        {
            SqlCommand auth = new SqlCommand
            {
                CommandText = $"Select Count(*) FROM Account Where login = '{login}' and password = '{pass}'",
                Connection = connection
            };

            int count = (int)auth.ExecuteScalar();

            if (count == 0)
                return false;
            else
            {
                return true;
            }
        }

        public bool IsUser(string Login, string Pass)
        {
            string queryString = "SELECT Status from Account WHERE [Login] = @Login and [Password] = @Password;";

            SqlCommand command = new SqlCommand(queryString, connection);

            SqlParameter login_param = new SqlParameter("@Login", Login);

            command.Parameters.Add(login_param);

            SqlParameter pswd_param = new SqlParameter("@Password", Pass);
            command.Parameters.Add(pswd_param);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var status = reader.GetString(0);
                        if (status == "User")
                        {
                            return true;
                        }                            
                    }
                }
            }
            return false;
        }

        public void InsertIntoAccQuery(string login, string password, string name, string status)
        {
            string query = "INSERT INTO Account(Login, Password, Name, Status) VALUES(@login, @password, @Name, @Status)";

            SqlCommand command = new SqlCommand(query, connection);

            SqlParameter login_param = new SqlParameter("@login", login);
            command.Parameters.Add(login_param);

            SqlParameter pswd_param = new SqlParameter("@password", password);
            command.Parameters.Add(pswd_param);

            SqlParameter name_param = new SqlParameter("@Name", name);
            command.Parameters.Add(name_param);

            SqlParameter status_param = new SqlParameter("@Status", status);
            command.Parameters.Add(status_param);

            command.ExecuteNonQuery();
            //MessageBox.Show($"Количество изменненых строк: {command.ExecuteNonQuery()}");
        }

        public void InsertIntoCarsQuery(string Img = " ", string Name = " ", string Category = " ", decimal Price = 0.0m)
        {
            string query = "INSERT INTO Cars(Img, Name, Category, Price) VALUES(@Img, @Name, @Category, @Price)";

            SqlCommand command = new SqlCommand(query, connection);

            SqlParameter img_param = new SqlParameter("@Img", Img);
            command.Parameters.Add(img_param);

            SqlParameter name_param = new SqlParameter("@Name", Name);
            command.Parameters.Add(name_param);

            SqlParameter category_param = new SqlParameter("@Category", Category);
            command.Parameters.Add(category_param);

            SqlParameter price_param = new SqlParameter("@Price", Price);
            command.Parameters.Add(price_param);

            command.ExecuteNonQuery();
            //MessageBox.Show($"Количество изменненых строк: {command.ExecuteNonQuery()}");
        }

        public void InsertIntoCarsQuery(Car car)
        {
            string query = "INSERT INTO Cars(Img, Name, Category, Price) VALUES(@Img, @Name, @Category, @Price)";

            SqlCommand command = new SqlCommand(query, connection);

            SqlParameter img_param = new SqlParameter("@Img", car.Image);
            command.Parameters.Add(img_param);

            SqlParameter name_param = new SqlParameter("@Name", car.Name);
            command.Parameters.Add(name_param);

            SqlParameter category_param = new SqlParameter("@Category", car.Category);
            command.Parameters.Add(category_param);

            SqlParameter price_param = new SqlParameter("@Price", car.Price);
            command.Parameters.Add(price_param);

            command.ExecuteNonQuery();
            //MessageBox.Show($"Количество изменненых строк: {command.ExecuteNonQuery()}");
        }

        public void UpdateCarsQuery(Car car)
        {
            string query = "Update Cars SET Img = @Img, Name = @Name, Category = @Category, Price = @Price WHERE Id = @Id;";

            SqlCommand command = new SqlCommand(query, connection);

            SqlParameter img_param = new SqlParameter("@Img", car.Image);
            command.Parameters.Add(img_param);

            SqlParameter name_param = new SqlParameter("@Name", car.Name);
            command.Parameters.Add(name_param);

            SqlParameter category_param = new SqlParameter("@Category", car.Category);
            command.Parameters.Add(category_param);

            SqlParameter price_param = new SqlParameter("@Price", car.Price);
            command.Parameters.Add(price_param);

            SqlParameter Id_param = new SqlParameter("@Id", car.Id);
            command.Parameters.Add(Id_param);

            command.ExecuteNonQuery();
            //MessageBox.Show($"Количество изменненых строк: {command.ExecuteNonQuery()}");
        }

        public void DeleteFromCars(int Id)
        {
            string query = $"Delete From Cars Where Id = @Id";

            SqlCommand command = new SqlCommand(query, connection);

            SqlParameter Id_param = new SqlParameter("@Id", Id);
            command.Parameters.Add(Id_param);

            command.ExecuteNonQuery();
        }

        public void GetCarsFromDB(BindingList<Car> bndCarLst)
        {
            string queryString = "SELECT * FROM dbo.Cars;";

            SqlCommand command = new SqlCommand(queryString, connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        bndCarLst.Add(
                            new Car
                            {
                                Id = reader.GetInt32(0),
                                Image = reader.GetString(1),
                                Name = reader.GetString(2),
                                Category = reader.GetString(3),
                                Price = reader.GetDecimal(4)
                            });
                    }
                }
            }
        }
        public int GetCarIdAfterAdd(Car car)
        {
            string query = $"SELECT Id FROM dbo.Cars Where Name = @Name and Category = @Category and Price = @Price;";

            SqlCommand command = new SqlCommand(query, connection);

            SqlParameter name_param = new SqlParameter("@Name", car.Name);
            command.Parameters.Add(name_param);

            SqlParameter category_param = new SqlParameter("@Category", car.Category);
            command.Parameters.Add(category_param);

            SqlParameter price_param = new SqlParameter("@Price", car.Price);
            command.Parameters.Add(price_param);

            int id = 0;

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                    };
                }
            }
            return id;
        }
    }
}
