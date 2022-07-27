using ProjectBase.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectBase.ViewModels
{
    public class CustomerViewModel
    {
        public CustomerViewModel() { }

        public CustomerViewModel(ProjectBaseEntities db, long id)
        {
            Customer model = db.Customers.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(Customer), model, typeof(CustomerViewModel), this);
        }
        public CustomerViewModel(Customer model)
        {
            BaseProgram.CopyProperties(typeof(Customer), model, typeof(CustomerViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }

        public void Save(ProjectBaseEntities db)
        {
            this.result = Constants.OK;
            Customer model;
            try
            {
                model = new Customer();
                model.Code = this.Code;
                model.Name = this.Name;
                model.Address = this.Address;
                if (this.mode == Constants.FORM_MODE_CREATE)
                {
                    var existedData = db.Customers.Where(r =>
                                            r.Code == this.Code
                                        ).ToArray();
                    if (existedData != null && existedData.Length > 0)
                    {
                        throw new Exception("Customer Code already exists!");
                    }
                    model.Created_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Created_Date = DateTime.Now;
                }
                else
                {
                    model = db.Customers.Find(this.Id);
                    model.Edited_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Edited_Date = DateTime.Now;
                }
                model.Is_Deleted = this.Is_Deleted;

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

        [Required]
        [Display(Name = "Customer ID")]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Is_Deleted")]
        public bool Is_Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Edited_By { get; set; }
        public Nullable<System.DateTime> Edited_Date { get; set; }

    }

}