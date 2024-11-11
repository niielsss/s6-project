using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.IdentityService.Domain.Models
{
    public class Pagination<T> where T : class
    {
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(decimal.Divide(Count, PageSize)) : 1;
        public IEnumerable<T> Result { get; set; } = [];
    }
}
