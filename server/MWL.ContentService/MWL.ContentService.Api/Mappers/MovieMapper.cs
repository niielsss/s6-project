using AutoMapper;
using MWL.ContentService.Api.DTOs;
using MWL.ContentService.Domain.Entities;
using MWL.ContentService.Domain.Models;

namespace MWL.ContentService.Api.Mappers
{
    public class MovieMapper : Profile
    {
        public MovieMapper()
        {
            CreateMovieMapper();
        }

        private void CreateMovieMapper()
        {
            CreateMap<MovieRequestDTO, Movie>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.DirectorId, y => y.Ignore())
                .ForMember(x => x.Director, y => y.Ignore())
                .ForMember(x => x.Cast, y => y.Ignore())
                .ForMember(x => x.CreatedAt, y => y.Ignore())
                .ForMember(x => x.UpdatedAt, y => y.Ignore());

            CreateMap<Movie, MovieResponseDTO>();

            CreateMap<MovieFilterDTO, MovieFilter>();

            CreateMap<Pagination<Movie>, PaginationDTO<MovieResponseDTO>>()
                .AfterMap((source, converted, context) =>
                {
                    converted.Result = context.Mapper.Map<List<MovieResponseDTO>>(source.Result);
                });

            CreateMap<Movie, MovieWithReviewsResponse>()
                .ForMember(x => x.Reviews, y => y.MapFrom(z => z.Reviews));

            CreateMap<Review, ReviewResponseDTO>();

            CreateMap<Pagination<Review>, PaginationDTO<ReviewResponseDTO>>()
                .AfterMap((source, converted, context) =>
                {
                    converted.Result = context.Mapper.Map<List<ReviewResponseDTO>>(source.Result);
                });
        }
    }
}
