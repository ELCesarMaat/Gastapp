namespace GastappApi.DTOs
{
    public class NewSpendingDTO
    {

        public int UserId { get; set; }

        public int? CategoryId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; } = null;

        public decimal Amount { get; set; }
        public DateTime SpendingDate { get; set; } = DateTime.UtcNow;
        public DateOnly SpendingDateOnly { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
