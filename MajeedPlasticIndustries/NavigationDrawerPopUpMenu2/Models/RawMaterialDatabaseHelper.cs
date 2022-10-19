using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizBook.Entities;

namespace BizBook.Models
{
    public class RawMaterialDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();
        public List<Entities.RawMaterial> GetRawMaterial()
        {
            var listOfRawMaterial = database.RawMaterialDbSet.ToList();
            return listOfRawMaterial;
        }
        public Entities.RawMaterial GetSingleRawMaterial(int id)
        {
            Entities.RawMaterial rawMaterial = database.RawMaterialDbSet.Where(s => s.Id == id).FirstOrDefault();
            return rawMaterial;
        }
       
        public Entities.RawMaterial CreateRawMaterial(Entities.RawMaterial rawMaterial)
        {
            database.RawMaterialDbSet.Add(rawMaterial);
            database.SaveChanges();
            return rawMaterial;
        }
        public Entities.RawMaterial EditRawMaterial(Entities.RawMaterial rawMaterial)
        {
            Entities.RawMaterial model = GetSingleRawMaterial(rawMaterial.Id);
            model.Name = rawMaterial.Name;
            model.Rate = rawMaterial.Rate;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }
    }
}
