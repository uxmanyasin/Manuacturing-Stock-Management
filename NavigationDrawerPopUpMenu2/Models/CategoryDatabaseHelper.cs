using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizBook.Entities;

namespace BizBook.Models
{
    public class CategoryDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();
        public List<Entities.Category> GetCategories()
        {
            var listOfCategory = database.CategoryDbSet.ToList();
            return listOfCategory;
        }
        public Entities.Category GetSingleCategory(int id)
        {
            Entities.Category category = database.CategoryDbSet.Where(s => s.Id == id).FirstOrDefault();
            return category;
        }

        public Entities.Category CreateRawMaterial(Entities.Category category)
        {
            database.CategoryDbSet.Add(category);
            database.SaveChanges();
            return category;
        }
        public Entities.Category EditCategory(Entities.Category category)
        {
            Entities.Category model = GetSingleCategory(category.Id);
            model.Name = category.Name;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }
    }
}
