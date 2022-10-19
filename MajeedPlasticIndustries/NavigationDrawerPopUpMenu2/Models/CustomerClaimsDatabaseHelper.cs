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
    public class CustomerClaimsDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();
        public List<Entities.CustomerClaims> GetClaims()
        {
            List<Entities.CustomerClaims> claims = database.CustomerClaimsDbSet.ToList();
            return claims;
        }
        public List<Entities.CustomerClaims> ClaimByID(string id)
        {
            var claims = database.CustomerClaimsDbSet.Where(s => s.GatePass == id).ToList();
            return claims;
        }
        public List<Entities.CustomerClaimDetails> GetClaimDetails(string Id)
        {
            List<Entities.CustomerClaimDetails> claims = database.CustomerClaimDetailsDbSet.Where(s=> s.CustomerClaimId == Id).ToList();
            return claims;
        }

        public List<Entities.CustomerClaims> GetSpecificCustomerClaims(int id)
        {
            var claims = database.CustomerClaimsDbSet.Where(s => s.CustomerId == id).ToList();
            return claims;
        }
        public List<Entities.CustomerClaims> GetDatedCustomerClaims(DateTime date)
        {
            var claims = database.CustomerClaimsDbSet.Where(s => s.ClaimDate == date).ToList();
            return claims;
        }

        public Entities.CustomerClaims CreateClaim(Entities.CustomerClaims claims, ObservableCollection<SaleDetailsViewModel> ClaimDetails)
        {
            foreach (var details in ClaimDetails)
            {
                database.CustomerClaimDetailsDbSet.Add(new Entities.CustomerClaimDetails { ProductId = details.ProductId, Quantity = details.Quantity, Rate = details.Price, Total = details.Total, CustomerClaimId = claims.CId });
            }
          
            Payments payment = new Payments()
            {
                EntryDate = DateTime.Now.Date,
                Debit = 0,
                Credit = claims.TotalPrice,
                CustomerID = claims.CustomerId,
                CustomerClaimsID = claims.CId,
                Date = claims.ClaimDate,
                Type = "Customer",
                Description = "Sale Return"
            };
            database.PaymentsDbSet.Add(payment);
            database.CustomerClaimsDbSet.Add(claims);
            database.SaveChanges();
            return claims;
        }
        public List<Entities.CustomerClaims> GetBetweenDatesReturn(DateTime dateFrom, DateTime dateTo)
        {
            var model = database.CustomerClaimsDbSet.Where(s => s.ClaimDate >= dateFrom && s.ClaimDate <= dateTo && s.CustomerName == null).ToList();
            return model;
        }

        public void DeleteReturn(Entities.CustomerClaims Return)
        {
            var listofReturnDetails = database.CustomerClaimDetailsDbSet.Where(s => s.CustomerClaimId == Return.CId).ToList();
            Entities.Payments pay = database.PaymentsDbSet.Where(s => s.CustomerClaimsID == Return.CId).FirstOrDefault();
            database.PaymentsDbSet.Remove(pay);
            foreach (var item in listofReturnDetails)
            {
                database.CustomerClaimDetailsDbSet.Remove(item);
            }
            database.CustomerClaimsDbSet.Remove(Return);
            database.SaveChanges();
        }

        public void DeleteSingleDetail(Entities.CustomerClaimDetails item)
        {
            database.CustomerClaimDetailsDbSet.Remove(item);
            database.SaveChanges();
        }

        public Entities.CustomerClaimDetails GetSpecificDetail(int id)
        {
            Entities.CustomerClaimDetails customerClaimDetails = database.CustomerClaimDetailsDbSet.Where(s => s.Id == id).FirstOrDefault();
            return customerClaimDetails;
        }

        public Entities.CustomerClaimDetails EditDetails(Entities.CustomerClaimDetails customerClaimDetails)
        {
            Entities.CustomerClaimDetails model = GetSpecificDetail(customerClaimDetails.Id);
            model.Id = customerClaimDetails.Id;
            model.Rate = customerClaimDetails.Rate;
            model.Quantity = customerClaimDetails.Quantity;
            model.ProductId = customerClaimDetails.ProductId;
            model.Total = customerClaimDetails.Total;
            model.CustomerClaimId = customerClaimDetails.CustomerClaimId;


            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Entities.CustomerClaims GetSingleClaims(string id)
        {
            var claims = database.CustomerClaimsDbSet.Where(s => s.CId == id).FirstOrDefault();
            return claims;
        }

        public Entities.CustomerClaims EditReturnAmount(Entities.CustomerClaims item)
        {
            Entities.CustomerClaims model = GetSingleClaims(item.CId);
            model.TotalPrice = item.TotalPrice;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Entities.CustomerClaims EditClaimDate(Entities.CustomerClaims item)
        {
            Entities.CustomerClaims model = GetSingleClaims(item.CId);
            model.ClaimDate = item.ClaimDate;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Entities.CustomerClaims EditGatePass(Entities.CustomerClaims item)
        {
            Entities.CustomerClaims model = GetSingleClaims(item.CId);
            model.GatePass = item.GatePass;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }
    }
}
