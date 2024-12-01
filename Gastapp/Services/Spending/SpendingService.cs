using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gastapp.Data;
using Gastapp.Models;
using Gastapp.Services.User;
using Microsoft.EntityFrameworkCore;
using Refit;
using Syncfusion.Maui.DataSource.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Gastapp.Services.Spending
{
    public class SpendingService(GastappDbContext dbContext)
    {
        private readonly GastappDbContext _dbContext = dbContext;

        public ObservableCollection<Models.Spending> GetSpendingByRange(DateTime start, DateTime end)
        {
            try
            {
                end = end.AddDays(1).AddSeconds(-1);
                return _dbContext.Spending
                    .Where(s => s.SpendingDate >= start && s.SpendingDate <= end)
                    .OrderBy(x => x.SpendingDate)
                    .ToObservableCollection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new ObservableCollection<Models.Spending>();
            }

        }
        public ObservableCollection<DateTime> GetDatesWithSpending()
        {
            try
            {
                var dates = _dbContext.Spending
                    .Select(x => x.SpendingDate)
                    .Distinct()
                    .OrderByDescending(x => x)
                    .ToObservableCollection();
                return dates;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new ObservableCollection<DateTime>();
            }
        }
        public bool CreateNewCategory(string name)
        {
            try
            {
                var newCategory = new Category()
                {
                    CategoryName = name,
                };
                _dbContext.Categories.Add(newCategory);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }
        public bool DeleteSpending(Models.Spending item)
        {
            try
            {
                _dbContext.Spending.Remove(item);
                var result = _dbContext.SaveChanges();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }
        public double GetTodayAmountAsync()
        {
            try
            {
                return _dbContext.Spending
                    .Where(s => s.SpendingDateOnly == DateOnly.FromDateTime(DateTime.Now))
                    .Sum(s => (double)s.Amount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public async Task<bool> CreateSpendingAsync(Models.Spending spending)
        {
            try
            {
                await _dbContext.Spending.AddAsync(spending);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

        public ObservableCollection<Models.Spending> GetTodaySpendings(DateTime date)
        {
            try
            {
                return _dbContext.Spending
                    .Where(s => s.SpendingDateOnly == DateOnly.FromDateTime(date))
                    .OrderBy(x=> x.SpendingDate)
                    .ToObservableCollection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new ObservableCollection<Models.Spending>();
            }
        }

        public async Task<ObservableCollection<Category>> GetMyCategoriesAsync()
        {
            try
            {
                return _dbContext.Categories
                    .OrderBy(c => c.CategoryName.ToLower())
                    .ToObservableCollection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ObservableCollection<Category>();
            }
        }
    }
}