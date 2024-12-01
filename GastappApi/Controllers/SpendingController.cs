using GastappApi.DTOs;
using GastappApi.Models;
using GastappApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GastappApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SpendingController(GastappDbContext dbContext, UserUtils userUtils) : ControllerBase
    {
        GastappDbContext _dbContext = dbContext;
        private UserUtils _userUtils = userUtils;

        [HttpGet("getSpendingById")]
        public async Task<ActionResult> GetSpendingById(int spendingId, int userId)
        {
            var spendingResult = await _dbContext.Spending.FirstOrDefaultAsync(p =>
                p.SpendingId == spendingId && p.UserId == userId);

            if (spendingResult == null)
            {
                return NotFound("Specified spending Id NOT FOUND");
            }

            return Ok(spendingResult);
        }

        [HttpGet("getSpendingsByCategory")]
        public async Task<ActionResult> GetSpendingByCategoryId(int categoryId, int userId)
        {
            var spendingList = await _dbContext.Spending
                .Where(p => p.Category.UserId == userId && p.CategoryId == categoryId)
                .Select(p => new NewSpendingDTO
                {
                    UserId = p.UserId,
                    Amount = p.Amount,
                    CategoryId = p.CategoryId,
                    Title = p.Title,
                    Description = p.Description
                }).ToListAsync();

            return Ok(spendingList);
        }


        [HttpPost("newCategory")]
        public async Task<ActionResult<newCategoryDTO>> CreateNewCategory(newCategoryDTO category)
        {
            var newCategory = new SpendingCategory
            {
                UserId = category.UserId,
                CategoryName = category.CategoryName
            };
            await _dbContext.SpendingCategories.AddAsync(newCategory);
            await _dbContext.SaveChangesAsync();
            return Ok(newCategory);
        }

        [HttpPost("newSpending")]
        public async Task<ActionResult> CreateNewSpending(NewSpendingDTO spending)
        {
            if (spending.CategoryId <= 0 || spending.CategoryId == null)
            {
                var initialCategory = await _dbContext.SpendingCategories
                    .FirstOrDefaultAsync(p => p.UserId == spending.UserId && p.IsInitial);

                if (initialCategory == null)
                    return BadRequest("Default user category not found ¿¿WHY???");

                spending.CategoryId = initialCategory.CategoryId;
            }

            var newSpending = new Spending
            {
                UserId = spending.UserId,
                CategoryId = spending.CategoryId ?? 0,
                Amount = spending.Amount,
                Description = spending.Description,
                Title = spending.Title,
            };

            await _dbContext.Spending.AddAsync(newSpending);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("getMyCategories")]
        public async Task<ActionResult<CategoryDTO>> GetMyCategories()
        {
            var userId = GetUserIdFromToken();
            var categories = await _dbContext.SpendingCategories
                .Where(c => c.UserId == userId)
                .Select(c => new CategoryDTO
                {
                    CategoryId = c.CategoryId,
                    IsInitial = c.IsInitial,
                    CategoryName = c.CategoryName
                })
                .ToListAsync();
            return Ok(categories);
        }

        [HttpPost("createSpending")]
        public async Task<ActionResult<bool>> CreateSpending(NewSpendingDTO spending)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var newSpending = new Spending
                {
                    Amount = spending.Amount,
                    CategoryId = spending.CategoryId ?? 0,
                    Description = spending.Description,
                    Title = spending.Title,
                    UserId = userId
                };

                _dbContext.Spending.Add(newSpending);
                await _dbContext.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(false);
            }

        }

        private int GetUserIdFromToken()
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return -1;
            }

            return _userUtils.GetUserIdFromToken(authorizationHeader.Substring("Bearer ".Length).Trim());
        }

    }
}