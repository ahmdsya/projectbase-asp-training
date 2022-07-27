using ProjectBase.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectBase.ViewModels
{
    public class StationaryViewModel
    {
        public StationaryViewModel() { }

        public StationaryViewModel(ProjectBaseEntities db, long id)
        {
            Stationary model = db.Stationaries.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(Stationary), model, typeof(StationaryViewModel), this);

            var details = db.Stationary_Detail.Where(r => r.Stationary_Id == model.Id && r.Is_Delete == "N").ToList();
            Details = new List<StationaryDetailViewModel>();
            foreach (var detail in details)
                Details.Add(new StationaryDetailViewModel(detail));

            var products = db.Stationary_Product.Where(r => r.Stationary_Id == model.Id && r.Is_Delete == "N").ToList();
            Products = new List<StationaryProductViewModel>();
            foreach (var product in products)
                Products.Add(new StationaryProductViewModel(product));
        }
        public StationaryViewModel(Stationary model)
        {
            BaseProgram.CopyProperties(typeof(Stationary), model, typeof(StationaryViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }

        public void Save(ProjectBaseEntities db)
        {
            this.result = Constants.OK;
            Stationary model;
            try
            {
                if (this.mode == Constants.FORM_MODE_CREATE)
                {
                    var existedData = db.Stationaries.Where(r =>
                                            r.Transaction_No == this.Transaction_No
                                        ).ToArray();
                    if (existedData != null && existedData.Length > 0)
                    {
                        throw new Exception("Transaction Number already exists!");
                    }
                    model = new Stationary();
                    model.Create_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Create_Date = DateTime.Now;
                    model.Is_Delete = "N";
                }
                else if(this.mode == Constants.FORM_MODE_EDIT)
                {
                    model = db.Stationaries.Find(this.Id);
                    model.Edit_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Edit_Date = DateTime.Now;
                    model.Is_Delete = "N";
                }
                else
                {
                    model = db.Stationaries.Find(this.Id);
                    model.Delete_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Delete_Date = DateTime.Now;
                    model.Is_Delete = "Y";
                }
                model.Transaction_No = this.Transaction_No;
                model.Transaction_Date = this.Transaction_Date;
                var customer = db.Customers.Find(this.Customer_Id);
                model.Customer = customer;
                model.Customer_Id = this.Customer_Id;
                model.Customer_Address = this.Customer_Address;

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (this.mode == Constants.FORM_MODE_CREATE)
                            db.Entry(model).State = System.Data.Entity.EntityState.Added;
                        else if (this.mode == Constants.FORM_MODE_EDIT)
                            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        else if (this.mode == Constants.FORM_MODE_DELETE)
                            db.Entry(model).State = System.Data.Entity.EntityState.Modified;

                        if (Details != null)
                        {
                            var db2 = new ProjectBaseEntities();
                            //var filteredDetail = Details.Where(r => r.mode != Constants.FORM_MODE_UNCHANGED).ToList();
                            foreach (var Detail in Details)
                            {
                                /*if (this.mode == Constants.FORM_MODE_DELETE)
                                {
                                    Detail.mode = this.mode;

                                    Detail.Save(db, model);
                                    if (Detail.result != Constants.OK) throw new Exception(Detail.result);
                                }*/
                                if(this.mode != Constants.FORM_MODE_DELETE)
                                {
                                    if (Detail.mode != Constants.FORM_MODE_UNCHANGED)
                                    {
                                        int sameCount = this.Details.Where(r => r.Book_Id == Detail.Book_Id && r.mode != Constants.FORM_MODE_DELETE).Count();

                                        if (sameCount > 1)
                                        {
                                            throw new Exception("Multiple detail with same product is not allowed: " + Detail.Book_Id);
                                        }

                                        Detail.Save(db, model);

                                        if (Detail.result != Constants.OK) throw new Exception(Detail.result);
                                    }
                                }
                            }
                        }

                        if (Products != null)
                        {
                            var db3 = new ProjectBaseEntities();
                            //var filteredDetail = Details.Where(r => r.mode != Constants.FORM_MODE_UNCHANGED).ToList();
                            foreach (var Product in Products)
                            {
                                /*if (this.mode == Constants.FORM_MODE_DELETE)
                                {
                                    Product.mode = this.mode;

                                    Product.Save(db, model);
                                    if (Product.result != Constants.OK) throw new Exception(Product.result);
                                }*/
                                if (this.mode != Constants.FORM_MODE_DELETE)
                                {
                                    if (Product.mode != Constants.FORM_MODE_UNCHANGED)
                                    {
                                        int sameCount = this.Products.Where(r => r.Product_Id == Product.Product_Id && r.mode != Constants.FORM_MODE_DELETE).Count();

                                        if (sameCount > 1)
                                        {
                                            throw new Exception("Multiple detail with same product is not allowed: " + Product.Product_Id);
                                        }

                                        Product.Save(db, model);

                                        if (Product.result != Constants.OK) throw new Exception(Product.result);
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
        [Display(Name = "Transaction Number")]
        public string Transaction_No { get; set; }
        [Required]
        [Display(Name = "Transaction Date")]
        //[DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Transaction_Date { get; set; }
        [Required]
        [Display(Name = "Customer ID")]
        public long Customer_Id { get; set; }
        public string Customer_Name { get; set; }
        [Required]
        [Display(Name = "Customer Address")]
        public string Customer_Address { get; set; }
        public string Create_By { get; set; }
        public DateTime Create_Date { get; set; }
        public string Edit_By { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public string Is_Delete { get; set; }

        public List<StationaryDetailViewModel> Details { get; set; }
        public List<StationaryProductViewModel> Products { get; set; }
    }

    public class StationaryDetailViewModel
    {
        public StationaryDetailViewModel() { }

        public StationaryDetailViewModel(ProjectBaseEntities db, long id)
        {
            Stationary_Detail model = db.Stationary_Detail.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(Stationary_Detail), model, typeof(StationaryDetailViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }
        public StationaryDetailViewModel(Stationary_Detail model)
        {
            BaseProgram.CopyProperties(typeof(Stationary_Detail), model, typeof(StationaryDetailViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }

        public void Save(ProjectBaseEntities db, Stationary stationary = null)
        {
            this.result = Constants.OK;
            Stationary_Detail model;
            try
            {
                if (this.mode == Constants.FORM_MODE_CREATE)
                {
                    model = new Stationary_Detail();
                    model.Create_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Create_Date = DateTime.Now;
                    model.Is_Delete = "N";
                }
                else if(this.mode == Constants.FORM_MODE_EDIT)
                {
                    model = db.Stationary_Detail.Find(this.Id);
                    model.Edit_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Edit_Date = DateTime.Now;
                    model.Is_Delete = "N";
                }
                else
                {
                    model = db.Stationary_Detail.Find(this.Id);
                    model.Delete_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Delete_Date = DateTime.Now;
                    model.Is_Delete = "Y";
                }

                model.Stationary = stationary;
                model.Book_Id = this.Book_Id;
                model.Book_Authors = this.Book_Authors;
                model.Book_Qty = this.Book_Qty;

                if (this.mode == Constants.FORM_MODE_CREATE)
                    db.Entry(model).State = System.Data.Entity.EntityState.Added;
                else if (this.mode == Constants.FORM_MODE_EDIT)
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                else if (this.mode == Constants.FORM_MODE_DELETE)
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
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
        public Nullable<long> Stationary_Id { get; set; }

        [Required]
        public Nullable<long> Book_Id { get; set; }

        [Required]
        public Nullable<int> Book_Qty { get; set; }

        [Required]
        public string Book_Authors { get; set; }

        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_Date { get; set; }
        public string Edit_By { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public Nullable<System.DateTime> Delete_Date { get; set; }

    }

    public class StationaryProductViewModel
    {
        public StationaryProductViewModel() { }

        public StationaryProductViewModel(ProjectBaseEntities db, long id)
        {
            Stationary_Product model = db.Stationary_Product.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(Stationary_Product), model, typeof(StationaryProductViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }
        public StationaryProductViewModel(Stationary_Product model)
        {
            BaseProgram.CopyProperties(typeof(Stationary_Product), model, typeof(StationaryProductViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }

        public void Save(ProjectBaseEntities db, Stationary stationary = null)
        {
            this.result = Constants.OK;
            Stationary_Product model;
            try
            {
                if (this.mode == Constants.FORM_MODE_CREATE)
                {
                    model = new Stationary_Product();
                    model.Create_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Create_Date = DateTime.Now;
                    model.Is_Delete = "N";
                }
                else if (this.mode == Constants.FORM_MODE_EDIT)
                {
                    model = db.Stationary_Product.Find(this.Id);
                    model.Edit_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Edit_Date = DateTime.Now;
                    model.Is_Delete = "N";
                }
                else
                {
                    model = db.Stationary_Product.Find(this.Id);
                    model.Delete_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Delete_Date = DateTime.Now;
                    model.Is_Delete = "Y";
                }

                model.Stationary = stationary;
                model.Product_Id = this.Product_Id;
                model.Product_Qty = this.Product_Qty;
                model.Product_UOM = this.Product_UOM;

                if (this.mode == Constants.FORM_MODE_CREATE)
                    db.Entry(model).State = System.Data.Entity.EntityState.Added;
                else if (this.mode == Constants.FORM_MODE_EDIT)
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                else if (this.mode == Constants.FORM_MODE_DELETE)
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
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
        public Nullable<long> Stationary_Id { get; set; }

        [Required]
        public Nullable<long> Product_Id { get; set; }

        [Required]
        public Nullable<decimal> Product_Qty { get; set; }

        [Required]
        public string Product_UOM { get; set; }

        public Nullable<System.DateTime> Create_Date { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public string Edit_By { get; set; }
        public System.DateTime Delete_Date { get; set; }
    }
}