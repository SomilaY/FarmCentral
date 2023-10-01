using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrototypeP1.Data;
using PrototypeP1.Models;

namespace PrototypeP1.Controllers
{
    public class EmployeeController : Controller
    {
        DBLayer dbHelper;
        DBFarmer dbFarmer;
        DBProduct dBProduct;

        public EmployeeController(IConfiguration config)
        {
            dbHelper = new DBLayer(config);
            dbFarmer = new DBFarmer(config);
            dBProduct= new DBProduct(config);

        }
        // GET: EmployeeController

        public ActionResult ProductIndex(string supplier, DateTime? startDate, DateTime? endDate)
        {
            List<Product> prodList = dBProduct.GetProductsBySupplier(supplier, startDate, endDate);

            if (!string.IsNullOrEmpty(supplier))
            {
                ViewBag.SupplierFilter = supplier;
            }
            else
            {
                ViewBag.SupplierFilter = string.Empty; // Empty the filter value in ViewBag
            }

            // Apply date range filter
            if (startDate != null && endDate != null)
            {
                ViewBag.StartDateFilter = startDate.Value.Date;
                ViewBag.EndDateFilter = endDate.Value.Date;
            }
            else
            {
                ViewBag.StartDateFilter = null;
                ViewBag.EndDateFilter = null;
            }

            return View(prodList);
        }

        public ActionResult Index()
        {
            List<Employee> empList = dbHelper.AllEmployees();
            return View(empList);
        }

        public ActionResult FarmerIndex()
        {
            List<Farmer> farmList = dbFarmer.AllFarmers();
            return View(farmList);
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(string id)
        {
            Employee emp = dbHelper.GetEmployee(id);
            return View(emp);

        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                string empID = (collection["txtEmployeeID"]);
                string firstname = collection["txtFirstName"];
                string lastname = collection["txtLastName"];
                string username = collection["txtUsername"];
                string password = collection["txtPassword"];
                string email = collection["txtEmail"];

                Employee emp = new Employee(empID, firstname, lastname, username, password, email);
                dbHelper.AddEmployee(emp);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(string id)
        {
            Employee emp = dbHelper.GetEmployee(id);
            return View(emp);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                string firstname = collection["txtName"];
                string lastname = collection["txtSurname"];
                string username = collection["txtUsername"];
                string password = collection["txtPassword"];
                string email = collection["txtEmail"];
                Employee emp = new Employee(id, firstname, lastname, username, password, email);
                dbHelper.UpdateEmployee(id, emp);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(string id)
        {
            Employee emp = dbHelper.GetEmployee(id);
            return View(emp);
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                dbHelper.DeleteEmployee(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Details/5
        public ActionResult FarmerDetails(string id)
        {
            Farmer farmer = dbFarmer.GetFarmer(id);
            return View(farmer);
        }

        public ActionResult CreateFarmer()
        {
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFarmer(IFormCollection collection)
        {
            try
            {
                string farmerID = collection["txtFarmerID"];
                string firstname = collection["txtFirstName"];
                string lastname = collection["txtLastName"];
                string username = collection["txtUsername"];
                string password = collection["txtPassword"];
                string email = collection["txtEmail"];

                Farmer farmer = new Farmer(farmerID, firstname, lastname, username, password, email);
                dbFarmer.AddFarmer(farmer);
                return RedirectToAction(nameof(FarmerIndex));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult EditFarmer(string id)
        {
            Farmer farmer = dbFarmer.GetFarmer(id);
            return View(farmer);
        }
        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFarmer(string id, IFormCollection collection)
        {
            try
            {
                string firstname = collection["txtName"];
                string lastname = collection["txtSurname"];
                string username = collection["txtUsername"];
                string password = collection["txtPassword"];
                string email = collection["txtEmail"];

                Farmer farmer = new Farmer(id, firstname, lastname, username, password, email);
                dbFarmer.UpdateFarmer(id, farmer);
                return RedirectToAction(nameof(FarmerIndex));
            }
            catch
            {
                return View();
            }
        }
        // GET: EmployeeController/Delete/5
        public ActionResult DeleteFarmer(string id)
        {
            Farmer farmer = dbFarmer.GetFarmer(id);
            return View(farmer);
        }
        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFarmer(string id, IFormCollection collection)
        {
            try
            {
                dbFarmer.DeleteFarmer(id);
                return RedirectToAction(nameof(FarmerIndex));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login model)
        {
            Employee employee = dbHelper.GetEmployeeByUsernameAndPassword(model.Username, model.Password);
            if (employee != null)
            {
                // Login successful, redirect to the employee index
                return RedirectToAction("Index");
            }

            Farmer farmer = dbFarmer.GetFarmerByUsernameAndPassword(model.Username, model.Password);
            if (farmer != null)
            {
                // Login successful, redirect to the farmer index
                return RedirectToAction("Index", "Product");

            }

            // Invalid credentials, show error message
            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }

    }
}
