using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizBook.Entities;
using BizBook.Models;
using System.Data.Entity;

namespace BizBook.Models
{
     public class ProductDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();

        public List<Entities.Product> GetProducts()
        {
            var listOfProducts = database.ProductDbSet.ToList();
            return listOfProducts;
        }
        public List<Entities.Product> GetCategoryProducts(int id)
        {
            var listOfProducts = database.ProductDbSet.Where(s=> s.CategoryId == id) .ToList();
            return listOfProducts;
        }
        public Entities.Product GetProduct(int id)
        {
            Entities.Product product = database.ProductDbSet.Where(s => s.Id == id).FirstOrDefault();
            return product;
        }
        public Entities.Product CreateProduct(Entities.Product product)
        {
            database.ProductDbSet.Add(product);
            database.SaveChanges();
            return product;
        }
        public Entities.Product EditProduct(Entities.Product product)
        {
            Entities.Product model = GetProduct(product.Id);
            model.Name = product.Name;
            model.Rate = product.Rate;
            model.CategoryId = product.CategoryId;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

    }
}
