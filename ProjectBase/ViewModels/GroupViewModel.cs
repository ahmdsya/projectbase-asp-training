using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectBase.ViewModels
{
    public class GroupViewModel
    {
        public GroupViewModel() { }
        public GroupViewModel(ProjectBaseEntities db, long id)
        {
            Group model = db.Groups.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(Group), model, typeof(GroupViewModel), this);

            var details = db.Group_Menu.Where(r => r.Group_Id == model.Id).ToList();
            Details = new List<Group_MenuViewModel>();
            foreach (var detail in details)
                Details.Add(new Group_MenuViewModel(detail));
        }
        public GroupViewModel(Group model)
        {
            BaseProgram.CopyProperties(typeof(Group), model, typeof(GroupViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }
        public void Save(ProjectBaseEntities db)
        {
            this.result = Constants.OK;
            Group model;
            try
            {
                if (this.mode == Constants.FORM_MODE_CREATE)
                {
                    var existedData = db.Groups.Where(r =>
                                            r.Group_Code == this.Group_Code
                                        ).ToArray();
                    if (existedData != null && existedData.Length > 0)
                    {
                        throw new Exception("Group already exists!");
                    }
                    model = new Group();
                    model.Create_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Create_Date = DateTime.Now;
                }
                else
                {
                    model = db.Groups.Find(this.Id);
                    model.Edit_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Edit_Date = DateTime.Now;
                }
                model.Group_Code = this.Group_Code;
                model.Group_Description = this.Group_Description;

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
                        if (Details != null && Details.Count > 0)
                        {
                            foreach (var Detail in Details)
                            {
                                if (this.mode == Constants.FORM_MODE_DELETE)
                                {
                                    Detail.mode = this.mode;
                                    Detail.Save(db, model);
                                }
                                else
                                {
                                    if (Detail.mode != Constants.FORM_MODE_UNCHANGED)
                                    {
                                        int sameCount = this.Details.Where(r => r.Menu_Id == Detail.Menu_Id && r.mode != Constants.FORM_MODE_DELETE).Count();

                                        if (sameCount > 1)
                                        {
                                            throw new Exception("Multiple detail with same menu is not allowed: " + Detail.Group_Id);
                                        }

                                        Detail.Save(db, model);

                                        if (Detail.result != Constants.OK) throw new Exception(Detail.result);
                                    }
                                }

                            }
                        }

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
        [Display(Name = "Code")]
        public string Group_Code { get; set; }
        [Display(Name = "Description")]
        public string Group_Description { get; set; }
        public string Create_By { get; set; }
        public DateTime? Create_Date { get; set; }
        public string Edit_By { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public List<Group_MenuViewModel> Details { get; set; }

    }

    public class Group_MenuViewModel
    {
        public Group_MenuViewModel() { }
        public Group_MenuViewModel(ProjectBaseEntities db, long id)
        {
            Group_Menu model = db.Group_Menu.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(Group_Menu), model, typeof(Group_MenuViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }
        public Group_MenuViewModel(Group_Menu model)
        {
            BaseProgram.CopyProperties(typeof(Group_Menu), model, typeof(Group_MenuViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }
        public void Save(ProjectBaseEntities db, Group group = null)
        {
            this.result = Constants.OK;
            Group_Menu model;
            try
            {
                if (this.mode == Constants.FORM_MODE_CREATE)
                {
                    model = new Group_Menu();
                    model.Create_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Create_Date = DateTime.Now;
                }
                else
                {
                    model = db.Group_Menu.Find(this.Id);
                    model.Edit_By = "[" + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER) + "] " + System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_FULLNAME);
                    model.Edit_Date = DateTime.Now;
                }

                model.Group = group;

                var menu = db.Menus.Where(r => r.Id == this.Menu_Id).SingleOrDefault();

                if (menu == null) throw new Exception("Menu not found: " + this.Menu_Id);

                model.Menu = menu;
                model.View = this.View;
                model.Create = this.Create;
                model.Print = this.Print;
                model.Edit = this.Edit;
                model.Delete = this.Delete;

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
        public long Group_Id { get; set; }
        [Required]
        public long Menu_Id { get; set; }

        public bool View { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool Create { get; set; }
        public bool Print { get; set; }

        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_Date { get; set; }
        public string Edit_By { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
    }
}