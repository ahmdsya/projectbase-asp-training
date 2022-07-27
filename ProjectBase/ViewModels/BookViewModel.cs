using ProjectBase.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectBase.ViewModels
{
    public class BookViewModel
    {
        public BookViewModel() { }

        public BookViewModel(ProjectBaseEntities db, long id)
        {
            Book model = db.Books.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(Book), model, typeof(BookViewModel), this);
        }
        public BookViewModel(Book model)
        {
            BaseProgram.CopyProperties(typeof(Book), model, typeof(BookViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }

        public void Save(ProjectBaseEntities db)
        {
            this.result = Constants.OK;
            Book model;
            try
            {
                if (this.mode == Constants.FORM_MODE_CREATE)
                {
                    var existedData = db.Books.Where(r =>
                                            r.Book_Name == this.Book_Name
                                        ).ToArray();
                    if (existedData != null && existedData.Length > 0)
                    {
                        throw new Exception("Book Name already exists!");
                    }
                    model = new Book();
                }
                else
                {
                    model = db.Books.Find(this.Id);
                    
                    if(model.Book_Name != this.Book_Name)
                    {
                        var existedData = db.Books.Where(r =>
                                            r.Book_Name == this.Book_Name
                                        ).ToArray();
                        if (existedData != null && existedData.Length > 0)
                        {
                            throw new Exception("Book Name already exists!");
                        }
                    }
                }

                model.Book_Name = this.Book_Name;
                model.Authors = this.Authors;
                model.Price = this.Price;
                model.Quantity = this.Quantity;

                if (this.mode == Constants.FORM_MODE_CREATE || this.mode == Constants.FORM_MODE_EDIT)
                {
                    model.Is_Deleted = "N";
                }
                else
                {
                    model.Is_Deleted = "Y";
                }    

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

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception exc_)
                    {
                        result = "Failed to save data. <br/> Error Message : " + exc_.Message.Replace(Environment.NewLine, " ").Replace("'", "");
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
        [Display(Name = "Book Name")]
        public string Book_Name { get; set; }
        [Required]
        [Display(Name = "Authors")]
        public string Authors { get; set; }
        [Required]
        [Display(Name = "Price")]
        public long Price { get; set; }
        [Required]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        
        public string Is_Deleted { get; set; }

    }
}