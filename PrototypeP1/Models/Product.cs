namespace PrototypeP1.Models
{
    public class Product
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public string ProductSupplier { get; set; }
        public string AddedBy { get; set; }  
        public DateTime DateAdded { get; set; }

        public Product()
        {

        }

        public Product(string productID, string productName, string productDescription, decimal productPrice, int productQuantity, string productSupplier, string addedby, DateTime dateadded)
        {
            ProductID = productID;
            ProductName = productName;
            ProductDescription = productDescription;
            ProductPrice = productPrice;
            ProductQuantity = productQuantity;
            ProductSupplier = productSupplier;
            AddedBy = addedby;
            DateAdded = dateadded;
        }
    }
}
