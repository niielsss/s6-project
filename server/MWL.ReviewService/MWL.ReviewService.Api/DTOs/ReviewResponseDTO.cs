namespace MWL.ReviewService.Api.DTOs
{
    public class ReviewResponseDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int MovieId { get; set; }
        public string UserId { get; set; }
    }
}
