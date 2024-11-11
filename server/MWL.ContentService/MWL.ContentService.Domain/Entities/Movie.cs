using MWL.ContentService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ContentService.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PlotSummary { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int GenreId { get; set; }
        public Genre Genre
        {
            get => (Genre)GenreId;
            set => GenreId = (int)value;
        }
        public int? DirectorId { get; set; }
        public Person? Director { get; set; }
        public List<Person> Cast { get; set; }
        public int Duration { get; set; }
        public string ProductionCompany { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set;}
        public Pagination<Review> Reviews { get; set; }
    }
}
