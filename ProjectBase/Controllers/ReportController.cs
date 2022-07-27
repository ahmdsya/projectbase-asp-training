using ProjectBase.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using System.Web.Mvc;

namespace ProjectBase.Controllers
{
    public class FilterModel
    {
        public string result { get; set; }
        public string mode { get; set; }
        public long Id { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] //Change display as dd/MM/yyyy in view
        [Display(Name = "Date From")]
        public DateTime Date_From { get; set; }
        [Display(Name = "Date To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] //Change display as dd/MM/yyyy in view
        public DateTime? Date_To { get; set; }
        [Display(Name = "Customer")]
        public long? Customer_Id { get; set; }
        [Display(Name = "Product")]
        public long? Product_Id { get; set; }
    }

    public class ReportController : Controller
    {
        ProjectBaseEntities db = new ProjectBaseEntities();

        [CustomAuthorize]
        public ActionResult Index()
        {
            FilterModel model = new FilterModel();
            model.Date_From = DateTime.Now;

            return View(model);
        }

        [HttpPost]
        public ActionResult ShowReport(FilterModel model)
        {
            if (ModelState.IsValid)
            {
                //PROCESS DATA
                string query = @"SELECT 
                                O.Id,
                                O.Order_No,
		                        O.Order_Date,
		                        C.Code + ' - ' + C.Name AS Customer_Name,
                                P.Product_Name,
		                        OD.Product_Qty,
		                        OD.Product_UOM
                            FROM [Order] O
                            INNER JOIN [Order_Detail] OD ON OD.Order_Id = O.Id
                            INNER JOIN [Customer] C ON C.Id = O.Order_Customer_Id
                            INNER JOIN [Product] P ON P.Id = OD.Product_Name
                            WHERE 1=1 {0}";
                string whereQuery = "";
                Dictionary<string, object> sqlParams = new Dictionary<string, object>();

                if (model.Date_To.HasValue)
                {
                    whereQuery += " AND O.Order_Date >= @DateFrom AND O.Order_Date <= @DateTo";
                    sqlParams.Add("DateTo", model.Date_To.Value);
                }
                else
                {
                    whereQuery += " AND O.Order_Date = @DateFrom";
                }
                sqlParams.Add("DateFrom", model.Date_From);

                if (model.Customer_Id.HasValue && model.Customer_Id.Value != 0)
                {
                    whereQuery += " AND C.Id = @CustomerId";
                    sqlParams.Add("CustomerId", model.Customer_Id.Value);
                }

                if (model.Product_Id.HasValue && model.Product_Id.Value != 0)
                {
                    whereQuery += " AND P.Id = @ProductId";
                    sqlParams.Add("ProductId", model.Product_Id.Value);
                }

                try
                {
                    DataTable dt = new DataTable();
                    string result = SQL.GetDataTable("", string.Format(query, whereQuery), sqlParams, out dt);
                    //SQL.GetDataTable to get queried data from database, hover to variable dt and click on the microscope to check the data retrieved

                    if (result != "OK")
                    {
                        //ERROR
                    }

                    if (dt.Rows.Count <= 0)
                    {
                        //ERROR
                    }

                    List<string> skipColumns = new List<string>()
                    {
                        "Order_No", "Order_Date", "Order_Customer_Name"
                    };

                    //GO AHEAD TO RENDER TABLE BODY
                    StringBuilder sb = new StringBuilder();
                    string prevId = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("<tr>");

                        if (string.IsNullOrEmpty(prevId))
                        {
                            prevId = dt.Rows[i]["Id"].ToString();

                            sb.Append("<td>" + dt.Rows[i]["Order_No"].ToString() + "</td>");
                            //sb.Append("<td>" + ((DateTime)dt.Rows[i]["Order_Date"]).ToString("dd/MM/yyyy") + "</td>");

                            DateTime dateTime = DateTime.Parse(dt.Rows[i]["Order_Date"].ToString());
                            sb.Append("<td>" + dateTime.ToString("dd/MM/yyyy") + "</td>");
                            sb.Append("<td>" + dt.Rows[i]["Customer_Name"].ToString() + "</td>");
                        }
                        else
                        {

                            if(prevId != dt.Rows[i]["Id"].ToString())
                            {
                                sb.Append("<td>" + dt.Rows[i]["Order_No"].ToString() + "</td>");
                                //sb.Append("<td>" + ((DateTime)dt.Rows[i]["Order_Date"]).ToString("dd/MM/yyyy") + "</td>");

                                DateTime dateTime = DateTime.Parse(dt.Rows[i]["Order_Date"].ToString());
                                sb.Append("<td>" + dateTime.ToString("dd/MM/yyyy") + "</td>");
                                sb.Append("<td>" + dt.Rows[i]["Customer_Name"].ToString() + "</td>");
                            }
                            else
                            {
                                sb.Append("<td>&nbsp;</td>");
                                //sb.Append("<td>" + ((DateTime)dt.Rows[i]["Order_Date"]).ToString("dd/MM/yyyy") + "</td>");

                                DateTime dateTime = DateTime.Parse(dt.Rows[i]["Order_Date"].ToString());
                                sb.Append("<td>&nbsp;</td>");
                                sb.Append("<td>&nbsp;</td>");
                            }
                            prevId = dt.Rows[i]["Id"].ToString();
                        }

                        sb.Append("<td>" + dt.Rows[i]["Product_Name"].ToString() + "</td>");
                        sb.Append("<td>" + dt.Rows[i]["Product_Qty"].ToString() + "</td>");
                        sb.Append("<td>" + dt.Rows[i]["Product_UOM"].ToString() + "</td>");

                        sb.Append("</tr>");
                    }

                    ////VIEWDATA
                    ViewData["DATA"] = sb.ToString();

                    return View();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }

        }
    }
}