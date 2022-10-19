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
    public class PurchaseDatabaseHelper
    {
        private DatabaseContext database = new DatabaseContext();

        public List<Entities.Purchase> GetPurchases()
        {
            var listOfPurchase = database.PurchaseDbSet.ToList();
            return listOfPurchase;
        }

        public List<Entities.Purchase> GetPurchaseByID(string id)
        {
            var listOfPurchase = database.PurchaseDbSet.Where(s => s.GatePass == id).ToList();
            return listOfPurchase;
        }

        public List<Entities.Purchase_Details> GetPurchaseDetails(string id)
        {
            var listOfPurchase = database.PurchaseDetailsDbSet.Where(s => s.PurchaseId == id).ToList();
            return listOfPurchase;
        }


        public List<Entities.Purchase> GetSupplierPurchase(int id)
        {
            var listOfPurchase = database.PurchaseDbSet.Where(s => s.SupplierId == id).ToList();
            return listOfPurchase;
        }

        public List<Entities.Purchase> GetDatedPurchase(DateTime date)
        {
            var listOfSales = database.PurchaseDbSet.Where(s => s.PurchaseDate == date).ToList();
            return listOfSales;
        }

        public List<Entities.Purchase> GetDateBetweenPurchase(DateTime date1,DateTime date2)
        {
           
            var listOfSales = database.PurchaseDbSet.Where(s => s.PurchaseDate >= date1 && s.PurchaseDate <= date2).ToList();
            return listOfSales;
        }
        public List<Entities.Purchase> GetDateBetweenSupplierPurchase(int id, DateTime date1, DateTime date2)
        {

            var listOfSales = database.PurchaseDbSet.Where(s => s.PurchaseDate >= date1 && s.PurchaseDate <= date2 && s.SupplierId == id).ToList();
            return listOfSales;
        }

        public List<Entities.Purchase> GetDateNOTBetweenSupplierPurchase(int id, DateTime date1, DateTime date2)
        {

            var listOfSales = database.PurchaseDbSet.Where(s => !(s.PurchaseDate >= date1 && s.PurchaseDate <= date2) && s.SupplierId == id).ToList();
            return listOfSales;
        }

        public Entities.Purchase GetPurchase(string id)
        {
            Entities.Purchase purchase = database.PurchaseDbSet.Where(s => s.PId == id).FirstOrDefault();
            return purchase;
        }

        public Payments CreatePayment(Payments payment)
        {
            database.PaymentsDbSet.Add(payment);
            database.SaveChanges();
            return payment;
        }
        public Payments GetBalance(int id)
        {
            var payment = database.PaymentsDbSet.Where(s => s.SupplierID == id).FirstOrDefault();
            return payment;
        }

        public List<Entities.Inventory> GetInventoryList()
        {
            var listOfInventory = database.InventoryDbSet.ToList();
            return listOfInventory;
        }
        public Entities.RawStock GetInventory(int id)
        {
            Entities.RawStock inventory = database.RawStockDbSet.Where(s => s.RawId == id ).FirstOrDefault();
            return inventory;
        }
        public Entities.Purchase CreatePurchase(Entities.Purchase purchase, ObservableCollection<SaleDetailsViewModel> PurchaseDetails)
        {
            foreach (var details in PurchaseDetails)
            {
                database.PurchaseDetailsDbSet.Add(new Entities.Purchase_Details { RawId = details.ProductId, Quantity = details.Quantity, Total = details.Total, Rate=details.Price, PurchaseId = purchase.PId});
            }
            
            foreach (var data in PurchaseDetails)
            {
               
                var inventory = GetInventory(data.ProductId);
                if (inventory == null)
                {
                    
                    Entities.RawStock NewInventory = new Entities.RawStock()
                    {
                        RawId = data.ProductId,
                        Quantity = data.Quantity,


                    };
                    database.RawStockDbSet.Add(NewInventory);
                }
                else
                {
                    float quantity = inventory.Quantity;
                    quantity = (quantity + (data.Quantity));
                    inventory.Quantity = quantity;
                    database.Entry(inventory).State = EntityState.Modified;

                }

            }
            Payments payment = new Payments()
            {
                
                EntryDate = DateTime.Now.Date,
                Credit = purchase.TotalPrice,
                Debit = 0,
                SupplierID = purchase.SupplierId,
                PurchaseId = purchase.PId,
                Date = purchase.PurchaseDate,
                Type = "Supplier",
                Description = "Purchase"
                
            };
          
            database.PaymentsDbSet.Add(payment);
           
            database.PurchaseDbSet.Add(purchase);
            database.SaveChanges();
            return purchase;
        }

        public Entities.Purchase EditPurchase(Entities.Purchase purchase)
        {
            Entities.Purchase model = GetPurchase(purchase.PId);
            model.TotalProducts = purchase.TotalProducts;
            model.TotalQuantity = purchase.TotalQuantity;
            model.TotalPrice = purchase.TotalPrice;
            model.EntryDate = purchase.EntryDate;
            model.PurchaseDate = purchase.PurchaseDate;
            model.SupplierId = purchase.SupplierId;
            model.Status = purchase.Status;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }
        public Entities.Purchase EditPurchaseAmounts(Entities.Purchase purchase)
        {
            Entities.Purchase model = GetPurchase(purchase.PId);
            model.Cash = purchase.Cash;
            model.PurchaseAmount = purchase.PurchaseAmount;
            model.TotalPrice = purchase.TotalPrice;
            model.Freight = purchase.Freight;
          
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Entities.Purchase EditPurchaseDate(Entities.Purchase purchase)
        {
            Entities.Purchase model = GetPurchase(purchase.PId);
            model.PurchaseDate = purchase.PurchaseDate;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }
        public Entities.Purchase EditGatePass(Entities.Purchase purchase)
        {
            Entities.Purchase model = GetPurchase(purchase.PId);
            model.GatePass = purchase.GatePass;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public List<Entities.Purchase_Details> GetSinglePurchaseDetails(string Pid)
        {
            var listOfSalesDetails = database.PurchaseDetailsDbSet.Where(s => s.PurchaseId == Pid).ToList();
            return listOfSalesDetails;
        }
        public List<Entities.Purchase> PurchaseBetweenDates(DateTime dateFrom,DateTime dateTo)
        {
            var model = database.PurchaseDbSet.Where(s => s.PurchaseDate >= dateFrom && s.PurchaseDate <= dateTo).ToList();
            return model;
        }

        public List<Entities.Purchase_Details> GetDatedRawMaterialPurchases(int id, DateTime date1, DateTime date2, int customerId)
        {
            var listOfSalesDetails = database.PurchaseDetailsDbSet.Where(s => s.RawId == id && s.purchase.SupplierId == customerId && s.purchase.PurchaseDate >= date1 && s.purchase.PurchaseDate <= date2).ToList();
            return listOfSalesDetails;
        }


        public Entities.Purchase EditPurchaseAmount(Entities.Purchase _purchase)
        {
            Entities.Purchase model = GetPurchase(_purchase.PId);
            model.TotalPrice = _purchase.TotalPrice;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Entities.Purchase_Details UpdateRates(Entities.Purchase_Details purchase_details)
        {
            Entities.Purchase_Details model = GetSpecificPurchaseDetail(purchase_details.Id);
            model.Id = purchase_details.Id;
            model.Quantity = purchase_details.Quantity;
            model.RawId = purchase_details.RawId;
            model.Total = purchase_details.Total;
            model.PurchaseId = purchase_details.PurchaseId;

            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Entities.Purchase_Details GetSpecificPurchaseDetail(int id)
        {
            Entities.Purchase_Details saledetails = database.PurchaseDetailsDbSet.Where(s => s.Id == id).FirstOrDefault();
            return saledetails;
        }
       
        //public void EditSaleDetailsDate(Entities.Purchase purchase)
        //{
        //    var items = GetSinglePurchaseDetails(purchase.PId);
        //    foreach (var item in items)
        //    {
        //        item.date = sale.SaleDate;
        //        database.Entry(item).State = EntityState.Modified;
        //        database.SaveChanges();

        //    }
        //}

        public void DeletePurchaseDetails(Entities.Purchase_Details model)
        {
            database.PurchaseDetailsDbSet.Remove(model);
            database.SaveChanges();
        }

        public Entities.Purchase_Details EditPurchaseDetail(Entities.Purchase_Details purchase_details)
        {
            Entities.Purchase_Details model = GetSpecificPurchaseDetail(purchase_details.Id);
            model.Id = purchase_details.Id;
            model.Quantity = purchase_details.Quantity;
            model.Rate = purchase_details.Rate;
            model.RawId = purchase_details.RawId;
            model.Total = purchase_details.Total;
            model.PurchaseId = purchase_details.PurchaseId;


            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public void DeletePurchase(Entities.Purchase purchase)
        {
            var listofPurchaseDetails = database.PurchaseDetailsDbSet.Where(s => s.PurchaseId == purchase.PId).ToList();
            Entities.Payments pay = database.PaymentsDbSet.Where(s => s.PurchaseId == purchase.PId).FirstOrDefault();
            database.PaymentsDbSet.Remove(pay);
            foreach (var item in listofPurchaseDetails)
            {
                database.PurchaseDetailsDbSet.Remove(item);
            }
            database.PurchaseDbSet.Remove(purchase);
            database.SaveChanges();
        }

        public List<Entities.Purchase> PurchaseSupplierBetweenDates( int id,DateTime dateFrom, DateTime dateTo)
        {
            var model = database.PurchaseDbSet.Where(s => s.SupplierId == id && s.PurchaseDate >= dateFrom && s.PurchaseDate <= dateTo).ToList();
            return model;
        }
    }
}
