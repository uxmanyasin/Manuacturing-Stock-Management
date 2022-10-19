using BizBook.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Models
{
    public class CompanyDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();
        public List<Company> GetCompanies()
        {
            var listOfCompanies = database.CompaniesDbSet.ToList();
            return listOfCompanies;
        }
        public Company GetCompany(int id)
        {
            Company comapny = database.CompaniesDbSet.Where(s => s.Id == id).FirstOrDefault();
            return comapny;
        }
        public Company GetSpecificCompany()
        {
            Company comapny = database.CompaniesDbSet.FirstOrDefault();
            return comapny;
        }
        public Company CreateCompany(Company company)
        {
            database.CompaniesDbSet.Add(company);
            database.SaveChanges();
            return company;
        }
        public Company EditCompany(Company company)
        {
            Company model = GetCompany(company.Id);
            model.Name = company.Name;
            model.OwnerName = company.OwnerName;
            model.Address = company.Address;
            model.Telephone = company.Telephone;
          
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }
        public void DeleteCompany(Company company)
        {
            database.CompaniesDbSet.Remove(company);
            database.SaveChanges();
        }
    }
}
