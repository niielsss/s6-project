using MWL.ContentService.Domain.Entities;

namespace MWL.ContentService.Api.DTOs
{
    public record class MovieResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PlotSummary { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Genre Genre { get; set; }
        public int Duration { get; set; }
        public string ProductionCompany { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
