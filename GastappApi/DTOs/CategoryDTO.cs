namespace GastappApi.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public bool IsInitial { get; set; } = false;

    }
}
