using BizBook.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Company> CompaniesDbSet { get; set; }
        public DbSet<Users> UsersDbSet { get; set; }
        public DbSet<Product> ProductDbSet { get; set; }
        public DbSet<Suppliers> SupplierDbSet { get; set; }
        public DbSet<Customers> CustomerDbSet { get; set; }
        public DbSet<Entities.Expense> ExpenseDbSet { get; set; }
        public DbSet<Entities.Inventory> InventoryDbSet { get; set; }
        public DbSet<Entities.Sale> SaleDbSet { get; set; }
        public DbSet<Entities.Purchase> PurchaseDbSet { get; set; }
        public DbSet<Entities.Purchase_Details> PurchaseDetailsDbSet { get; set; }
        public DbSet<Entities.Sale_Details> SaleDeatilsDbSet { get; set; }
        public DbSet<Entities.Payments> PaymentsDbSet { get; set;}
      
        public DbSet<Entities.RawMaterial> RawMaterialDbSet { get; set; }
        public DbSet<Entities.Category> CategoryDbSet { get; set; }
        public DbSet<Entities.DueAmount> DueAmountDbSet { get; set; }

        public DbSet<Entities.RawStock> RawStockDbSet { get; set; }
        public DbSet<Entities.BankCheque> BankChequeDbSet { get; set; }

        public DbSet<Entities.CustomerRate> CustomerRateDbSet { get; set; }
        public DbSet<Entities.SupplierRate> SupplierRateDbSet { get; set; }

        public DbSet<Entities.CustomerClaims> CustomerClaimsDbSet { get; set; }
        public DbSet<Entities.CustomerClaimDetails> CustomerClaimDetailsDbSet { get; set; }

        public DbSet<Entities.SupplierClaims> SupplierClaimsDbSet { get; set; }
        public DbSet<Entities.SupplierClaimDetails> SupplierClaimDetailsDbSet { get; set; }


        public DatabaseContext() : base("BizBook")
        {

        }

    }
}
