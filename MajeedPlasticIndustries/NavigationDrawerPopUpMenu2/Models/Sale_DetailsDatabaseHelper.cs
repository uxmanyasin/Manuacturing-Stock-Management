using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Models
{
    public class Sale_DetailsDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();

        public List<Entities.Sale_Details> GetSaleDetails()
        {
            var listOfDetailsSales = database.SaleDeatilsDbSet.ToList();
            return listOfDetailsSales;
        }

        public List<Entities.Sale_Details> GetSingleSaleDetails(string sale_id)
        {
            var listOfSalesDetails = database.SaleDeatilsDbSet.Where(s => s.SaleId == sale_id).ToList();
            return listOfSalesDetails;
        }
        public List<Entities.Sale_Details> GetProductSales(int id)
        {
            var listOfSalesDetails = database.SaleDeatilsDbSet.Where(s => s.ProductId == id).ToList();
            return listOfSalesDetails;
        }

        public List<Entities.Sale_Details> GetCustomerProductSales(int P_id ,int C_id)
        {
            var listOfSalesDetails = database.SaleDeatilsDbSet.Where(s => s.ProductId == P_id && s.sale.customers.Id == C_id).ToList();
            return listOfSalesDetails;
        }
        public List<Entities.Sale_Details> GetDatedSales(DateTime date)
        {
            var listOfSalesDetails = database.SaleDeatilsDbSet.Where(s => s.SaleDate == date).ToList();
            return listOfSalesDetails;
        }

        public List<Entities.Sale_Details> GetRangeDatedSales(DateTime dateFrom , DateTime dateTo)
        {
            var listOfSalesDetails = database.SaleDeatilsDbSet.Where(s => s.SaleDate >= dateFrom && s.SaleDate <= dateTo).ToList();
            return listOfSalesDetails;
        }

        public List<Entities.Sale_Details> GetDetailsBySaleID(string SaleID)
        {
            var listOfSalesDetails = database.SaleDeatilsDbSet.Where(s => s.SaleId == SaleID).ToList();
            return listOfSalesDetails;
        }

        public List<Entities.Sale_Details> GetProductRangeDatedSales(int id, DateTime dateFrom, DateTime dateTo)
        {
            var listOfSalesDetails = database.SaleDeatilsDbSet.Where(s => s.SaleDate >= dateFrom && s.SaleDate <= dateTo && s.ProductId == id).ToList();
            return listOfSalesDetails;
        }

        public List<Entities.Sale_Details> GetCustomerProductRangeDatedSales(int cid, int id, DateTime dateFrom, DateTime dateTo)
        {
            var listOfSalesDetails = database.SaleDeatilsDbSet.Where(s => s.sale.CustomerId == cid && s.SaleDate >= dateFrom && s.SaleDate <= dateTo && s.ProductId == id).ToList();
            return listOfSalesDetails;
        }

        public Entities.Sale_Details GetSpecificSaleDetail(int id)
        {
            Entities.Sale_Details saledetails = database.SaleDeatilsDbSet.Where(s => s.Id == id).FirstOrDefault();
            return saledetails;
        }

        public Entities.Sale_Details CreateSaleDetails(Entities.Sale_Details sale_Details)
        {
            database.SaleDeatilsDbSet.Add(sale_Details);
            database.SaveChanges();
            return sale_Details;
        }

        public List<Entities.Sale_Details> GetDatedProductSales(int id,DateTime date1 ,DateTime date2 ,int customerId)
        {
            var listOfSalesDetails = database.SaleDeatilsDbSet.Where(s => s.ProductId == id && s.sale.CustomerId == customerId && s.SaleDate >= date1 && s.SaleDate <= date2).ToList();
            return listOfSalesDetails;
        }

        

        public Entities.Sale_Details EditSaleDetails(Entities.Sale_Details sale_Details)
        {
            Entities.Sale_Details model = GetSpecificSaleDetail(sale_Details.Id);
            model.Id = sale_Details.Id;
            model.Rate = sale_Details.Rate;
            model.Quantity = sale_Details.Quantity;
            model.ProductId = sale_Details.ProductId;
            model.Total = sale_Details.Total;
            model.SaleId = sale_Details.SaleId;


            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Entities.Sale_Details UpdateRates(Entities.Sale_Details sale_Details)
        {
            Entities.Sale_Details model = GetSpecificSaleDetail(sale_Details.Id);
            model.Id = sale_Details.Id;
            model.Rate = sale_Details.Rate;
            model.Quantity = sale_Details.Quantity;
            model.ProductId = sale_Details.ProductId;
            model.Total = sale_Details.Total;
            model.SaleId = sale_Details.SaleId;


            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public void DeleteSaleDetails(Entities.Sale_Details saleDetails)
        {
            database.SaleDeatilsDbSet.Remove(saleDetails);
            database.SaveChanges();
        }
    }
}
