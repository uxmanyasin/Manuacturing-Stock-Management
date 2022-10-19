using CsvHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
namespace BizBook.Models
{
    public class CSVHelper
    {
        public string Header;

        public void CreateSupplierLedgerCSV(string Title, ObservableCollection<Models.SupplierLedgerModel> _ModelList)
        {
            try
            {
                string date = DateTime.Now.Date.ToString("  dd-MMM-yyyy");
                string time = DateTime.Now.ToShortTimeString();
                Header = Title + date;

                using (var writer = new StreamWriter("E:\\Supplier Ledgers\\" + Header + ".csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(_ModelList);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void CreateCustomerLedgerCSV(string Title, ObservableCollection<Models.CustomerLedgerModel> _ModelList)
        {
            try
            {
                string date = DateTime.Now.Date.ToString("  dd-MMM-yyyy");
                string time = DateTime.Now.ToShortTimeString();
                Header = Title + date;

                using (var writer = new StreamWriter("E:\\Customer Ledgers\\" + Header + ".csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(_ModelList);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
