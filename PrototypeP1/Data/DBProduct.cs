using PrototypeP1.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.CodeAnalysis;
using System;

namespace PrototypeP1.Data
{
    public class DBProduct
    {
        private string conString;
        private IConfiguration _config;

        public DBProduct(IConfiguration configuration)
        {
            _config = configuration;
            conString = _config.GetConnectionString("azureDBConnect");
        }

        public List<Product> AllProducts()
        {
            List<Product> ProductList = new List<Product>();

            using (SqlConnection myConnection = new SqlConnection(conString))
            {
                string query = "SELECT * FROM Product";

                using (SqlDataAdapter myAdapter = new SqlDataAdapter(query, myConnection))
                {
                    DataTable myTable = new DataTable();
                    myAdapter.Fill(myTable);

                    foreach (DataRow myRow in myTable.Rows)
                    {
                        string productID = (string)myRow["ProductID"];
                        string productName = (string)myRow["ProductName"];
                        string productDescription = (string)myRow["ProductDescription"];
                        decimal productPrice = Convert.ToDecimal(myRow["ProductPrice"]);
                        int productQuantity = (int)myRow["ProductQuantity"];
                        string productSupplier = (string)myRow["ProductSupplier"];
                        string addedBy = (string)myRow["AddedBy"];
                        DateTime dateAdded = ((DateTime)myRow["DateAdded"]).Date;

                        Product product = new Product(productID, productName, productDescription, productPrice, productQuantity, productSupplier, addedBy, dateAdded);
                        ProductList.Add(product);
                    }
                }
            }
            return ProductList;

        }

        public Product GetProduct(string ID)
        {
            Product prod = new Product();
            using (SqlConnection myConnection = new SqlConnection(conString))
            {
                SqlCommand cmdSelect = new SqlCommand($"SELECT * FROM Product WHERE ProductID = '{ID}'", myConnection);
                myConnection.Open();
                using (SqlDataReader reader = cmdSelect.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        prod = new Product
                        {
                            ProductID = reader.GetString(0),
                            ProductName = reader.GetString(1),
                            ProductDescription = reader.GetString(2),
                            ProductPrice = reader.GetDecimal(3),
                            ProductQuantity = reader.GetInt32(4),
                            ProductSupplier = reader.GetString(5),
                            AddedBy = reader.GetString(6),
                            DateAdded = reader.GetDateTime(7)
                        };
                    }
                }
            }
            return prod;
        }

        public void AddProduct(Product prod)
        {
            using (SqlConnection myConnection = new SqlConnection(conString))
            {
                SqlCommand cmdInsert = new SqlCommand($"INSERT INTO Product (ProductID, ProductName, ProductDescription, ProductPrice, ProductQuantity, ProductSupplier, AddedBy, DateAdded) " +
        $"VALUES ('{prod.ProductID}', '{prod.ProductName}', '{prod.ProductDescription}', {prod.ProductPrice}, {prod.ProductQuantity}, '{prod.ProductSupplier}', '{prod.AddedBy}', @DateAdded)", myConnection);

                cmdInsert.Parameters.AddWithValue("@DateAdded", prod.DateAdded);


                myConnection.Open();
                cmdInsert.ExecuteNonQuery();
            }
        }

        public void UpdateProduct(string ID, Product prod)
        {
            using (SqlConnection myConnection = new SqlConnection(conString))
            {
                SqlCommand cmdUpdate = new SqlCommand($"UPDATE Product SET " +
         $"ProductName = '{prod.ProductName}', " +
         $"ProductDescription = '{prod.ProductDescription}', " +
         $"ProductPrice = {prod.ProductPrice}, " +
         $"ProductQuantity = {prod.ProductQuantity}, " +
         $"ProductSupplier = '{prod.ProductSupplier}', " +
         $"AddedBy = '{prod.AddedBy}', " +
         $"DateAdded = '{prod.DateAdded.ToString("yyyy-MM")}' " +
         $"WHERE ProductID = '{prod.ProductID}'", myConnection);

                myConnection.Open();
                cmdUpdate.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(string ID)
        {
            using (SqlConnection myConnection = new SqlConnection(conString))
            {
                SqlCommand cmdDelete = new SqlCommand($"DELETE FROM Product WHERE ProductID = '{ID}'", myConnection);
                myConnection.Open();
                cmdDelete.ExecuteNonQuery();
            }
        }
        public List<Product> GetProductsBySupplier(string supplier, DateTime? startDate, DateTime? endDate)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection myConnection = new SqlConnection(conString))
            {
                string query = "SELECT * FROM Product WHERE ProductSupplier LIKE @Supplier";

                if (startDate != null && endDate != null)
                {
                    query += " AND DateAdded >= @StartDate AND DateAdded <= @EndDate";
                }

                SqlCommand cmdSelect = new SqlCommand(query, myConnection);
                cmdSelect.Parameters.AddWithValue("@Supplier", "%" + supplier + "%");

                if (startDate != null && endDate != null)
                {
                    cmdSelect.Parameters.AddWithValue("@StartDate", startDate.Value.Date);
                    cmdSelect.Parameters.AddWithValue("@EndDate", endDate.Value.Date.AddDays(1).AddSeconds(-1));
                }

                myConnection.Open();

                using (SqlDataReader reader = cmdSelect.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product prod = new Product
                        {
                            ProductID = reader.GetString(0),
                            ProductName = reader.GetString(1),
                            ProductDescription = reader.GetString(2),
                            ProductPrice = reader.GetDecimal(3),
                            ProductQuantity = reader.GetInt32(4),
                            ProductSupplier = reader.GetString(5),
                            AddedBy = reader.GetString(6),
                            DateAdded = reader.GetDateTime(7)
                        };

                        products.Add(prod);
                    }
                }
            }

            return products;
        }


    }
}





