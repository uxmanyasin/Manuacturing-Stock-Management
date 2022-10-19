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
     public class ExpenseDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();

        public List<Entities.Expense> GetExpenses()
        {
            var listOfExpense = database.ExpenseDbSet.ToList();
            return listOfExpense;
        }

        public List<Entities.Expense> GetCategoryExpenses(string category)
        {
            var listOfExpense = database.ExpenseDbSet.Where(s => s.Category == category).ToList();
            return listOfExpense;
        }

        public List<Entities.Expense> GetDateExpenses(string date)
        {
            var listOfExpense = database.ExpenseDbSet.Where(s => s.Date == date).ToList();
            return listOfExpense;
        }

        public Entities.Expense GetExpense(int id)
        {
            Entities.Expense expense = database.ExpenseDbSet.Where(s => s.Id == id).FirstOrDefault();
            return expense;
        }

        
        public Entities.Expense CreateExpense(Entities.Expense expense)
        {
            database.ExpenseDbSet.Add(expense);

            database.SaveChanges();
            return expense;
        }
        public Entities.Expense EditExpense(Entities.Expense expense)
        {
            Entities.Expense model = GetExpense(expense.Id);
            model.Category = expense.Category;
            model.Amount = expense.Amount;
            model.Date = expense.Date;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }
        public void DeleteExpense(Entities.Expense expense)
        {
            database.ExpenseDbSet.Remove(expense);
            database.SaveChanges();
        }
    }
}
