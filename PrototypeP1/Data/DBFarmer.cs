using PrototypeP1.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;

namespace PrototypeP1.Data
{
    public class DBFarmer
    {

        private string conString;
        private IConfiguration _config;

        public DBFarmer(IConfiguration configuration)
        {
            _config = configuration;
            conString = _config.GetConnectionString("azureDBConnect");
        }

        public List<Farmer> AllFarmers()
        {
            List<Farmer> FarmList = new List<Farmer>();
            using (SqlConnection myConnection = new SqlConnection(conString))
            {
                string query = "SELECT * FROM Farmer";
                using (SqlDataAdapter myAdapter = new SqlDataAdapter(query, myConnection))
                {
                    DataTable myTable = new DataTable();
                    myAdapter.Fill(myTable);

                    foreach (DataRow myRow in myTable.Rows)
                    {
                        string ID = (string)myRow["FarmerID"];
                        string firstName = (string)myRow["Name"];
                        string surname = (string)myRow["Surname"];
                        string username = (string)myRow["Username"];
                        string password = (string)myRow["Password"];
                        string email = (string)myRow["Email"];

                        Farmer farmer = new Farmer(ID,firstName,surname,username,password,email);
                        FarmList.Add(farmer);
                    }
                }
            }
            return FarmList;
        }
        public Farmer GetFarmer(string ID)
        {
            Farmer farm = new Farmer();
            SqlConnection myConnection = new SqlConnection(conString);
            SqlCommand cmdSelect = new SqlCommand($"SELECT * FROM Farmer WHERE FarmerID = '{ID}'", myConnection);

            myConnection.Open();
            using (SqlDataReader reader = cmdSelect.ExecuteReader())
            {
                while (reader.Read())
                {
                    farm = new Farmer((string)reader[0], (string)reader[1], (string)reader[2], (string)reader[3], (string)reader[4], (string)reader[5]);

                }
            }
            myConnection.Close();
            return farm;
        }
        public void AddFarmer(Farmer farm)
        {
            using (SqlConnection myConnection = new SqlConnection(conString))
            {
                SqlCommand cmdInsert = new SqlCommand($"INSERT INTO Farmer " +
     $"VALUES ('{farm.FarmerID}','{farm.first_name}','{farm.last_name}','{farm.username}','{farm.password}','{farm.email}')", myConnection);

                myConnection.Open();
                cmdInsert.ExecuteNonQuery();
            }
        }
        public void UpdateFarmer(string ID, Farmer farm)
        {
            using (SqlConnection myConnection = new SqlConnection(conString))
            {
                SqlCommand cmdInsert = new SqlCommand($"UPDATE Employee SET Name = '{farm.first_name}', " +
                 $"Surname = '{farm.last_name}', " +
                 $"Username = '{farm.username}', " +
                 $"Password = '{farm.username}', " +
                 $"Email = '{farm.email}' " +
                 $"WHERE FarmerID = '{farm.FarmerID}'", myConnection);
                myConnection.Open();
                cmdInsert.ExecuteNonQuery();
            }
        }
        public void DeleteFarmer(string ID)
        {
            using (SqlConnection myConnection = new SqlConnection(conString))
            {
                SqlCommand cmdDelete = new SqlCommand($"DELETE FROM Farmer WHERE FarmerID = '{ID}'", myConnection);
                myConnection.Open();
                cmdDelete.ExecuteNonQuery();
            }
        }
        public Farmer GetFarmerByUsernameAndPassword(string username, string password)
        {
            Farmer farmer = null;
            using (SqlConnection myConnection = new SqlConnection(conString))
            {
                SqlCommand cmdSelect = new SqlCommand($"SELECT * FROM Farmer WHERE Username = '{username}' AND Password = '{password}'", myConnection);

                myConnection.Open();

                using (SqlDataReader reader = cmdSelect.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        farmer = new Farmer
                        {
                            FarmerID = reader.GetString(0),
                            first_name = reader.GetString(1),
                            last_name = reader.GetString(2),
                            username = reader.GetString(3),
                            password = reader.GetString(4),
                            email = reader.GetString(5)
                        };
                    }
                }
            }

            return farmer;
        }
    }
}
