using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ContentService.Domain.Entities.Extensions
{
    public static class MovieExtensions
    {
        public static void SetCreatedDate(this Movie movie)
        {
            movie.CreatedAt = DateTime.Now.ToUniversalTime();
        }

        public static void SetUpdatedDate(this Movie movie)
        {
            movie.UpdatedAt = DateTime.Now.ToUniversalTime();
        }
    }
}
