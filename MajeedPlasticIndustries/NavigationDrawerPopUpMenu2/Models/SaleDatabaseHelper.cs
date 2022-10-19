using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizBook.Entities;
using BizBook.Models;

namespace BizBook.Models
{
    public class SaleDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();

        public List<Entities.Sale> GetSales()
        {
            var listOfSales = database.SaleDbSet.Where(s=> s.Status == true && s.CustomerName == null) .ToList();
            return listOfSales;
        }

        public List<Entities.Sale> SaleByID(string id)
        {
            var listOfSales = database.SaleDbSet.Where(s => s.GatePass == id).ToList();
            return listOfSales;
        }
        public List<Entities.Sale_Details> GetSaleDetails(string id)
        {
            var listOfSales = database.SaleDeatilsDbSet.Where(s => s.SaleId == id).ToList();
            return listOfSales;
        }
        public List<Entities.Sale> GetwalkingSales()
        {
            var listOfSales = database.SaleDbSet.Where(s => s.Status == true && s.CustomerId == null).ToList();
            return listOfSales;
        }

        public List<Entities.Sale> GetCustomerSales(int id)
        {
            var listOfSales = database.SaleDbSet.Where(s => s.CustomerId == id && s.Status == true && s.CustomerName == null).ToList();
            return listOfSales;
        }
        public List<Entities.Sale> GetCustomerwalkingSales(string name)
        {
            var listOfSales = database.SaleDbSet.Where(s => s.CustomerName == name && s.Status == true && s.CustomerId == null).ToList();
            return listOfSales;
        }

        public List<Entities.Sale> GetDatedSales(DateTime date)
        {
            var listOfSales = database.SaleDbSet.Where(s => s.SaleDate == date && s.Status==true && s.CustomerName==null ).ToList();
            return listOfSales;
        }
        public List<Entities.Sale> GetDatedwalkingSales(DateTime date)
        {
            var listOfSales = database.SaleDbSet.Where(s => s.SaleDate == date && s.Status == true && s.CustomerId == null).ToList();
            return listOfSales;
        }

        public Entities.Sale GetSale(string id)
        {
            Entities.Sale sale = database.SaleDbSet.Where(s => s.SId == id && s.Status == true).FirstOrDefault();
            return sale;
        }

        public List<Entities.Inventory> GetInventoryList()
        {
            var listOfInventory = database.InventoryDbSet.ToList();
            return listOfInventory;
        }

        public Entities.Inventory GetInventory(int id)
        {
            Entities.Inventory inventory = database.InventoryDbSet.Where(s => s.ProductId == id ).FirstOrDefault();
            return inventory;
        }
        public Entities.Sale CreateSale(Entities.Sale sale,ObservableCollection<SaleDetailsViewModel> saleDetails )
        {
            foreach(var details in saleDetails)
            {
                database.SaleDeatilsDbSet.Add(new Entities.Sale_Details { ProductId = details.ProductId, SaleDate = sale.SaleDate ,Quantity = details.Quantity, Rate = details.Price , Total = details.Total, SaleId = sale.SId });
            }

            foreach (var data in saleDetails)
            {
              
                var inventory = GetInventory(data.ProductId);
                if (inventory != null)
                {
                    float quantity = inventory.Quantity;
                    quantity = (quantity - (data.Quantity));
                    inventory.ProductId = data.ProductId;
                    inventory.Quantity = quantity;
                    database.Entry(inventory).State = EntityState.Modified;
                }
              
            }
            Entities.Payments payments = new Payments
            {
                CustomerID = sale.CustomerId,
                Credit = 0,
                Debit = sale.TotalPrice,
                Date = sale.SaleDate,
                EntryDate = sale.EntryDate,
                SaleId = sale.SId,
                Type = "Customer",
                Description = "Sale"
            };

            database.PaymentsDbSet.Add(payments);
            database.SaleDbSet.Add(sale);
         
            database.SaveChanges();
            return sale;
        }

        public Entities.Sale EditSale(Entities.Sale sale)
        {
            Entities.Sale model = GetSale(sale.SId);
            model.TotalPrice = sale.TotalPrice;
            model.EntryDate = sale.EntryDate;
            model.SaleDate = sale.SaleDate;
            model.CustomerId = sale.CustomerId;
            model.Status = sale.Status;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Entities.Sale EditSaleAmount(Entities.Sale sale)
        {
            Entities.Sale model = GetSale(sale.SId);
            model.TotalPrice = sale.TotalPrice;
            model.TotalQuantity = sale.TotalQuantity;
            model.TotalProducts = sale.TotalProducts;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Entities.Sale EditGatePass(Entities.Sale sale)
        {
            Entities.Sale model = GetSale(sale.SId);
            model.GatePass = sale.GatePass;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Entities.Sale EditSaleDate(Entities.Sale sale)
        {
            Entities.Sale model = GetSale(sale.SId);
            model.SaleDate = sale.SaleDate;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }
        public List<Entities.Sale_Details> GetSingleSaleDetails(string sale_id)
        {
            var listOfSalesDetails = database.SaleDeatilsDbSet.Where(s => s.SaleId == sale_id).ToList();
            return listOfSalesDetails;
        }
        public void EditSaleDetailsDate(Entities.Sale sale)
        {
            var items = GetSingleSaleDetails(sale.SId);
            foreach (var item in items) 
            {
                item.SaleDate = sale.SaleDate;
                database.Entry(item).State = EntityState.Modified;
                database.SaveChanges();
               
            } 
        }

        public List<Entities.Sale> SalesBetweenDates(DateTime dateFrom,DateTime dateTo)
        {
            var model = database.SaleDbSet.Where(s => s.SaleDate >= dateFrom && s.SaleDate <= dateTo && s.CustomerName == null).ToList();
            return model;
        }
        public List<Entities.Sale> walkingSalesBetweenDates(DateTime dateFrom, DateTime dateTo)
        {
            var model = database.SaleDbSet.Where(s => s.SaleDate >= dateFrom && s.SaleDate <= dateTo && s.CustomerId == null).ToList();
            return model;
        }
        public List<Entities.Sale> GetDateBetweenCustomerSales(int id,DateTime date1 ,DateTime date2)
        {
            var listOfSales = database.SaleDbSet.Where(s => s.CustomerId == id && s.Status == true && s.CustomerName == null && s.SaleDate >= date1 && s.SaleDate <= date2).ToList();
            return listOfSales;
        }

        public List<Entities.Sale> GetDateNOTBetweenCustomerSales(int id, DateTime date1, DateTime date2)
        {
            var listOfSales = database.SaleDbSet.Where (s => s.CustomerId == id && s.Status == true && s.CustomerName == null && !(s.SaleDate >= date1 && s.SaleDate <= date2)).ToList();
            return listOfSales;
        }
        public List<Entities.Sale> walkingcustomers()
        {
            var model = database.SaleDbSet.Where(s => s.Status == true && s.CustomerId == null).ToList();
            return model;
        }

        public void DeleteSale(Entities.Sale sale)
        {
            DatabaseContext database1 = new DatabaseContext();
            var listofSaleDetails = database1.SaleDeatilsDbSet.Where(s => s.SaleId == sale.SId).ToList();
            Entities.Payments pay = database1.PaymentsDbSet.Where(s => s.SaleId == sale.SId).FirstOrDefault();
            database1.PaymentsDbSet.Remove(pay);


            foreach (var item in listofSaleDetails)
            {
                database1.SaleDeatilsDbSet.Remove(item);
            }
            var deletesale = database1.SaleDbSet.Where(s=> s.SId == sale.SId).FirstOrDefault();
            database1.SaleDbSet.Remove(deletesale);

            database1.SaveChanges();
        }
        public void DeleteWalkingSale(Entities.Sale sale)
        {
            DatabaseContext database1 = new DatabaseContext();
            var listofSaleDetails = database.SaleDeatilsDbSet.Where(s => s.SaleId == sale.SId).ToList();
            Entities.Payments pay = database.PaymentsDbSet.Where(s => s.SaleId == sale.SId).FirstOrDefault();
         
            database1.PaymentsDbSet.Remove(pay);
            foreach (var item in listofSaleDetails)
            {
                database1.SaleDeatilsDbSet.Remove(item);
            }
            database1.SaleDbSet.Remove(sale);
            database1.SaveChanges();
        }

        public List<Entities.Sale> CustomerSalesBetweenDates(int id,DateTime dateFrom, DateTime dateTo)
        {
            var model = database.SaleDbSet.Where(s =>s.CustomerId == id && s.SaleDate >= dateFrom && s.SaleDate <= dateTo && s.CustomerName == null).ToList();
            return model;
        }


    }
}
