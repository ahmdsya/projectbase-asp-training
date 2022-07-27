using ProjectBase.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectBase.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel() { }

        public ProductViewModel(ProjectBaseEntities db, long id)
        {
            Product model = db.Products.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(Product), model, typeof(ProductViewModel), this);
        }
        public ProductViewModel(Product model)
        {
            BaseProgram.CopyProperties(typeof(Product), model, typeof(ProductViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }

        public void Save(ProjectBaseEntities db)
        {
            this.result = Constants.OK;
            Product model;
            try
            {
                if (this.mode == Constants.FORM_MODE_CREATE)
                {
                    var existedData = db.Products.Where(r =>
                                            r.Id == this.Id
                                        ).ToArray();
                    if (existedData != null && existedData.Length > 0)
                    {
                        throw new Exception("Product ID already exists!");
                    }
                    model = new Product();
                }
                else
                {
                    model = db.Products.Find(this.Id);
                }
                model.Product_Name = this.Product_Name;
                model.Product_Qty = this.Product_Qty;
                model.Product_UOM = this.Product_UOM;

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
        public long Id { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string Product_Name { get; set; }
        [Required]
        [Display(Name = "Product Quantity")]
        public Decimal Product_Qty { get; set; }
        [Required]
        [Display(Name = "Product UOM")]
        public string Product_UOM { get; set; }
    }
}