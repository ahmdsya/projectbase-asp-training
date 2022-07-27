using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectBase.Controllers
{
    public class AjaxController : Controller
    {
        ProjectBaseEntities db = new ProjectBaseEntities();
        public ActionResult GroupList(string q, string initValue = "",int page = 0)
        {
            int fromIdx = page > 0 ? (page * 10) : 0;
            int toIdx = page > 0 ? page * 10 : 10;
            var listData = db.Groups.Where(r => 1 == 2);

            if (!string.IsNullOrEmpty(initValue))
            {
                listData = db.Groups
                .Where
                (
                    r =>
                    (string.IsNullOrEmpty(initValue) || (initValue == r.Id.ToString()))
                )
                .OrderBy(r => r.Group_Code);
            }
            else
            {
                listData = db.Groups
                .Where
                (
                    r =>
                    (string.IsNullOrEmpty(q) || ("[" + r.Group_Code + "] " + r.Group_Description).ToUpper().Contains(q.ToUpper()))
                )
                .OrderBy(r => r.Group_Code);
            }
            var data = listData.Skip(fromIdx)
                .Take(toIdx)
                .Select(r => new { id = r.Id, text = "[" + r.Group_Code + "] " + r.Group_Description })
                .ToList();

            return Json(new { items = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MenuList(string q, string initValue = "", int page = 0)
        {
            int fromIdx = page > 0 ? (page * 10) : 0;
            int toIdx = page > 0 ? page * 10 : 10;
            var listData = db.Menus.Where(r => 1 == 2);

            if (!string.IsNullOrEmpty(initValue))
            {
                listData = db.Menus
                .Where
                (
                    r =>
                    (string.IsNullOrEmpty(initValue) || (initValue == r.Id.ToString()))
                )
                .OrderBy(r => r.Id) ;
            }
            else
            {
                listData = db.Menus
                .Where
                (
                    r =>
                    (string.IsNullOrEmpty(q) || (r.Menu_Name).ToUpper().Contains(q.ToUpper()))
                )
                .OrderBy(r => r.Id);
            }
            var data = listData.Skip(fromIdx)
                .Take(toIdx)
                .Select(r => new { id = r.Id, text = r.Menu_Name })
                .ToList();

            return Json(new { items = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CustomerList(string q, string initValue = "", int page = 0)
        {
            int fromIdx = page > 0 ? (page * 10) : 0;
            int toIdx = page > 0 ? page * 10 : 10;
            var listData = db.Customers.Where(r => 1 == 2);

            if (!string.IsNullOrEmpty(initValue))
            {
                listData = db.Customers
                .Where
                (
                    r =>
                    (string.IsNullOrEmpty(initValue) || (initValue == r.Id.ToString()))
                )
                .OrderBy(r => r.Id);
            }
            else
            {
                listData = db.Customers
                .Where
                (
                    r =>
                    (string.IsNullOrEmpty(q) || (r.Code).ToUpper().Contains(q.ToUpper()))
                )
                .OrderBy(r => r.Id);
            }
            var data = listData.Skip(fromIdx)
                .Take(toIdx)
                .Select(r => new { id = r.Id, text = "[" + r.Code + "] " + r.Name })
                .ToList();

            return Json(new { items = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductList(string q, string initValue = "", int page = 0)
        {
            int fromIdx = page > 0 ? (page * 10) : 0;
            int toIdx = page > 0 ? page * 10 : 10;
            var listData = db.Products.Where(r => 1 == 2);

            if (!string.IsNullOrEmpty(initValue))
            {
                listData = db.Products
                .Where
                (
                    r =>
                    (string.IsNullOrEmpty(initValue) || (initValue == r.Id.ToString()))
                )
                .OrderBy(r => r.Id);
            }
            else
            {
                listData = db.Products
                .Where
                (
                    r =>
                    (string.IsNullOrEmpty(q) || (r.Product_Name).ToUpper().Contains(q.ToUpper()))
                )
                .OrderBy(r => r.Id);
            }
            var data = listData.Skip(fromIdx)
                .Take(toIdx)
                .Select(r => new { id = r.Id, text = r.Product_Name})
                .ToList();

            return Json(new { items = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BookList(string q, string initValue = "", int page = 0)
        {
            int fromIdx = page > 0 ? (page * 10) : 0;
            int toIdx = page > 0 ? page * 10 : 10;
            var listData = db.Books.Where(r => 1 == 2);

            if (!string.IsNullOrEmpty(initValue))
            {
                listData = db.Books
                .Where
                (
                    r =>
                    (string.IsNullOrEmpty(initValue) || (initValue == r.Id.ToString()))
                )
                .OrderBy(r => r.Id);
            }
            else
            {
                listData = db.Books
                .Where
                (
                    r =>
                    (string.IsNullOrEmpty(q) || (r.Book_Name).ToUpper().Contains(q.ToUpper()))
                )
                .OrderBy(r => r.Id);
            }
            var data = listData.Skip(fromIdx)
                .Take(toIdx)
                .Select(r => new { id = r.Id, text = r.Book_Name })
                .ToList();

            return Json(new { items = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProductData(long Id)
        {
            var product = db.Products.Where(r => r.Id == Id).Select(r => new { Product_UOM = r.Product_UOM }).FirstOrDefault();
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomerData(long Id)
        {
            var customer = db.Customers.Where(r => r.Id == Id).Select(r => new { Address = r.Address }).FirstOrDefault();
            return Json(customer, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBookData(long Id)
        {
            var book = db.Books.Where(r => r.Id == Id).Select(r => new { Authors = r.Authors }).FirstOrDefault();
            return Json(book, JsonRequestBehavior.AllowGet);
        }
    }
}