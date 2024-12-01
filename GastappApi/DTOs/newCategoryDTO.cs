namespace GastappApi.DTOs
{
    public class newCategoryDTO
    {
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
