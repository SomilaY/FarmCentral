namespace PrototypeP1.Models
{
    public class Employee
    {
        public string Employee_ID { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }

        public Employee() { }

        public Employee(string employee_ID, string first_name, string last_name, string username, string password, string email)
        {
            Employee_ID = employee_ID;
            this.first_name = first_name;
            this.last_name = last_name;
            this.username = username;
            this.password = password;
            this.email = email;
        }
    }
}
