namespace MWL.ReviewService.Api.DTOs
{
    public class ReviewFilterDTO
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string UserId { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string OrderBy { get; set; } = "id";
        public string SortBy { get; set; } = "asc";
    }
}
