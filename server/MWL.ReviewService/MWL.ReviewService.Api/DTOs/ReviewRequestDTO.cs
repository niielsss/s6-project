namespace MWL.ReviewService.Api.DTOs
{
    public class ReviewRequestDTO
    {
        public string UserId { get; set; }
        public int MovieId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
