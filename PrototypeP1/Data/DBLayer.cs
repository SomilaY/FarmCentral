using PrototypeP1.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;

namespace PrototypeP1.Data
{
    public class DBLayer
    {
        private string conString;
        private IConfiguration _config;

        public DBLayer(IConfiguration configuration)
        {
            _config = configuration;
            conString = _config.GetConnectionString("azureDBConnect");
        }

        public List<Employee> AllEmployees()
        {
            List<Employee> empList = new List<Employee>();

            using (SqlConnection myConnection = new SqlConnection(conString))
            {
                string query = "SELECT * FROM Employee";

                using (SqlDataAdapter myAdapter = new SqlDataAdapter(query, myConnection))
                {
                    DataTable myTable = new DataTable();
                    myAdapter.Fill(myTable);

                    foreach (DataRow myRow in myTable.Rows)
                    {
                        string ID = (string)myRow["EmployeeID"];
                        string firstName = (string)myRow["Name"];
                        string surname = (string)myRow["Surname"];
                        string username = (string)myRow["Username"];
                        string password = (string)myRow["Password"];
                        string email = (string)myRow["Email"];

                        Employee employee = new Employee(ID, firstName, surname, username, password, email);
                        empList.Add(employee);
                    }
                }
            }

            return empList;
        }

        public Employee GetEmployee(string ID)
        {
            Employee emp = new Employee();
            SqlConnection myConnection = new SqlConnection(conString);
            SqlCommand cmdSelect = new SqlCommand($"SELECT * FROM Employee WHERE EmployeeID = '{ID}'", myConnection);

            myConnection.Open();

            using (SqlDataReader reader = cmdSelect.ExecuteReader())
            {
                while (reader.Read())
                {
                    emp = new Employee((string)reader[0], (string)reader[1], (string)reader[2], (string)reader[3], (string)reader[4], (string)reader[5]);

                }
            }
            myConnection.Close();
            return emp;
        }

        public void AddEmployee(Employee emp)
        {
            using (SqlConnection myConnection = new SqlConnection(conString))
            {
                SqlCommand cmdInsert = new SqlCommand($"INSERT INTO Employee " +
     $"VALUES ('{emp.Employee_ID}','{emp.first_name}','{emp.last_name}','{emp.username}','{emp.password}','{emp.email}')", myConnection);

                myConnection.Open();
                cmdInsert.ExecuteNonQuery();
            }
        }

        public void UpdateEmployee(string ID, Employee emp)
        {
            using (SqlConnection myConnection = new SqlConnection(conString))
            {
                SqlCommand cmdInsert = new SqlCommand($"UPDATE Employee SET Name = '{emp.first_name}', " +
                 $"Surname = '{emp.last_name}', " +
                 $"Username = '{emp.username}', " +
                 $"Password = '{emp.password}', " +
                 $"Email = '{emp.email}' " +
                 $"WHERE EmployeeID = '{emp.Employee_ID}'", myConnection);
                myConnection.Open();
                cmdInsert.ExecuteNonQuery();
            }
        }

        public void DeleteEmployee(string ID)
        {
            using (SqlConnection myConnection = new SqlConnection(conString))
            {
                SqlCommand cmdDelete = new SqlCommand($"DELETE FROM Employee WHERE EmployeeID = '{ID}'", myConnection);
                myConnection.Open();
                cmdDelete.ExecuteNonQuery();
            }
        }

        public Employee GetEmployeeByUsernameAndPassword(string username, string password)
        {
            Employee emp = null;
            using (SqlConnection myConnection = new SqlConnection(conString))
            {
                SqlCommand cmdSelect = new SqlCommand($"SELECT * FROM Employee WHERE Username = '{username}' AND Password = '{password}'", myConnection);

                myConnection.Open();

                using (SqlDataReader reader = cmdSelect.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        emp = new Employee
                        {
                            Employee_ID = reader.GetString(0),
                            first_name = reader.GetString(1),
                            last_name = reader.GetString(2),
                            username = reader.GetString(3),
                            password = reader.GetString(4),
                            email = reader.GetString(5)
                        };
                    }
                }
            }

            return emp;
        }

    }
}
