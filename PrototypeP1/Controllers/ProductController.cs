using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrototypeP1.Data;
using PrototypeP1.Models;

namespace PrototypeP1.Controllers
{
    public class ProductController : Controller
    {
        DBProduct dbproduct;

        public ProductController(IConfiguration config)
        {
            dbproduct = new DBProduct(config);
        }
        // GET: ProductController
        public ActionResult Index(string supplier, DateTime? startDate, DateTime? endDate)
        {
            List<Product> prodList;

            if (!string.IsNullOrEmpty(supplier) || (startDate != null && endDate != null))
            {
                prodList = dbproduct.GetProductsBySupplier(supplier, startDate, endDate);
            }
            else
            {
                prodList = dbproduct.AllProducts();
            }

            return View(prodList);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(string id)
        {
            Product ProdList = dbproduct.GetProduct(id);
            return View(ProdList);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                string ProductID = collection["PrID"];
                string ProductName = collection["PrName"];
                string ProductDescription = collection["PrDesc"];
                decimal ProductPrice = Convert.ToDecimal(collection["PrPrice"]);
                int ProductQuantity = Convert.ToInt32(collection["PrQuantity"]);
                string ProductSupplier = collection["PrSupp"];
                string AddedBy = collection["PrAdd"];
                DateTime DateAdded = DateTime.Now;

                Product Prod = new Product(ProductID, ProductName, ProductDescription, ProductPrice, ProductQuantity, ProductSupplier, AddedBy, DateAdded);
                dbproduct.AddProduct(Prod);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the product: " + ex.Message);
                return View();
            }

        }
    

        // GET: ProductController/Edit/5
        public ActionResult Edit(string id)
        {
            Product prod = dbproduct.GetProduct(id);
            return View(prod);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                string ProductName = collection["PrName"];
                string ProductDescription = collection["PrDesc"];
                decimal ProductPrice = Convert.ToDecimal(collection["PrPrice"]);
                int ProductQuantity = Convert.ToInt32(collection["PrQuantity"]);
                string ProductSupplier = collection["PrSupp"];
                string AddedBy = collection["PrAdd"];
                DateTime DateAdded = Convert.ToDateTime(collection["PrDA"]);

                Product prod = new Product(id, ProductName, ProductDescription, ProductPrice, ProductQuantity, ProductSupplier,AddedBy,DateAdded);
                dbproduct.UpdateProduct(id,prod); 
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the product: " + ex.Message);
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(string id)
        {
            Product prod = dbproduct.GetProduct(id);
            return View(prod);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                dbproduct.DeleteProduct(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
