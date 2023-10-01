namespace PrototypeP1.Models
{
    
        public class Farmer
        {
            public string FarmerID { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string email { get; set; }

            public Farmer() { }

            public Farmer(string farmerID, string first_name, string last_name, string username, string password, string email)
            {
                FarmerID = farmerID;
                this.first_name = first_name;
                this.last_name = last_name;
                this.username = username;
                this.password = password;
                this.email = email;
            }
        }
    }

