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
    public class SupplierClaimsDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();
        public List<Entities.SupplierClaims> GetClaims()
        {
            List<Entities.SupplierClaims> claims = database.SupplierClaimsDbSet.ToList();
            return claims;
        }
        public List<Entities.SupplierClaims> ClaimsByID(string id)
        {
            var claims = database.SupplierClaimsDbSet.Where(s => s.GatePass == id).ToList();
            return claims;
        }
        public List<Entities.SupplierClaimDetails> GetClaimDetails(string Id)
        {
            List<Entities.SupplierClaimDetails> claims = database.SupplierClaimDetailsDbSet.Where(s => s.SupplierClaimId == Id).ToList();
            return claims;
        }
        public List<Entities.SupplierClaims> GetSpecificSupplierClaims(int id)
        {
            var claims = database.SupplierClaimsDbSet.Where(s => s.SupplierId == id).ToList();
            return claims;
        }

        public Entities.SupplierClaims GetSingleClaims(string id)
        {
            var claims = database.SupplierClaimsDbSet.Where(s => s.CId == id).FirstOrDefault();
            return claims;
        }
        public List<Entities.SupplierClaims> GetDatedSupplierClaims(DateTime date)
        {
            var claims = database.SupplierClaimsDbSet.Where(s => s.ClaimDate == date).ToList();
            return claims;
        }

        public Entities.SupplierClaims CreateClaim(Entities.SupplierClaims claims, ObservableCollection<SaleDetailsViewModel> ClaimDetails)
        {
            foreach (var details in ClaimDetails)
            {
                database.SupplierClaimDetailsDbSet.Add(new Entities.SupplierClaimDetails { RawId = details.ProductId, Quantity = details.Quantity, Rate = details.Price, Total = details.Total, SupplierClaimId = claims.CId });
            }

            Payments payment = new Payments()
            {
                EntryDate = DateTime.Now.Date,
                Debit = claims.TotalPrice,
                Credit = 0,
                SupplierID = claims.SupplierId,
                SupplierClaimsID = claims.CId,
                Date = claims.ClaimDate,
                Type = "Supplier",
                Description = "Purchase Return"
            };
            database.PaymentsDbSet.Add(payment);
            database.SupplierClaimsDbSet.Add(claims);
            database.SaveChanges();
            return claims;
        }
        public List<Entities.SupplierClaims> GetBetweenDatesReturn(DateTime dateFrom, DateTime dateTo)
        {
            var model = database.SupplierClaimsDbSet.Where(s => s.ClaimDate >= dateFrom && s.ClaimDate <= dateTo).ToList();
            return model;
        }

        public void DeleteReturn(Entities.SupplierClaims Return)
        {
            var listofReturnDetails = database.SupplierClaimDetailsDbSet.Where(s => s.SupplierClaimId == Return.CId).ToList();
            Entities.Payments pay = database.PaymentsDbSet.Where(s => s.SupplierClaimsID == Return.CId).FirstOrDefault();
            database.PaymentsDbSet.Remove(pay);
            foreach (var item in listofReturnDetails)
            {
                database.SupplierClaimDetailsDbSet.Remove(item);
            }
            database.SupplierClaimsDbSet.Remove(Return);
            database.SaveChanges();
        }

        public void DeleteSingleDetail(Entities.SupplierClaimDetails item)
        {
            database.SupplierClaimDetailsDbSet.Remove(item);
            database.SaveChanges();
        }

        public Entities.SupplierClaimDetails GetSpecificDetail(int id)
        {
            Entities.SupplierClaimDetails item = database.SupplierClaimDetailsDbSet.Where(s => s.Id == id).FirstOrDefault();
            return item;
        }

        public Entities.SupplierClaimDetails EditDetails(Entities.SupplierClaimDetails item)
        {
            Entities.SupplierClaimDetails model = GetSpecificDetail(item.Id);
            model.Id = item.Id;
            model.Rate = item.Rate;
            model.Quantity = item.Quantity;
            model.RawId = item.RawId;
            model.Total = item.Total;
            model.SupplierClaimId = item.SupplierClaimId;


            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Entities.SupplierClaims EditReturnAmount(Entities.SupplierClaims item)
        {
            Entities.SupplierClaims model = GetSingleClaims(item.CId);
            model.TotalPrice = item.TotalPrice;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Entities.SupplierClaims EditClaimDate(Entities.SupplierClaims item)
        {
            Entities.SupplierClaims model = GetSingleClaims(item.CId);
            model.ClaimDate = item.ClaimDate;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Entities.SupplierClaims EditGatePass(Entities.SupplierClaims item)
        {
            Entities.SupplierClaims model = GetSingleClaims(item.CId);
            model.GatePass = item.GatePass;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

    }
}
