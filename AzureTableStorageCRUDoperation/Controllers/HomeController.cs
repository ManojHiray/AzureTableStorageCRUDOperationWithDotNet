using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureTableStorageCRUDoperation
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(string id)
        {
            // for edit view
            if (!string.IsNullOrEmpty(id))
            {
                // Set the name of the table
                TableManager TableManagerObj = new TableManager("person");

                // Retrieve the student object where RowKey eq value of id
                List<Student> SutdentListObj = TableManagerObj.RetrieveEntity<Student>("RowKey eq '" + id + "'");

                Student StudentObj = SutdentListObj.FirstOrDefault();
                return View(StudentObj);
            }

            // new entry view
            return View(new Student());
        }

        [HttpPost]
        public ActionResult Index(string id, FormCollection formData)
        {
            Student StudentObj = new Student();
            StudentObj.Name = formData["Name"] == "" ? null : formData["Name"];
            StudentObj.Department = formData["Department"] == "" ? null : formData["Department"];
            StudentObj.Email = formData["Email"] == "" ? null : formData["Email"];

            // Insert
            if (string.IsNullOrEmpty(id))
            {
                StudentObj.PartitionKey = "Student";
                StudentObj.RowKey = Guid.NewGuid().ToString();

                TableManager TableManagerObj = new TableManager("person");
                TableManagerObj.InsertEntity<Student>(StudentObj, true);
            }
            // Update
            else
            {
                StudentObj.PartitionKey = "Student";
                StudentObj.RowKey = id;

                TableManager TableManagerObj = new TableManager("person");
                TableManagerObj.InsertEntity<Student>(StudentObj, false);
            }

            return RedirectToAction("Get");
        }

        public ActionResult Get()
        {
            TableManager TableManagerObj = new TableManager("person");
            // Get all Student object, pass null as query
            List<Student> SutdentListObj = TableManagerObj.RetrieveEntity<Student>(null);
            return View(SutdentListObj);
        }

        public ActionResult ChangeStatus(string id, bool Status)
        {
            TableManager TableManagerObj = new TableManager("person");
            List<Student> SutdentListObj = TableManagerObj.RetrieveEntity<Student>("RowKey eq '" + id + "'");
            Student StudentObj = SutdentListObj.FirstOrDefault();
            StudentObj.IsActive = !Status;
            TableManagerObj.InsertEntity<Student>(StudentObj, false);

            return RedirectToAction("Get");
        }

        public ActionResult Delete(string id)
        {
            // Retrieve the object to Delete
            TableManager TableManagerObj = new TableManager("person");
            List<Student> SutdentListObj = TableManagerObj.RetrieveEntity<Student>("RowKey eq '" + id + "'");
            Student StudentObj = SutdentListObj.FirstOrDefault();

            // Delete the object
            TableManagerObj.DeleteEntity<Student>(StudentObj);
            return RedirectToAction("Get");
        }
    }
}