using ProjectBase.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using System.Web.Mvc;

namespace ProjectBase.Controllers
{
    public class SummaryModel
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
        [Display(Name = "Quantity From")]
        public long? Quantity_From { get; set; }
        [Display(Name = "Quantity To")]
        public long? Quantity_To { get; set; }
    }

    public class SummaryController : Controller
    {
        ProjectBaseEntities db = new ProjectBaseEntities();

        [CustomAuthorize]
        public ActionResult Index()
        {
            SummaryModel model = new SummaryModel();
            model.Date_From = DateTime.Now;
            model.Quantity_From = 0;

            return View(model);
        }

        [HttpPost]
        public ActionResult ShowSummary(SummaryModel model)
        {
            if (ModelState.IsValid)
            {
                //PROCESS DATA
                string query = @"SELECT 
                                    O.Id,
                                    O.Order_No,
                                    O.Order_Date,
                                    C.Code + ' - ' + C.Name AS Customer_Name,
                                    SUM(OD.Product_Qty) As Qty
                                FROM [Order] O
                                INNER JOIN [Order_Detail] OD ON OD.Order_Id = O.Id
                                INNER JOIN [Customer] C ON C.Id = O.Order_Customer_Id
                                WHERE 1=1 {0}
                                GROUP BY O.Id, O.Order_No,
                                O.Order_Date,
                                C.Code + ' - ' + C.Name
                                HAVING 1=1 {1}";
                string whereQuery = "";
                string havingQuery = "";
                Dictionary<string, object> sqlParams = new Dictionary<string, object>();

                if (model.Date_To.HasValue)
                {
                    whereQuery += " AND O.Order_Date >= @DateFrom AND O.Order_Date <= @DateTo";
                    havingQuery += "";
                    sqlParams.Add("DateTo", model.Date_To.Value);
                }
                else
                {
                    whereQuery += " AND O.Order_Date = @DateFrom";
                    havingQuery += "";
                }
                sqlParams.Add("DateFrom", model.Date_From);

                if (model.Customer_Id.HasValue && model.Customer_Id.Value != 0)
                {
                    whereQuery += " AND C.Id = @CustomerId";
                    havingQuery += "";
                    sqlParams.Add("CustomerId", model.Customer_Id.Value);
                }

                if (model.Quantity_To.HasValue)
                {
                    whereQuery += "";
                    havingQuery += " AND SUM(OD.Product_Qty) >= @QuantityFrom AND SUM(OD.Product_Qty) <= @QuantityTo";
                    sqlParams.Add("QuantityTo", model.Quantity_To.Value);
                }
                else
                {
                    whereQuery += "";
                    havingQuery += " AND SUM(OD.Product_Qty) >= @QuantityFrom";
                }
                sqlParams.Add("QuantityFrom", model.Quantity_From);

                try
                {
                    DataTable dt = new DataTable();
                    string result = SQL.GetDataTable("", string.Format(query, whereQuery, havingQuery), sqlParams, out dt);
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
                            if (prevId != dt.Rows[i]["Id"].ToString())
                            {
                                sb.Append("<td>" + dt.Rows[i]["Order_No"].ToString() + "</t d>");
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
                        sb.Append("<td>" + dt.Rows[i]["Qty"].ToString() + "</td>");

                        sb.Append("</tr>");
                    }

                    //VIEWDATA
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