namespace MWL.ReviewService.Api.DTOs
{
    public record class PaginationDTO<TDTO> where TDTO : class
    {
        public int CurrentPage { get; set; }
        public int Count { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<TDTO> Result { get; set; }
    }
}
