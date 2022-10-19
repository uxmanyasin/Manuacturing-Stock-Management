using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizBook.Entities;
using BizBook.Models;

namespace BizBook.Models
{
    public class InventoryDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();

        public List<Entities.Inventory> GetInventoryList()
        {
            var listOfInventory = database.InventoryDbSet.ToList();
            return listOfInventory;
        }
        public Entities.Inventory GetSpecificInventory(int id)
        {
            var listOfInventory = database.InventoryDbSet.Where(s => s.ProductId == id).FirstOrDefault();
            return listOfInventory;
        }

        public List<Entities.Inventory> GetSpecificInventoryList(int id)
        {
            var listOfInventory = database.InventoryDbSet.Where(s => s.ProductId == id).ToList();
            return listOfInventory;
        }
        //==================================================================
        public List<Entities.Inventory> GetShopInventoryList()
        {
            var listOfInventory = database.InventoryDbSet.ToList();
            return listOfInventory;
        }
        public Entities.Inventory GetShopSpecificInventoryList(int id)
        {
            Entities.Inventory inventory = database.InventoryDbSet.Where(s => s.ProductId == id ).FirstOrDefault();
            return inventory;
        }
        //=====================================================================
        public List<Entities.Inventory> GetFactoryInventoryList()
        {
            var listOfInventory = database.InventoryDbSet.ToList();
            return listOfInventory;
        }
        public Entities.Inventory GetFactorySpecificInventoryList(int id)
        {
            Entities.Inventory inventory = database.InventoryDbSet.Where(s => s.ProductId == id ).FirstOrDefault();
            return inventory;
        }
        //========================================================================
        public Entities.Inventory ProductStock_In(Entities.Inventory inventory)
        {
            Entities.Inventory model = GetSpecificInventory(inventory.ProductId);

            model.Quantity = inventory.Quantity;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Entities.Inventory CreateInventory(Entities.Inventory inventory)
        {
            database.InventoryDbSet.Add(inventory);
            database.SaveChanges();
            return inventory;
        }
    }
}
