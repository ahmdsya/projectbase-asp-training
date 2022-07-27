using ProjectBase.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectBase.Controllers
{
    public class BookController : Controller
    {
        ProjectBaseEntities db = new ProjectBaseEntities();

        [CustomAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadData(string draw = null, string start = null, string length = null, OrderClass[] order = null, ColumnClass[] columns = null, string[] filter = null, DateTime? DateFrom = null)
        {
            try
            {
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int filteredTotal = 0, recordsTotal = 0;
                string orderBy = string.Join(",", order.Select(r => ("A." + columns[r.column].data + " " + r.dir)));
                orderBy = orderBy.Replace("A.Book_Name", "A.Book_Name");
                string query = @"
                   SELECT A.*
                    FROM [Book] A 
                    WHERE 1=1 {0}
                    ORDER BY {3} {2} {1}
                ", whereQuery = "",
                queryCount = @"SELECT COUNT(A.Id) AS Count_ FROM [Book] A WHERE 1=1 {0}";

                List<SqlParameter> parameters = new List<SqlParameter>();
                if (filter != null && filter.Length > 0)
                {
                    for (int i = 0; i < filter.Length; i++)
                    {
                        if (string.IsNullOrEmpty(filter[i]))
                            continue;
                        string columnName = columns[i].data;
                        string tableAlias = "A.";

                        whereQuery += " AND " + tableAlias + columnName + " LIKE @" + columnName;
                        parameters.Add(new SqlParameter("@" + columnName, "%" + filter[i] + "%"));
                    }
                }

                List<SqlParameter> parameterc = new List<SqlParameter>();
                foreach (SqlParameter prm in parameters)
                    parameterc.Add(new SqlParameter(prm.ParameterName, prm.Value));

                var fullQuery = string.Format(query,
                                    whereQuery,
                                    (pageSize > 0 ? "FETCH NEXT " + pageSize.ToString() + " ROWS ONLY" : ""),
                                    (skip > -1 ? "OFFSET " + skip.ToString() + " ROWS" : ""),
                                    (string.IsNullOrEmpty(orderBy) ? "Created_Date DESC" : orderBy)
                                );

                List<BookViewModel> listOfData_ =
                    db.Database.SqlQuery<BookViewModel>(fullQuery, parameters.ToArray())
                    .ToList();

                var qc = db.Database.SqlQuery<Int32>(string.Format(queryCount, whereQuery), parameterc.ToArray()).ToArray();
                filteredTotal = qc.Length > 0 ? qc[0] : 0;

                var qt = db.Database.SqlQuery<Int32>(string.Format(queryCount, "")).ToArray();
                recordsTotal = qt.Length > 0 ? qt[0] : 0;

                return Json(new { draw = draw, recordsFiltered = filteredTotal, recordsTotal = recordsTotal, data = listOfData_ });
            }
            catch (Exception exc)
            {
                var a = exc.Message;
                throw;
            }
        }

        [CustomAuthorize]
        public ActionResult Editor(string mode, long? Id)
        {
            BookViewModel viewModel = new BookViewModel();
            if (mode != Constants.FORM_MODE_CREATE && Id != null)
                viewModel = new BookViewModel(db, Id.Value);
            else if (mode != Constants.FORM_MODE_CREATE && Id == null)
                return RedirectToAction("Index");
            viewModel.mode = mode;
            return View("Editor", viewModel);
        }

        [HttpPost]
        public ActionResult Editor(BookViewModel viewModel)
        {
            var a = ModelState.AsEnumerable().Where(r => r.Value.Errors.Count > 0).ToList();
            if (ModelState.IsValid)
            {
                viewModel.Save(db);
                if (viewModel.result == Constants.OK)
                    return RedirectToAction("Index");
                else
                {
                    ModelState.AddModelError(Constants._ERROR, viewModel.result);
                }
            }
            else
            {
                ModelState.AddModelError(Constants._ERROR, string.Join(",", (ModelState.Values.Where(r => r.Errors.Count > 0).SelectMany(r => r.Errors).Select(r => r.ErrorMessage).ToArray())) + " <br/> ");
            }
            return View("Editor", viewModel);

        }
    }
}