﻿using ProjectBase.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectBase.ViewModels
{
    public class OrderViewModel
    {
        public OrderViewModel() { }

        public OrderViewModel(ProjectBaseEntities db, long id)
        {
            Order model = db.Orders.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(Order), model, typeof(OrderViewModel), this);

            var details = db.Order_Detail.Where(r => r.Order_Id == model.Id).ToList();
            Details = new List<OrderDetailViewModel>();
            foreach (var detail in details)
                Details.Add(new OrderDetailViewModel(detail));
        }
        public OrderViewModel(Order model)
        {
            BaseProgram.CopyProperties(typeof(Order), model, typeof(OrderViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }

        public void Save(ProjectBaseEntities db)
        {
            this.result = Constants.OK;
            Order model;
            try
            {
                if (this.mode == Constants.FORM_MODE_CREATE)
                {
                    var existedData = db.Orders.Where(r =>
                                            r.Order_No == this.Order_No
                                        ).ToArray();
                    if (existedData != null && existedData.Length > 0)
                    {
                        throw new Exception("Order Number already exists!");
                    }
                    model = new Order();
                    model.Create_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Create_Date = DateTime.Now;
                }
                else
                {
                    model = db.Orders.Find(this.Id);
                    model.Edit_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Edit_Date = DateTime.Now;
                }
                model.Order_No = this.Order_No;
                model.Order_Date = this.Order_Date;
                var customer = db.Customers.Find(this.Order_Customer_Id);
                model.Customer = customer;
                model.Order_Customer_Id = this.Order_Customer_Id;
                model.Order_Customer_Address = this.Order_Customer_Address;

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (this.mode == Constants.FORM_MODE_CREATE)
                            db.Entry(model).State = System.Data.Entity.EntityState.Added;
                        else if (this.mode == Constants.FORM_MODE_EDIT)
                            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        else if (this.mode == Constants.FORM_MODE_DELETE)
                            db.Entry(model).State = System.Data.Entity.EntityState.Deleted;

                        if (Details != null)
                        {
                            var db2 = new ProjectBaseEntities();
                            var filteredDetail = Details.Where(r => r.mode != Constants.FORM_MODE_UNCHANGED).ToList();
                            foreach (var Detail in Details)
                            {
                                if (this.mode == Constants.FORM_MODE_DELETE)
                                {
                                    Detail.mode = this.mode;

                                    Detail.Save(db, model);
                                    if (Detail.result != Constants.OK) throw new Exception(Detail.result);
                                }
                                else
                                {
                                    if (Detail.mode != Constants.FORM_MODE_UNCHANGED)
                                    {
                                        int sameCount = this.Details.Where(r => r.Product_Name == Detail.Product_Name && r.mode != Constants.FORM_MODE_DELETE).Count();

                                        if (sameCount > 1)
                                        {
                                            throw new Exception("Multiple detail with same product is not allowed: " + Detail.Product_Name);
                                        }

                                        Detail.Save(db, model);

                                        if (Detail.result != Constants.OK) throw new Exception(Detail.result);
                                    }
                                }
                            }
                        }
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception exc_)
                    {
                        result = "Failed to save data.<br/>Error Message : " + exc_.Message.Replace(Environment.NewLine, " ").Replace("'", "");
                        transaction.Rollback();
                    }
                }
            }
            catch (Exception exc)
            {
                result = exc.Message.Replace(Environment.NewLine, " ").Replace("'", "");
            }
        }

        public string result { get; set; }
        public string mode { get; set; }
        public long Id { get; set; }
        [Required]
        [Display(Name = "Order Number")]
        public string Order_No { get; set; }
        [Required]
        [Display(Name = "Order Date")]
        public DateTime Order_Date { get; set; }
        [Required]
        [Display(Name = "Customer ID")]
        public long Order_Customer_Id { get; set; }
        public string Customer_Name { get; set; }
        [Required]
        [Display(Name = "Customer Address")]
        public string Order_Customer_Address { get; set; }
        public string Create_By { get; set; }
        public DateTime Create_Date { get; set; }
        public string Edit_By { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }

        public List<OrderDetailViewModel> Details { get; set; }
    }

    public class OrderDetailViewModel
    {
        public OrderDetailViewModel() { }

        public OrderDetailViewModel(ProjectBaseEntities db, long id)
        {
            Order_Detail model = db.Order_Detail.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(Order_Detail), model, typeof(OrderDetailViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }
        public OrderDetailViewModel(Order_Detail model)
        {
            BaseProgram.CopyProperties(typeof(Order_Detail), model, typeof(OrderDetailViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }
        
        public void Save(ProjectBaseEntities db, Order order = null)
        {
            this.result = Constants.OK;
            Order_Detail model;
            try
            {
                if (this.mode == Constants.FORM_MODE_CREATE)
                {
                    model = new Order_Detail();
                    model.Create_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Create_Date = DateTime.Now;
                }
                else
                {
                    model = db.Order_Detail.Find(this.Id);
                    model.Edit_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Edit_Date = DateTime.Now;
                }

                //var menu = db.Menus.Where(r => r.Id == this.Order_Id).SingleOrDefault();

                //if (menu == null) throw new Exception("Order ID not found: " + this.Order_Id);

                model.Order = order;
                model.Product_Name = this.Product_Name;
                model.Product_UOM = this.Product_UOM;
                model.Product_Qty = this.Product_Qty;

                if (this.mode == Constants.FORM_MODE_CREATE)
                    db.Entry(model).State = System.Data.Entity.EntityState.Added;
                else if (this.mode == Constants.FORM_MODE_EDIT)
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                else if (this.mode == Constants.FORM_MODE_DELETE)
                    db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception exc)
            {
                result = exc.Message.Replace(Environment.NewLine, " ").Replace("'", "");
            }
        }

        public string result { get; set; }
        public string mode { get; set; }
        public long Id { get; set; }
        [Required]
        public long Order_Id { get; set; }

        [Required]
        public string Product_Name { get; set; }

        [Required]
        public Decimal Product_Qty { get; set; }

        [Required]
        public string Product_UOM { get; set; }

        public string Create_By { get; set; }
        public DateTime Create_Date { get; set; }
        public string Edit_By { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }

    }
}