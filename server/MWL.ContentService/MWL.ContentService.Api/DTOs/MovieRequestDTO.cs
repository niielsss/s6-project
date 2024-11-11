using MWL.ContentService.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace MWL.ContentService.Api.DTOs
{
    public record class MovieRequestDTO
    {
        [Required(ErrorMessage = "The Title field is required.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "The PlotSummary field is required.")]
        public string PlotSummary { get; set; }
        [Required(ErrorMessage = "The ReleaseDate field is required.")]
        public DateTime ReleaseDate { get; set; }
        [Required(ErrorMessage = "The Genre field is required.")]
        public Genre Genre { get; set; }
        [Required(ErrorMessage = "The Duration field is required.")]
        public int Duration { get; set; }
        [Required(ErrorMessage = "The ProductionCompany field is required.")]
        public string ProductionCompany { get; set; }
    }
}
