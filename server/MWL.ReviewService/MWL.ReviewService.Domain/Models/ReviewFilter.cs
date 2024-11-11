using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ReviewService.Domain.Models
{
    public class ReviewFilter
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string OrderBy { get; set; } = "id";
        public string SortBy { get; set; } = "asc";
    }
}
