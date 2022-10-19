using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizBook.Models;
using BizBook.Entities;
namespace BizBook.Models
{
    public class RawStockDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();
        public List<Entities.RawStock> GetRawStocks()
        {
            var listOfRawStock = database.RawStockDbSet.ToList();
            return listOfRawStock;
        }
        public Entities.RawStock GetSingleRawStock(int id)
        {
            Entities.RawStock rawStock = database.RawStockDbSet.Where(s => s.RawId == id).FirstOrDefault();
            return rawStock;
        }

        public Entities.RawStock GetSpecificRawStock(int id)
        {
            Entities.RawStock rawStock = database.RawStockDbSet.Where(s => s.RawId == id).FirstOrDefault();
            return rawStock;
        }

        public Entities.RawStock CreateRawStock(Entities.RawStock rawStock)
        {
            database.RawStockDbSet.Add(rawStock);
            database.SaveChanges();
            return rawStock;
        }
        public Entities.RawStock RawStock_In(Entities.RawStock rawStock)
        {
            var model = GetSingleRawStock(rawStock.RawId);
 
            model.Quantity = rawStock.Quantity;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Entities.RawStock RawStock_Out(Entities.RawStock rawStock)
        {
            var model = GetSingleRawStock(rawStock.Id);

          
            model.Quantity = rawStock.Quantity;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }





        public void DeleteCompany(Entities.RawStock rawStock)
        {
            database.RawStockDbSet.Remove(rawStock);
            database.SaveChanges();
        }
    }
}
