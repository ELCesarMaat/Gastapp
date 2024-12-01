using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gastapp.Models;
using Refit;

namespace Gastapp.Services.Spending
{
    public interface ISpendingService
    {
        [Get("/Spending/getMyCategories")]
        public Task<List<Category>> GetMyCategoriesAsync([Authorize]string token);

        [Post("/Spending/createSpending")]
        public Task<bool> CreateSpendingAsync(Models.Spending spending, [Authorize]string token);
    }

}
