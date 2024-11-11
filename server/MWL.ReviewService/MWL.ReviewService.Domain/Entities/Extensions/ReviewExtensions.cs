using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ReviewService.Domain.Entities.Extensions
{
    public static class ReviewExtensions
    {
        public static bool IsValidRating(this Review review)
        {
            return review.Rating >= 1 && review.Rating <= 5;
        }

        public static bool IsValidComment(this Review review)
        {
            return !string.IsNullOrWhiteSpace(review.Comment);
        }

        public static void SetCreatedAt(this Review review)
        {
            review.CreatedAt = DateTime.Now.ToUniversalTime();
        }
    }
}
